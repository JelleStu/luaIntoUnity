using System;
using System.Collections.Generic;
using LuaBridge.Core.Extensions;

namespace LuaBridge.Core.Events
{
    /// <summary>
    /// Event transformer class to convert 1 event to anotherand relay they on the event bus.
    /// This helps break concrete dependencies
    /// </summary>
    /// <typeparam name="T1">type 1</typeparam>
    /// <typeparam name="T2">type 2</typeparam>
    public abstract class AbstractEventTransformer<T1, T2> : IDisposable
    {
        private IDisposable _sub;

        public abstract T2 From(T1 from);

        public IDisposable Subscribe()
        {
            _sub = EventBus.Subscribe<T1>(t1 => { EventBus.Publish<T2>(From(t1)); });
            return this;
        }

        public virtual void Dispose()
        {
            _sub.Dispose();
        }
    }

    /// <summary>
    /// Event transformer class to convert 1 event to another (and vice-versa) and relay they on the event bus.
    /// This helps break concrete dependencies
    /// </summary>
    /// <typeparam name="T1">type 1</typeparam>
    /// <typeparam name="T2">type 2</typeparam>
    public abstract class AbstractCrossEventTransformer<T1, T2> : IDisposable
    {
        private IDisposable[] _subs;
        private HashSet<T1> _t1Events;
        private HashSet<T2> _t2Events;

        public abstract T2 From(T1 from);
        public abstract T1 To(T2 to);

        public IDisposable Subscribe()
        {
            _subs = new[]
            {
                EventBus.Subscribe<T1>(t1 =>
                    {
                        var t2 = From(t1);
                        _t2Events.Add(t2);
                        EventBus.Publish<T2>(t2);
                    },
                    t1 => _t1Events.Remove(t1)),
                EventBus.Subscribe<T2>(t2 =>
                    {
                        var t1 = To(t2);
                        _t1Events.Add(t1);
                        EventBus.Publish<T1>(t1);
                    },
                    t2 => !_t2Events.Remove(t2)),
            };
            return this;
        }


        public virtual void Dispose()
        {
            _t1Events = null;
            _t2Events = null;
            _subs.Dispose();
        }
    }

    /// <summary>
    /// Event transformer class to convert 1 event to another (and vice-versa) and relay they on the event bus.
    /// This helps break concrete dependencies
    /// </summary>
    /// <typeparam name="T1">type 1</typeparam>
    /// <typeparam name="T2">type 2</typeparam>
    public abstract class AbstractResponseEventTransformer<T1, T2, T1Response, T2Response> : IDisposable
    {
        private IDisposable[] _subs;
        private HashSet<T1> _t1Events;
        private HashSet<T2> _t2Events;
        private HashSet<T1Response> _t1ResponseEvents;
        private HashSet<T2Response> _t2ResponseEvents;

        public abstract T2 From(T1 from);
        public abstract T1 To(T2 to);
        public abstract T2Response FromResponse(T1Response rep);
        public abstract T1Response ToResponse(T2Response to);

        public IDisposable Subscribe()
        {
            _subs = new[]
            {
                EventBus.Subscribe<T1>(t1 =>
                    {
                        var t2 = From(t1);
                        _t2Events.Add(t2);
                        EventBus.Publish<T2>(t2);
                    },
                    t1 => _t1Events.Remove(t1)),
                EventBus.Subscribe<T2>(t2 =>
                    {
                        var t1 = To(t2);
                        _t1Events.Add(t1);
                        EventBus.Publish<T1>(t1);
                    },
                    t2 => !_t2Events.Remove(t2)),

                EventBus.Subscribe<T1Response>(t1Response =>
                    {
                        var t2Response = FromResponse(t1Response);
                        _t2ResponseEvents.Add(t2Response);
                        EventBus.Publish<T2Response>(t2Response);
                    },
                    t1Response => _t1ResponseEvents.Remove(t1Response)),
                EventBus.Subscribe<T2Response>(t2Response =>
                    {
                        var t1Response = ToResponse(t2Response);
                        _t1ResponseEvents.Add(t1Response);
                        EventBus.Publish<T1Response>(t1Response);
                    },
                    t2Response => !_t2ResponseEvents.Remove(t2Response)),
            };
            return this;
        }


        public virtual void Dispose()
        {
            _t1Events = null;
            _t2Events = null;
            _t1ResponseEvents = null;
            _t2ResponseEvents = null;
            _subs.Dispose();
        }
    }
}