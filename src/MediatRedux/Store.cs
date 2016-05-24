using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MediatRedux
{
        
    public interface IStore<TState> : IObservable<TState>
    {
        IReduxAction<TState> Dispatch(IReduxAction<TState> action);

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

        public IReduxAction<TState> Dispatch(IReduxAction<TState> action)
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

        private IReduxAction<TState> InnerDispatch(IReduxAction<TState> action)
        {
            lock (_syncRoot)
            {
                // Set the current state in the action
                //
                action.State = _lastState;

                _lastState = _mediator.Send(action);
            }
            _stateSubject.OnNext(_lastState);
            return action;
        }
    }
}
