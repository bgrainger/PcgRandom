namespace Pcg.Tests;

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

		AssertChiSquare(c_sampleCount, counts, c_chiSquareThreshold100);
	}

	[Fact]
	public void TestNextInt64()
	{
		// generate random numbers and count how many fall into each bin
		var random = new PcgRandom();
		var counts = new int[c_binCount];
		for (int i = 0; i < c_sampleCount; i++)
			counts[random.NextInt64(counts.Length)]++;

		AssertChiSquare(c_sampleCount, counts, c_chiSquareThreshold100);
	}

	[Fact]
	public void TestNexBytesArray()
	{
		var random = new PcgRandom();
		var counts = new int[256];

		var bytes = new byte[1000];
		for (int i = 0; i < counts.Length; i++)
		{
			random.NextBytes(bytes);
			foreach (var by in bytes)
				counts[by]++;
		}

		AssertChiSquare(counts.Length * bytes.Length, counts, c_chiSquareThreshold256);
	}

	[Fact]
	public void TestNexBytesSpan()
	{
		var random = new PcgRandom();
		var counts = new int[256];

		Span<byte> bytes = stackalloc byte[1000];
		for (int i = 0; i < counts.Length; i++)
		{
			random.NextBytes(bytes);
			foreach (var by in bytes)
				counts[by]++;
		}

		AssertChiSquare(counts.Length * bytes.Length, counts, c_chiSquareThreshold256);
	}

	private void AssertChiSquare(int sampleCount, int[] counts, double threshold)
	{
		// calculate chi squared value for the distribution
		var chiSquare = 0.0;
		var expectedCount = sampleCount / (double) counts.Length;
		for (var i = 0; i < counts.Length; i++)
			chiSquare += Math.Pow(counts[i] - expectedCount, 2) / expectedCount;

		Assert.InRange(chiSquare, 0, threshold);
	}

	const int c_binCount = 100;
	const int c_sampleCount = 10_000 * c_binCount;
	const double c_chiSquareThreshold100 = 135.807; // for 99% confidence with 100 degrees of freedom; https://www.wolframalpha.com/input?i=chi-squared+distribution%2C+v+%3D+100%2C+99th+percentile
	const double c_chiSquareThreshold256 = 311.56; // for 99% confidence with 256 degrees of freedom; https://www.wolframalpha.com/input?i=chi-squared+distribution%2C+v+%3D+256%2C+99th+percentile
}
