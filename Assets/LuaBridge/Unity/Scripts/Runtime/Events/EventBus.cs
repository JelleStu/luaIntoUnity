using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Services.Telemetry;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LuaBridge.Core.Events
{
    public sealed class EventBus
    {
        [DebuggerNonUserCode]
        private readonly struct IObservableAwaiter<T> : INotifyCompletion
        {
            private readonly IObservable<T> _subscription;
            private readonly CancellationToken _token;
            public bool IsCompleted => _subscription.IsCompleted;

            public IObservableAwaiter(IObservable<T> subscription, CancellationToken token)
            {
                _subscription = subscription;
                _token = token;
            }

            public void OnCompleted(Action continuation) => _subscription.Completed += continuation;
            public T GetResult() => _subscription.Result;
        }

        private interface IObservable<T> : ISubscription<T>
        {
            public bool IsCompleted { get; }
            public T Result { get; }
            public event Action Completed;
            public IObservableAwaiter<T> GetAwaiter();
        }

        private class Observable<T> : IObservable<T>
        {
            private readonly Topic _topic;
            private readonly CancellationToken _token;
            private readonly Predicate<T> _filter;
            public bool IsCompleted => _token.IsCancellationRequested || _isCompleted;
            private bool _isCompleted = false;
            public T Result { get; private set; }
            public event Action Completed;

            public IObservableAwaiter<T> GetAwaiter()
            {
                return new IObservableAwaiter<T>(this, _token);
            }

            internal Observable(Topic topic, CancellationToken token, Predicate<T> filter = null)
            {
                _topic = topic;
                _token = token;
                _filter = filter;
            }

            public void Invoke(T @event)
            {
                if (_token.IsCancellationRequested || _filter == null || _filter.Invoke(@event))
                {
                    Result = @event;
                    _isCompleted = true;
                    Completed?.Invoke();
                }
            }

            public void Dispose()
            {
                _topic.Unsubscribe(this);
            }
        }

        private interface ISubscription<in T> : IDisposable
        {
            public void Invoke(T @event);
        }

        private class Subscription<T> : ISubscription<T>
        {
            private readonly Topic _topic;
            private readonly Action<T> _event;
            private readonly Predicate<T> _filter;

            internal Subscription(Topic topic, Action<T> @event, Predicate<T> filter = null)
            {
                _topic = topic;
                _event = @event;
                _filter = filter;
            }

            public void Invoke(T @event)
            {
                if (_filter == null || _filter.Invoke(@event))
                    _event.Invoke(@event);
            }

            public void Dispose()
            {
                _topic.Unsubscribe(this);
            }
        }

        private class Topic
        {
            private ITelemetry _telemetry;
            private readonly List<IDisposable> _subs;

            public Topic(ITelemetry telemetry)
            {
                _telemetry = telemetry;
                _subs = new List<IDisposable>();
            }

            internal void SetTelemetry(ITelemetry telemetry)
            {
                _telemetry = telemetry;
            }

            internal void Unsubscribe(IDisposable sub)
            {
                _subs.Remove(sub);
            }

            internal void Publish<T>(T @event)
            {
                if (_subs.Count == 0)
                    return;
                _telemetry?.Log(@event);
                for (int i = 0; i < _subs.Count; i++)
                    if (_subs[i] is ISubscription<T> generic)
                        generic.Invoke(@event);
                    else if (_subs[i] is ISubscription<object> obj)
                        obj.Invoke(@event);
            }

            public void Replay(object @event)
            {
                if (_subs.Count == 0)
                    return;

                var type = typeof(ISubscription<>).MakeGenericType(@event.GetType());
                var method = type.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < _subs.Count; i++)
                    method.Invoke(_subs[i], new[] { @event });
            }

            internal IDisposable Subscribe<T>(Action<T> @event, Predicate<T> filter = null)
            {
                var subscription = new Subscription<T>(this, @event, filter);
                _subs.Add(subscription);
                return subscription;
            }

            internal IObservable<T> SubscribeObservable<T>(CancellationToken token, Predicate<T> filter)
            {
                var subscription = new Observable<T>(this, token, filter);
                _subs.Add(subscription);
                return subscription;
            }

            internal void PurgeAll()
            {
                for (int i = 0; i < _subs.Count; i++)
                    _subs[i].Dispose();
            }
        }

        private static Dictionary<Type, Topic> _subscriptions;
        private static ITelemetry _telemetry;
        private static bool _isReplaying;

        private EventBus()
        {
            PurgeSubscriptions();
            _subscriptions = new Dictionary<Type, Topic>();
            _telemetry?.Dispose();
            _telemetry = null;
            _isReplaying = false;
        }

        private void SetTelemetry(ITelemetry telemetry)
        {
            _telemetry?.Disable();
            _telemetry = telemetry;
            foreach (var topic in _subscriptions)
                topic.Value.SetTelemetry(_telemetry);
            if (telemetry != null)
            {
                _telemetry.RegisterReplayFn(Replay, ReplayToggle);
                _telemetry.Enable();
            }
        }

        private void ReplayToggle(bool enableReplay)
        {
            _isReplaying = enableReplay;
        }

        private void PurgeSubscriptions()
        {
            if (_subscriptions == null)
                return;

            Debug.LogWarning("<color=green>Purging subscriptions! Probably because you used the Bus in Editor-mode or Testing!</color>");
            foreach (var topic in _subscriptions)
                topic.Value.PurgeAll();
        }

        public static IDisposable Subscribe<T>(Action<T> callback, Predicate<T> filter = null)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
                _subscriptions.Add(typeof(T), new Topic(_telemetry));
            return _subscriptions[typeof(T)].Subscribe(callback, filter);
        }

        public static IDisposable Subscribe(Type t, Action<object> callback, Predicate<object> filter = null)
        {
            if (!_subscriptions.ContainsKey(t))
                _subscriptions.Add(t, new Topic(_telemetry));
            return _subscriptions[t].Subscribe(callback, filter);
        }

        public static async Task<T> SubscribeAsync<T>(CancellationToken token, Predicate<T> filter = null)
        {
            if (!_subscriptions.ContainsKey(typeof(T)))
                _subscriptions.Add(typeof(T), new Topic(_telemetry));

            using var observable = _subscriptions[typeof(T)].SubscribeObservable(token, filter);
            var response = await observable;
            return response;
        }

        public static async Task<object> SubscribeAsync(Type t, CancellationToken token, Predicate<object> filter = null)
        {
            if (!_subscriptions.ContainsKey(t))
                _subscriptions.Add(t, new Topic(_telemetry));

            using var observable = _subscriptions[t].SubscribeObservable(token, filter);
            var response = await observable;
            return response;
        }

        private static void Replay(object @event)
        {
            if (_subscriptions.TryGetValue(@event.GetType(), out var subs))
                subs.Replay(@event);
        }

        public static void Publish<T>(T @event)
        {
            if (_isReplaying)
                return;
            if (_subscriptions.TryGetValue(@event.GetType(), out var subs))
                subs.Publish(@event);
        }

        public static void Publish<T, TResponse>(T @event, Action<TResponse> oneTimeResponse, Predicate<TResponse> filter = null)
        {
            IDisposable listener = null;

            void UnSub(TResponse response)
            {
                oneTimeResponse.Invoke(response);
                listener.Dispose();
            }

            listener = Subscribe(UnSub, filter);
            if (_isReplaying)
                return;
            Publish(@event);
        }

        public static async Task<TResponse> PublishAsync<T, TResponse>(T @event, CancellationToken cancellationToken, Predicate<TResponse> filter = null)
        {
            var sub = SubscribeAsync(cancellationToken, filter);
            TResponse response = default;
            if (_isReplaying)
                response = await sub;
            else
            {
                Publish(@event);
                response = await sub;
            }

            sub.Dispose();
            return response;
        }

        public static class Factory
        {
            private static EventBus _eventBus;

            public static void Create()
            {
                if (_eventBus != null && Application.isPlaying)
                    throw new NotSupportedException("You are not allowed to instantiate a new Event bus at runtime!");
                _eventBus = new EventBus();
            }

            public static void SetTelemetry(ITelemetry telemetry)
            {
                _eventBus.SetTelemetry(telemetry);
            }
        }
    }
}