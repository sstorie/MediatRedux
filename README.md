# MediatRedux

MediatRedux is a simple combination of Jimmy Bogard's [MediatR library](https://github.com/jbogard/MediatR) with the Redux pattern implemented by Guillaume Salles's [Redux.NET project](https://github.com/GuillaumeSalles/redux.NET). It provides the functionality behind the redux pattern using Rx, but with the flexibility provided by MediatR.

## Table of contents

- [Motivation](#motivation)
- [License](#license)


## Motivation

I created this library because the one thing about redux implementations I never liked
is they either rely on strings to identify the actions, or you have some sort of large switch/if block that determines how to modify the state based on an action. I also don't like the large reducer function that results...and I know you can split it up, but I much prefer the concise implementation provided by MediatR for handling a specific action (or Request in MediatR speak).


## Installation

TBD, but hopefully using straight nuget.

## Usage

Using this library is simple. Just create your instance of IMediatR however you like, and provide it to the Store during construction:

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

```


## License

MIT
