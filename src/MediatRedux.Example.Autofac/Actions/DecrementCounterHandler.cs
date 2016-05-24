using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRedux.Example.Autofac.Actions
{
    public class DecrementCounterHandler : ReduxActionHandler<DecrementCounter, State>
    {
        protected override void HandleAction(State state, DecrementCounter action)
        {
            state.Counter--;
        }
    }
}
