# MediatRedux

MediatRedux is a simple combination of Jimmy Bogard's [MediatR library](https://github.com/jbogard/MediatR) with the Redux pattern implemented by Guillaume Salles's [Redux.NET project](https://github.com/GuillaumeSalles/redux.NET). It provides the functionality behind the redux pattern using Rx, but with the flexibility provided by MediatR.

[![Build status](https://ci.appveyor.com/api/projects/status/jjk6hi4el8wd075t?svg=true)](https://ci.appveyor.com/project/sstorie/mediatredux) [![NuGet](https://img.shields.io/nuget/v/MediatRedux.svg?maxAge=2592000)]()

## Table of contents

- [Motivation](#motivation)
- [Installation](#installation)
- [TODO](#todo)
- [Examples](#examples)
- [License](#license)


## Motivation

I created this library because the one thing about redux implementations I never liked
is they either rely on strings to identify the actions, or you have some sort of large switch/if block that determines how to modify the state based on an action. I also don't like the large reducer function that results...and I know you can split it up, but I much prefer the concise implementation provided by MediatR for handling a specific action (or Request in MediatR speak).

With MediatR you have a simple handler class that provides the one spot to go for understanding how a specific action affects the state object. Since it's a single class it's very easy to test and reason about.

I can also easily add [decorators to implement the "middleware" idea](https://lostechies.com/jimmybogard/2014/09/09/tackling-cross-cutting-concerns-with-a-mediator-pipeline/) used in many redux implementations. With this you can add state loggers, validation, or any other cross-cutting concern you might want in your redux pipeline.


## Installation

TBD, but hopefully using straight nuget.

## Usage

Using this library is simple. Just create your instance of `IMediator` however you like, and provide it to the Store during construction:

```
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

This example uses the following state class, actions and handlers:

```
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
- Convert to a PCL

## Examples

- Console app using Autofac - [MediatRedux.Example.Autofac](https://github.com/sstorie/MediatRedux/tree/master/src/MediatRedux.Example.Autofac)

## License

MIT
