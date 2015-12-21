# PcgRandom Documentation

PcgRandom is a .NET port of the [C Library](http://www.pcg-random.org/using-pcg-c.html)
from [pcg-random.org](http://www.pcg-random.org/).

It provides a high-level API (which is a drop-in replacement for `System.Random`) and a low-level
API (which is roughly equivalent to the "high level" C API; there is no equivalent in this library
to the "low level" C API).

## High-Level API

The `PcgRandom` library provides an implementation of `System.Random` that produces pseudorandom
numbers that are less predictable and pass more statistical tests for randomness.

The API is the same as [`System.Random`](https://msdn.microsoft.com/en-US/library/system.random.aspx):

```
System.Random rng = new Pcg.PcgRandom();
int randomNumber = rng.Next();
int boundedNumber = rng.Next(6); // die roll
int rangedNumber = rng.Next(1000, 2000); // from 1000-1999
double randomDouble = rng.NextDouble();
byte[] bytes = new byte[16];
rng.NextBytes(bytes); // fill an array with random numbers
```

## Low-Level API

The primary low-level type is [`Pcg32`](pcg32.md), which implements the `pcg32_random_t`
type described in the [C library documentation](http://www.pcg-random.org/using-pcg-c.html).

`Pcg32Single` has the same API, but provides a *single* stream. Given the large period, if
multiple RNGs are initialized properly, it is unlikely that the outputs will ever coincide.
