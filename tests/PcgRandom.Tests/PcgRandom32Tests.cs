using System;
using System.Linq;
using NUnit.Framework;

namespace PcgRandom.Tests
{
	[TestFixture]
	public class PcgRandom32Tests
	{
		[SetUp]
		public void SetUp()
		{
			_rng = new PcgRandom32(42);
		}

		[Test]
		public void Reseed()
		{
			Assert.AreEqual(TestValues[0] % int.MaxValue, _rng.Next());

			_rng = new PcgRandom32(42);
			Assert.AreEqual(TestValues[0] % int.MaxValue, _rng.Next());
		}

		[Test]
		public void NextSequence()
		{
			var expected = TestValues.Select(x => x % int.MaxValue).ToArray();
			var actual = Enumerable.Range(0, TestValues.Length).Select(x => _rng.Next()).ToArray();
			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void NextBytes()
		{
			var expected = TestValues.Select(x => (byte) x).ToArray();

			var buffer = new byte[6];
			_rng.NextBytes(buffer);
			CollectionAssert.AreEqual(expected, buffer);
		}

		[Test]
		public void NextBytesNull()
		{
			Assert.Throws<ArgumentNullException>(() => _rng.NextBytes(null));
		}

		[Test]
		public void NextMaxValueZero()
		{
			Assert.AreEqual(0, _rng.Next(0));
		}

		[Test]
		public void NextMaxValueOne()
		{
			Assert.AreEqual(0, _rng.Next(1));
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(100)]
		[TestCase(1000000)]
		[TestCase(int.MaxValue)]
		public void NextMaxInRange(int maxValue)
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.Next(maxValue);
				Assert.LessOrEqual(0, r);
				Assert.Less(r, maxValue);
			}
		}

		[TestCase(-100, 0)]
		[TestCase(0, 1)]
		[TestCase(0, 2)]
		[TestCase(0, 100)]
		[TestCase(0, 1000000)]
		[TestCase(-1000000, 1000000)]
		[TestCase(int.MinValue, 0)]
		[TestCase(0, int.MaxValue)]
		[TestCase(int.MinValue, int.MaxValue)]
		public void NextMinMaxInRange(int minValue, int maxValue)
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.Next(minValue, maxValue);
				Assert.LessOrEqual(minValue, r);
				Assert.Less(r, maxValue);
			}
		}

		[TestCase(int.MinValue)]
		[TestCase(-100)]
		[TestCase(0)]
		[TestCase(100)]
		[TestCase(int.MaxValue - 1)]
		public void NextMinMaxEqual(int value)
		{
			Assert.AreEqual(value, _rng.Next(value, value));
			Assert.AreEqual(value, _rng.Next(value, value + 1));
		}

		[Test]
		public void NextMaxMaxOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(-1));
		}

		[Test]
		public void NextMinMaxMaxOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(0, -1));
		}

		[Test]
		public void NextDoubleInRange()
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.NextDouble();
				Assert.LessOrEqual(0, r);
				Assert.Less(r, 1.0);
			}
		}

		// test values are first round output from https://raw.githubusercontent.com/imneme/pcg-c/master/test-high/expected/check-pcg32s.out
		static readonly uint[] TestValues = { 0xc2f57bd6u, 0x6b07c4a9u, 0x72b7b29bu, 0x44215383u, 0xf5af5eadu, 0x68beb632 };
		const int Repetitions = 1000;

		Random _rng;
	}
}