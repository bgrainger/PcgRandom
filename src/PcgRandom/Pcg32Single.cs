namespace PcgRandom
{
	/// <summary>
	/// Implements the <code>pcg32s</code> random number generator described
	/// at <a href="http://www.pcg-random.org/using-pcg-c.html">http://www.pcg-random.org/using-pcg-c.html</a>.
	/// </summary>
	public sealed class Pcg32Single
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Pcg32Single"/> pseudorandom number generator.
		/// </summary>
		/// <param name="state">The starting state for the RNG; you can pass any 64-bit value.</param>
		public Pcg32Single(ulong state)
		{
			// implements pcg_oneseq_64_srandom_r
			Step();
			_state += state;
			Step();
		}

		/// <summary>
		/// Generates the next random number.
		/// </summary>
		/// <returns></returns>
		public uint GenerateNext()
		{
			// implements pcg_oneseq_64_xsh_rr_32_random_r
			var oldState = _state;
			Step();
			return Helpers.OutputXshRr(oldState);
		}

		/// <summary>
		/// Generates a uniformly distributed 32-bit unsigned integer less than <paramref name="bound"/> (i.e., <c>x</c> where
		/// <c>0 &lt;= x &lt; bound</c>.
		/// </summary>
		/// <param name="bound">The exclusive upper bound of the random number to be generated.</param>
		/// <returns>A random number between <c>0</c> and <paramref name="bound"/> (exclusive).</returns>
		public uint GenerateNext(uint bound)
		{
			// implements pcg_oneseq_64_xsh_rr_32_boundedrand_r
			uint threshold = ((uint) -bound) % bound;
			while (true)
			{
				uint r = GenerateNext();
				if (r >= threshold)
					return r % bound;
			}
		}

		/// <summary>
		/// Advances the RNG by <paramref name="delta"/> steps, doing so in <c>log(delta)</c> time.
		/// </summary>
		/// <param name="delta">The number of steps to advance; pass <c>2<sup>64</sup> - delta</c> (i.e., <c>-delta</c>) to go backwards.</param>
		public void Advance(ulong delta)
		{
			// implements pcg_oneseq_64_advance_r
			_state = Helpers.Advance(_state, delta, Helpers.Multiplier64, Helpers.Increment64);
		}

		private void Step()
		{
			// implements pcg_oneseq_64_step_r
			_state = unchecked(_state * Helpers.Multiplier64 + Helpers.Increment64);
		}

		ulong _state;
	}
}