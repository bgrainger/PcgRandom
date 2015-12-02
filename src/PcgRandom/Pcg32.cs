namespace PcgRandom
{
	/// <summary>
	/// Implements the <code>pcg32</code> random number generator described
	/// at <a href="http://www.pcg-random.org/using-pcg-c.html">http://www.pcg-random.org/using-pcg-c.html</a>.
	/// </summary>
	public sealed class Pcg32
	{
		public Pcg32(ulong seed, ulong sequence)
		{
			// corresponds to pcg_setseq_64_srandom_r
			_inc = (sequence << 1) | 1u;
			Step();
			_state += seed;
			Step();
		}

		public uint Next()
		{
			// corresponds to pcg_setseq_64_xsh_rr_32_random_r
			var oldState = _state;
			Step();
			return OutputXshRr(oldState);
		}

		public uint Next(uint bound)
		{
			// corresponds to pcg_setseq_64_xsh_rr_32_boundedrand_r
			uint threshold = (uint) (-bound % bound);
			while (true)
			{
				uint r = Next();
				if (r >= threshold)
					return r % bound;
			}
		}

		public void Advance(ulong delta)
		{
			// corresponds to pcg_setseq_64_advance_r
			_state = Advance(_state, delta, Multiplier, _inc);
		}

		private ulong Advance(ulong state, ulong delta, ulong multiplier, ulong increment)
		{
			// corresponds to pcg_advance_lcg_64
			ulong acc_mult = 1u;
			ulong acc_plus = 0u;
			while (delta > 0)
			{
				if (delta % 2 == 1)
				{
					acc_mult *= multiplier;
					acc_plus = acc_plus * multiplier + increment;
				}
				increment = (multiplier + 1) * increment;
				multiplier *= multiplier;
				delta /= 2;
			}
			return acc_mult * state + acc_plus;
		}

		private void Step()
		{
			// corresponds to pcg_setseq_64_step_r
			_state = unchecked(_state * Multiplier + _inc);
		}

		private uint OutputXshRs(ulong state)
		{
			// correponds to pcg_output_xsh_rs_64_32
			return unchecked((uint) (((state >> 22) ^ state) >> (int) ((state >> 61) + 22u)));
		}

		private static uint OutputXshRr(ulong state)
		{
			// corresponds to pcg_output_xsh_rr_64_32
			return Rotate((uint) (((state >> 18) ^ state) >> 27), (int) (state >> 59));
		}

		private static uint Rotate(uint value, int rotate)
		{
			// implements pcg_rotr_32
			return (value >> rotate) | (value << (-rotate & 31));
		}

		const ulong Multiplier = 6364136223846793005ul;

		readonly ulong _inc;
		ulong _state;
	}
}