# MediatRedux

MediatRedux is a simple combination of Jimmy Bogard's [MediatR library](https://github.com/jbogard/MediatR) with the Redux pattern inspired by Guillaume Salles's [Redux.NET project](https://github.com/GuillaumeSalles/redux.NET). It provides the power of redux using reactive extensions, but with the flexibility and strict types provided by MediatR.

[![Build status](https://ci.appveyor.com/api/projects/status/jjk6hi4el8wd075t?svg=true)](https://ci.appveyor.com/project/sstorie/mediatredux) [![NuGet](https://img.shields.io/nuget/v/MediatRedux.svg?maxAge=2592000)]()

## Table of contents

- [Motivation](#motivation)
- [Installation](#installation)
- [TODO](#todo)
- [Examples](#examples)
- [License](#license)


## Motivation

I created this library because the one thing about redux implementations I never liked
is they either rely on strings to identify the actions (like this example from the Angular 2 [@ngrx/store library](https://github.com/ngrx/store), which is awesome btw):

```ts
// counter.ts
import { ActionReducer, Action } from '@ngrx/store';

export const INCREMENT = 'INCREMENT';
export const DECREMENT = 'DECREMENT';
export const RESET = 'RESET';

export const counterReducer: ActionReducer<number> = (state: number = 0, action: Action) => {
	switch (action.type) {
		case INCREMENT:
			return state + 1;

		case DECREMENT:
			return state - 1;

		case RESET:
			return 0;

		default:
			return state;
	}
}
```

...or you have some sort of type check and casting to deal with (like this *very* simple example from the [Redux.NET library](https://github.com/GuillaumeSalles/redux.NET)):

```csharp
namespace Redux.Counter.Universal
{
    public static class CounterReducer
    {
        public static int Execute(int state, IAction action)
        {
            if(action is IncrementAction)
            {
                return state + 1;
            }

            if(action is DecrementAction)
            {
                return state - 1;
            }

            return state;
        }
    }
}
```

I also don't like the large `switch`-based reducer function that usually results...and I know you can split it up, but I much prefer the concise implementation provided by MediatR to concretely tie a handler class to a given action.

With MediatR you have a simple handler class that provides the one spot to go for understanding how a specific action affects the state object. Since it's a single class it's very easy to test and reason about.

I can also easily add [decorators to implement the "middleware" idea](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/) used in many redux implementations. With this you can add state loggers, validation, or any other cross-cutting concern you might want in your redux pipeline.


## Installation

You should install MediatRedux using NuGet:

```
Install-Package MediatRedux
```

Running the command above will install MediatRedux and all required dependencies.

## Usage

Using this library is simple. Just create your instance of `IMediator` however you like ([many, many examples](https://github.com/jbogard/MediatR/tree/master/samples)), and provide it to the Store during construction. Now the store itself is an observable that will emit a new state whenever an action is processed, so you simply subscribe to it for any updates. Note, you can also use DistinctUntilChanged() to ensure you receive an update only when a specific slice of the state has changed ([reference](https://github.com/GuillaumeSalles/redux.NET#distinctuntilchanged-to-the-rescue)):

```csharp
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
    // Create the mediator using whatever IoC library you want
}

```

The library provides two base classes `ReduxAction<TState>` and `ReduxActionHandler<TAction, TState>` that help reduce the boilerplate required. Simply derive your actions from `ReduxAction` and derive your handlers from `ReduxActionHandler`. Then in each handler, override the `HandleAction(TState state, ReduxAction action)` method and manipulate the state object as needed based on the provided action. The base class will take care of creating the copy of state you need to satisfy the redux pattern. This example uses the following state class, actions and handlers:

```csharp
public class State
{
    public int Counter { get; set; }
}

public class IncrementCounter : ReduxAction<State> {}

public class IncrementCounterHandler : ReduxActionHandler<IncrementCounter, State>
{
    protected override void HandleAction(State state, IncrementCounter action)
    {
        state.Counter++;
    }
}

public class DecrementCounter : ReduxAction<State> {}

public class DecrementCounterHandler : ReduxActionHandler<DecrementCounter, State>
{
    protected override void HandleAction(State state, DecrementCounter action)
    {
        state.Counter--;
    }
}
```

## TODO

- Add tests
- Add a decorator example that provides action/state logging
- Convert to a PCL

## Examples

- Console app using Autofac - [MediatRedux.Example.Autofac](https://github.com/sstorie/MediatRedux/tree/master/src/MediatRedux.Example.Autofac)

## License

MIT
