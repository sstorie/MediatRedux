using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRedux.Example.Autofac.Actions
{
    public class IncrementCounterHandler : ActionHandler<IncrementCounter>
    {
        protected override void HandleAction(State state, IncrementCounter action)
        {
            state.Counter++;
        }
    }
}
