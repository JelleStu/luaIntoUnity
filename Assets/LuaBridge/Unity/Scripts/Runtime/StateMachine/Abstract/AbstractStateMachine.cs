using System;
using System.Collections.Generic;
using System.Linq;
using LuaBridge.Core.Extensions;
using UnityEngine;

namespace LuaBridge.Core.StateMachine.Abstract
{
    public abstract class AbstractStateMachine : IStateMachine
    {
        private Dictionary<Type, IState> _suspendedStates;
        private Dictionary<string, IState> _availableStates;
        public event Action<IState> StateChanged;
        public event Action Quitted;
        public IState CurrentState { get; private set; }

        protected AbstractStateMachine()
        {
            _suspendedStates = new Dictionary<Type, IState>();
            _availableStates = new();
        }

        protected AbstractStateMachine(HashSet<IState> availableStates = null) : this()
        {
            _availableStates = availableStates?.ToDictionary(s => s.Name, s => s) ?? new();
        }

        public virtual void Start(IState state)
        {
            CurrentState = state;
            CurrentState.OnEnter(this);
            OnStateChanged(CurrentState);
        }

        protected void AddAvailableState(IState state)
        {
            _availableStates[state.Name] = state;
        }

        protected IState GetAvailableState(string name)
        {
            return _availableStates.TryGetValue(name, out var state) ? state : null;
        }

        public virtual void Start(string stateName)
        {
            if (!_availableStates.TryGetValue(stateName, out var state))
                throw new ArgumentException($"Cannot find a state named {stateName} in {GetType().Name}");
            CurrentState = state;
            CurrentState.OnEnter(this);
            OnStateChanged(CurrentState);
        }

        public void SetState(string stateName, bool suspendCurrentState = false)
        {
            if (!_availableStates.TryGetValue(stateName, out var state))
                throw new ArgumentException($"Cannot find a state named {stateName} in {GetType().Name}");

            if (_suspendedStates.ContainsKey(state.GetType()))
                Sustain(state, suspendCurrentState);
            else
                SetState(state, suspendCurrentState);
        }

        public void SetState(IState state, bool suspendCurrentState = false)
        {
            if (_suspendedStates.ContainsKey(state.GetType()))
            {
                Sustain(state, suspendCurrentState);
                return;
            }

            if (suspendCurrentState)
            {
                _suspendedStates.Add(CurrentState.GetType(), CurrentState);
                CurrentState.OnSuspend();
            }
            else
            {
                CurrentState.OnExit();
            }

            Debug.Log($"Changing state from {CurrentState.GetType().Name} to {state.GetType().Name}");
            _availableStates[state.Name] = state;
            CurrentState = state;
            CurrentState.OnEnter(this);
            OnStateChanged(CurrentState);
        }

        public void Sustain(IState state, bool suspendCurrentState = false)
        {
            if (_suspendedStates.TryGetValue(state.GetType(), out _))
            {
                _suspendedStates.Remove(state.GetType());
                if (suspendCurrentState)
                {
                    _suspendedStates.Add(CurrentState.GetType(), CurrentState);
                    CurrentState.OnSuspend();
                }
                else
                {
                    CurrentState.OnExit();
                }

                Debug.Log($"Changing state from {CurrentState.GetType().Name} to {state.GetType().Name}");
                _availableStates[state.Name] = state;
                CurrentState = state;
                CurrentState.OnSustain();
                OnStateChanged(CurrentState);
            }
            else
            {
                SetState(state, suspendCurrentState);
            }
        }

        public void Sustain<T>(bool suspendCurrentState = false) where T : IState
        {
            if (_suspendedStates.TryGetValue(typeof(T), out var state))
            {
                _suspendedStates.Remove(state.GetType());
                if (suspendCurrentState)
                {
                    _suspendedStates.Add(CurrentState.GetType(), CurrentState);
                    CurrentState.OnSuspend();
                }
                else
                {
                    CurrentState.OnExit();
                }

                Debug.Log($"Changing state from {CurrentState.GetType().Name} to {state.GetType().Name}");
                _availableStates[state.Name] = state;
                CurrentState = state;
                CurrentState.OnSustain();
                OnStateChanged(CurrentState);
            }
            else
            {
                Debug.LogError($"Cannot Sustain state {typeof(T)}, it has not been suspended! State remains {CurrentState.GetType()}!");
            }
        }

        public void Sustain(string stateName, bool suspendCurrentState = false)
        {
            var state = _suspendedStates.FirstOrDefault(s => s.Value.Name == stateName).Value;
            if (state != null)
            {
                _suspendedStates.Remove(state.GetType());
                if (suspendCurrentState)
                {
                    _suspendedStates.Add(CurrentState.GetType(), CurrentState);
                    CurrentState.OnSuspend();
                }
                else
                {
                    CurrentState.OnExit();
                }

                Debug.Log($"Changing state from {CurrentState.GetType().Name} to {state.GetType().Name}");
                _availableStates[state.Name] = state;
                CurrentState = state;
                CurrentState.OnSustain();
                OnStateChanged(CurrentState);
            }
            else
            {
                Debug.LogError($"Cannot Sustain state {stateName}, it has not been suspended! State remains {CurrentState.GetType()}!");
            }
        }

        protected virtual void PreQuit()
        {
            
        }

        public void Quit()
        {
            PreQuit();
            foreach (var state in AggregateStates())
                state.OnExit();
            CurrentState = null;
            _suspendedStates = new Dictionary<Type, IState>();
            _availableStates = new Dictionary<string, IState>();
            Debug.Log($"Quit {GetType()}");
            OnQuit();
        }

        private void OnQuit()
        {
            Quitted?.Invoke();
        }

        private void OnStateChanged(IState state)
        {
            StateChanged?.Invoke(state);
        }

        private IEnumerable<IState> AggregateStates()
        {
            return _suspendedStates.Values.Concat(_availableStates.Values).DistinctBy(t => t.GetType());
        }
    }
}