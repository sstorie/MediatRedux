using AutoMapper;
using MediatR;

namespace MediatRedux
{
    /// <summary>
    /// A base class to reduce the boilerplate assocaited with creating action 
    /// handlers 
    /// </summary>
    /// <typeparam name="TAction">The specific type of action this handler can handle</typeparam>
    /// <typeparam name="TState">The state object type</typeparam>
    public abstract class ReduxActionHandler<TAction, TState> : IRequestHandler<TAction, TState> where TAction : ReduxAction<TState>
    {
        public TState Handle(TAction action)
        {
            // Create a copy of the initial state to satisfy the redux
            //  requirement of always emitting a new object
            //
            var stateCopy = Mapper.Map<TState>(action.State);

            HandleAction(stateCopy, action);

            return stateCopy;
        }

        /// <summary>
        /// The method that all commands must implement to update the state
        /// </summary>
        /// <param name="state">The state object that should be updated.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected abstract void HandleAction(TState state, TAction action);
    }
}
