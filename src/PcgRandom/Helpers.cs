namespace PcgRandom
{
	internal static class Helpers
	{
		/// <summary>
		/// Implements <c>pcg_advance_lcg_64</c>.
		/// </summary>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/src/pcg-advance-64.c#L46">original source</a>.</remarks>
		public static ulong Advance(ulong state, ulong delta, ulong multiplier, ulong increment)
		{
			// corresponds to pcg_advance_lcg_64
			ulong accumulatedMultipler = 1u;
			ulong accumulatedIncrement = 0u;
			while (delta > 0)
			{
				if (delta % 2 == 1)
				{
					accumulatedMultipler *= multiplier;
					accumulatedIncrement = accumulatedIncrement * multiplier + increment;
				}
				increment = (multiplier + 1) * increment;
				multiplier *= multiplier;
				delta /= 2;
			}
			return accumulatedMultipler * state + accumulatedIncrement;
		}

		/// <summary>
		/// Implements <c>pcg_output_xsh_rs_64_32</c>.
		/// </summary>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/include/pcg_variants.h#L133">original source</a>.</remarks>
		public static uint OutputXshRs(ulong state)
		{
			return unchecked((uint) (((state >> 22) ^ state) >> (int) ((state >> 61) + 22u)));
		}

		/// <summary>
		/// Implements <c>pcg_output_xsh_rr_64_32</c>.
		/// </summary>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/include/pcg_variants.h#L158">original source</a>.</remarks>
		public static uint OutputXshRr(ulong state)
		{
			return RotateRight((uint) (((state >> 18) ^ state) >> 27), (int) (state >> 59));
		}

		/// <summary>
		/// Implements <c>pcg_rotr_32</c>.
		/// </summary>
		/// <param name="value">The value to rotate right.</param>
		/// <param name="rotate">The number of bits to rotate.</param>
		/// <returns>The input <paramref name="value"/>, rotated right by <paramref name="rotate"/> bits.</returns>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/include/pcg_variants.h#L88">original source</a>.</remarks>
		public static uint RotateRight(uint value, int rotate)
		{
			return (value >> rotate) | (value << (-rotate & 31));
		}

		/// <summary>
		/// Represents <c>PCG_DEFAULT_MULTIPLIER_64</c>.
		/// </summary>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/include/pcg_variants.h#L253">original source</a>.</remarks>
		public const ulong Multiplier64 = 6364136223846793005ul;

		/// <summary>
		/// Represents <c>PCG_DEFAULT_INCREMENT_64</c>.
		/// </summary>
		/// <remarks>See <a href="https://github.com/imneme/pcg-c/blob/e2383c4bfcc862b40c3d85a43c9d495ff61186cb/include/pcg_variants.h#L258">original source</a>.</remarks>
		public const ulong Increment64 = 1442695040888963407ul;
	}
}