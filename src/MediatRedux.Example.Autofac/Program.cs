using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using MediatRedux.Example.Autofac.Actions;

namespace MediatRedux.Example.Autofac
{
    class Program
    {
        static void Main(string[] args)
        {
            var mediator = BuildMediator();

            var store = new Store<State>(mediator, new State {Counter = 0});

            store
                .DistinctUntilChanged(x => new { x.Counter })
                .Subscribe(x => Console.WriteLine($"New state: counter - {x.Counter}"));

            store.Dispatch(new IncrementCounter());
            store.Dispatch(new IncrementCounter());
            store.Dispatch(new IncrementCounter());
            store.Dispatch(new DecrementCounter());

            Console.ReadLine();
        }

        private static IMediator BuildMediator()
        {
            var builder = new ContainerBuilder();

            // Register our specific action handlers
            //
            builder.RegisterAssemblyTypes(typeof(State).GetTypeInfo().Assembly).AsImplementedInterfaces();

            // Boilerplate MediatR code
            //
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            var mediator = builder.Build().Resolve<IMediator>();

            return mediator;
        }
    }
}
