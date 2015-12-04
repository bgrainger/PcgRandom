namespace PcgRandom
{
	/// <summary>
	/// Implements the <code>pcg32</code> random number generator described
	/// at <a href="http://www.pcg-random.org/using-pcg-c.html">http://www.pcg-random.org/using-pcg-c.html</a>.
	/// </summary>
	public sealed class Pcg32
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Pcg32"/> pseudorandom number generator.
		/// </summary>
		/// <param name="state">The starting state for the RNG; you can pass any 64-bit value.</param>
		/// <param name="sequence">The output sequence for the RNG; you can pass any 64-bit value, although only the low
		/// 63 bits are significant.</param>
		/// <remarks>For this generator, there are 2<sup>63</sup> possible sequences of pseudorandom numbers. Each sequence
		/// is entirely distinct and has a period of 2<sup>64</sup>. The <paramref name="sequence"/> argument selects which
		/// stream you will use. The <paramref name="state"/> argument specifies where you are in that 2<sup>64</sup> period.</remarks>
		public Pcg32(ulong state, ulong sequence)
		{
			// implements pcg_setseq_64_srandom_r
			_inc = (sequence << 1) | 1u;
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
			// implements pcg_setseq_64_xsh_rr_32_random_r
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
			// implements pcg_setseq_64_xsh_rr_32_boundedrand_r
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
			// implements pcg_setseq_64_advance_r
			_state = Helpers.Advance(_state, delta, Helpers.Multiplier64, _inc);
		}
		
		private void Step()
		{
			// corresponds to pcg_setseq_64_step_r
			_state = unchecked(_state * Helpers.Multiplier64 + _inc);
		}

		readonly ulong _inc;
		ulong _state;
	}
}