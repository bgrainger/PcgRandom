using NUnit.Framework;

namespace PcgRandom.Tests
{
	[TestFixture]
	public class Pcg32Tests
	{
		[Test]
		public void Test()
		{
			Pcg32 pcg32 = new Pcg32(42, 54);

			Assert.AreEqual(0xa15c02b7u, pcg32.Next());
		}
	}
}