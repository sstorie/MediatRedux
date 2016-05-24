using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRedux.Example.Autofac.Actions
{
    /// <summary>
    /// A base class specific to a given application to reduce the boilerplate
    /// required to create actions
    /// </summary>
    public class Action : IReduxAction<State>
    {
        public State State { get; set; }
    }
}
