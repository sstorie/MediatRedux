using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRedux.Example.Autofac.Actions
{
    public class DecrementCounterHandler : ActionHandler<DecrementCounter>
    {
        protected override void HandleAction(State state, DecrementCounter action)
        {
            state.Counter--;
        }
    }
}
