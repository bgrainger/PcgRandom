using System;
using Xunit;

namespace Pcg.Tests
{
	public class ChiSquareTests
	{
		[Fact]
		public void TestNext()
		{
			// generate random numbers and count how many fall into each bin
			var random = new PcgRandom();
			var counts = new int[c_binCount];
			for (int i = 0; i < c_sampleCount; i++)
				counts[random.Next(counts.Length)]++;

			AssertChiSquare(c_sampleCount, counts);
		}

		private void AssertChiSquare(int sampleCount, int[] counts)
		{
			// calculate chi squared value for the distribution
			var chiSquare = 0.0;
			var expectedCount = sampleCount / (double) counts.Length;
			for (var i = 0; i < counts.Length; i++)
				chiSquare += Math.Pow(counts[i] - expectedCount, 2) / expectedCount;

			const double c_chiSquareThreshold = 135.81; // for 99% confidence with 100 degrees of freedom
			Assert.InRange(chiSquare, 0, c_chiSquareThreshold);
		}

		const int c_binCount = 100;
		const int c_sampleCount = 10_000 * c_binCount;
	}
}
