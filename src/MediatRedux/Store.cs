using System;
using System.Reactive.Subjects;
using MediatR;

namespace MediatRedux
{
    /// <summary>
    /// The public interface for the Store class
    /// </summary>
    /// <remarks>
    /// Note, this was taken pretty much from Redux.NET and adapted with MediatR
    /// https://github.com/GuillaumeSalles/redux.NET
    /// </remarks>
    /// <typeparam name="TState">The type of state class</typeparam>
    public interface IStore<TState> : IObservable<TState>
    {
        ReduxAction<TState> Dispatch(ReduxAction<TState> action);

        TState GetState();
    }

    public class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private readonly IMediator _mediator;
        private TState _lastState;

        public Store(IMediator mediator, TState initialState = default(TState))
        {
            _mediator = mediator;
            _lastState = initialState;
            _stateSubject.OnNext(_lastState);
        }

        public ReduxAction<TState> Dispatch(ReduxAction<TState> action)
        {
            return InnerDispatch(action);
        }

        public TState GetState()
        {
            return _lastState;
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject.Subscribe(observer);
        }

        private ReduxAction<TState> InnerDispatch(ReduxAction<TState> action)
        {
            lock (_syncRoot)
            {
                // Set the current state in the action
                //
                action.State = _lastState;

                // Fetch a new state using the mediator
                //
                _lastState = _mediator.Send(action);
            }

            _stateSubject.OnNext(_lastState);

            return action;
        }
    }
}
