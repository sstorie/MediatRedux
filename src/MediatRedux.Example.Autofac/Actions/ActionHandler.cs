using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace MediatRedux.Example.Autofac.Actions
{
    /// <summary>
    /// A base class to reduce the boilerplate assocaited with creating action 
    /// handlers for the specific State object used in this class
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    public abstract class ActionHandler<TAction> : IRequestHandler<TAction, State> where TAction : IReduxAction<State>
    {
        public State Handle(TAction action)
        {
            // Create a copy of the initial state to satisfy the redux
            //  requirement of always emitting a new object
            //
            var stateCopy = Mapper.Map<State>(action.State);

            HandleAction(stateCopy, action);

            return stateCopy;
        }

        /// <summary>
        /// The method that all commands must implement to update the state
        /// </summary>
        /// <param name="state">The state object that should be updated.</param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected abstract void HandleAction(State state, TAction action);
    }
}
