## About

`PcgRandom` provides an implementation of `Random` that generates pseudorandom numbers using the [PCG](http://www.pcg-random.org/) family of random number generators. It is a .NET port (written entirely in C#) of the [C library](https://www.pcg-random.org/using-pcg-c.html) by Melissa O'Neill. You can use a `PcgRandom` instance anywhere you would use a `Random` instance.

## Basic Usage

```csharp
using Pcg;

// create a new random number generator based on the current time
Random random = new PcgRandom();

// or with a seed (to generate the same sequence of numbers)
// random = new PcgRandom(1);

// generate a random non-negative 32-bit integer
Console.WriteLine(random.Next());

// generate a random non-negative 64-bit integer
Console.WriteLine(random.NextInt64());

// generate a random number in a range, e.g., a dice roll; note that
// the upper bound is exclusive, so this generates numbers from 1 to 6
Console.WriteLine(random.Next(1, 7));

// generate a random Boolean
Console.WriteLine(random.Next(2) == 1);

// fill an array with random bytes
var bytes = new byte[16];
random.NextBytes(bytes);
Console.WriteLine(Convert.ToHexString(bytes));

// generate a random floating-point number
Console.WriteLine(random.NextDouble());

// generate a random floating point number from 5 to 15
Console.WriteLine(5.0 + random.NextDouble() * 10);
```

The above code will produce output similar to the following:

```
359607667
2470866491422793932
5
True
F0F1E514E052C05868664FA43BA9285D
0.3241723496466875
14.117484646849334
```

## Low-Level API

`PcgRandom` also provides a low-level API that is roughly equivalent to the the [PCG C High-Level API](https://www.pcg-random.org/using-pcg-c.html).

The `Pcg32` class is equivalent to the C `pcg32_random_t` type:

```csharp
// create a random Pcg32 instance, specifying a seed and a sequence identifier
var pcg32 = new Pcg32((ulong) random.NextInt64(), (ulong) random.NextInt64());

// generate a random unsigned 32-bit integer
Console.WriteLine(pcg32.GenerateNext());

// skip over 300 numbers in the sequence (without generating them), then generate the next random number
pcg32.Advance(300);
Console.WriteLine(pcg32.GenerateNext());

// advance the state all the way around so that the same number is generated
pcg32.Advance(ulong.MaxValue);
Console.WriteLine(pcg32.GenerateNext());
```

The the `Pcg32Single` class is equivalent to the C `pcg32s_random_t` type:

```csharp
// Pcg32Single takes just a seed; there is only one sequence
var pcg32Single = new Pcg32Single((ulong) random.NextInt64());

// the API is the same as Pcg32
Console.WriteLine(pcg32Single.GenerateNext());
```