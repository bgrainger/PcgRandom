using System;

namespace Pcg
{
	/// <summary>
	/// Provides an implementation of <see cref="System.Random"/> that uses <see cref="Pcg32Single"/> to generate its random numbers.
	/// </summary>
	public sealed class PcgRandom : Random
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PcgRandom"/> class, using a time-dependent default seed value.
		/// </summary>
		public PcgRandom()
			: this(Environment.TickCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PcgRandom"/> class, using the specified seed value.
		/// </summary>
		/// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence.</param>
		public PcgRandom(int seed)
		{
			_rng = new Pcg32Single(unchecked((uint) seed));
		}

		/// <summary>
		/// Returns a non-negative random integer.
		/// </summary>
		/// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="int.MaxValue"/>.</returns>
		public override int Next()
		{
			return (int) _rng.GenerateNext(int.MaxValue);
		}

		/// <summary>
		/// Returns a non-negative random integer that is less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
		/// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
		/// includes 0 but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
		public override int Next(int maxValue)
		{
			if (maxValue < 0)
				throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue, "maxValue must be non-negative");
			if (maxValue <= 1)
				return 0;

			return (int) _rng.GenerateNext((uint) maxValue);
		}

		/// <summary>
		/// Returns a random integer that is within a specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
		/// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>;
		/// that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/>
		/// equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
				throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue, $"maxValue must be greater than minValue ({minValue})");

			var range = (uint) ((long) maxValue - minValue);
			return (int) (minValue + (range <= 1 ? 0 : _rng.GenerateNext(range)));
		}

		/// <summary>
		/// Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		/// <param name="buffer">An array of bytes to contain random numbers.</param>
		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			for (var i = 0; i < buffer.Length; i++)
				buffer[i] = (byte) _rng.GenerateNext(256);
		}

		/// <summary>
		/// Returns a random floating-point number between 0.0 and 1.0.
		/// </summary>
		/// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
		protected override double Sample()
		{
			// as per http://www.pcg-random.org/using-pcg-c.html#generating-doubles, return (rnd * 1/2**32)
			return _rng.GenerateNext() * Math.Pow(2, -32);
		}

		readonly Pcg32Single _rng;
	}
}
