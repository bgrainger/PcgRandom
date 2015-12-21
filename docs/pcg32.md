# Pcg32

The `Pcg32` class implements the high-level C API exposed by the `pcg32_random_t` RNG described
in the [C library documentation](http://www.pcg-random.org/using-pcg-c.html).

Internally, this RNG uses two 64-bit integers for its internal state, consisting of:

* *the current state* — the RNG iterates through all 2<sup>64</sup> possible internal states.
* *the RNG-sequence constant* — a value that defines which of 2<sup>63</sup> possible random
   sequences the current state is iterating through; it holds the same value over the lifetime of the RNG.

Different values for the sequence constant cause the generator to produce a different (and unique) sequence
of random numbers (sometimes called the *stream*). In other words, it's as if you have 2<sup>63</sup>
different RNGs available, and this constant lets you choose which one you're using.

You can create as many separate RNGs as you like. If you give them different sequence constants, they
will be independent and uncorrelated with each other (i.e., their sequences will not overlap at all).

The API centers around these functions:

```
Pcg32(ulong state, ulong sequence)
uint GenerateNext()
uint GenerateNext(uint bound)
void Advance(ulong delta)
```

## `Pcg32(ulong state, ulong sequence)`

The constructor initializes (a.k.a. “seeds”) the random number generator. The arguments are defined
as follows:

* `state` is the starting state for the RNG; you can pass any 64-bit value.
* `sequence` selects the output sequence for the RNG; you can pass any 64-bit value, although
   only the low 63 bits are significant.
   
For this generator, there are 2<sup>63</sup> possible sequences of pseudorandom numbers. Each
sequence is entirely distinct and has a period of 2<sup>64</sup>. The `sequence` argument selects
*which* stream you will use. The `state` argument specifies where you are in that 2<sup>64</sup> period.

Constructing a `Pcg32` object with the same arguments produces the same output, allowing programs
to use random number sequences repeatably.

If you want truly nondeterministic output for each run of your program, you should pass values that
will be different from run to run. In .NET, `System.Security.Cryptography.RandomNumberGenerator`
provides random bytes that can be used for initialization, but if you want a quick and dirty way to
do the initialization, one option is to pass the current time (e.g., `Environment.TickCount`).

(This corresponds to `pcg32_srandom_r` in the C API.)

## `GenerateNext()`

Generates a pseudorandom uniformly distributed 32-bit unsigned integer (i.e., *x* where, *0* <= *x* < *2<sup>32</sup>*).

(This corresponds to `pcg32_random_r` in the C API.)

## `GenerateNext(uint bound)`

Generates a uniformly distributed 32-bit unsigned integer less than bound (i.e., *x* where *0* <= *x* < *bound*).

Some programmers may think that they can just run `rng.GenerateNext() % bound`, but doing so
introduces nonuniformity when `bound` is not a power of two. The code for `GenerateNext(uint bound)`
avoids the nonuniformity by dropping a portion of the RNG's output.

(This corresponds to `pcg32_boundedrand_r` in the C API.)

## `Advance(ulong delta)`

This operation provides jump-ahead; it advances the RNG by `delta` steps, doing so in log(`delta`)
time. Because of the periodic nature of generation, advancing by 2<sup>64</sup> - *d* (i.e.,
passing *-d*) is equivalent to backstepping the generator by *d* steps.

Note that a call to `GenerateNext()` counts as one step, whereas calls to `GenerateNext(bound)`
can require multiple steps because it can discard some outputs.

(This corresponds to `pcg32_advance_r` in the C API.)

## Generating doubles

This library does not provide a direct facility to generate floating point random numbers. It turns
out that generating random floating point values is [surprisingly challenging](http://mumble.net/~campbell/tmp/random_real.c).
If you are happy to have a floating point value in the range `[0,1)` that has been rounded down
to the nearest multiple of 1/2<sup>32</sup>, you can use

```
double d = rng.GenerateNext() * Math.Pow(2, -32);
```
