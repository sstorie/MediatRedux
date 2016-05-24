using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MediatRedux
{
    public interface IReduxAction<TState> : IRequest<TState>
    {
        /// <summary>
        /// A settable property that will be loaded with the current state when
        /// the action is dispatched
        /// </summary>
        TState State { get; set; }
    }
}
