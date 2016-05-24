using MediatR;

namespace MediatRedux
{
    /// <summary>
    /// A base class specific to a given application to reduce the boilerplate
    /// required to create actions
    /// </summary>
    public class ReduxAction<TState> : IRequest<TState>
    {
        public TState State { get; set; }
    }
}
