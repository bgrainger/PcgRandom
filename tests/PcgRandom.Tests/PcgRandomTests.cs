using System;
using System.Linq;
using Xunit;

namespace Pcg.Tests
{
	public class PcgRandomTests
	{
		public PcgRandomTests()
		{
			_rng = new PcgRandom(42);
		}

		[Fact]
		public void Reseed()
		{
			Assert.Equal((int) (TestValues[0] % int.MaxValue), _rng.Next());

			_rng = new PcgRandom(42);
			Assert.Equal((int) (TestValues[0] % int.MaxValue), _rng.Next());
		}

		[Fact]
		public void NextSequence()
		{
			var expected = TestValues.Select(x => (int) (x % int.MaxValue)).ToArray();
			var actual = Enumerable.Range(0, TestValues.Length).Select(x => _rng.Next()).ToArray();
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void NextBytes()
		{
			var expected = TestValues.Select(x => (byte) x).ToArray();

			var buffer = new byte[6];
			_rng.NextBytes(buffer);
			Assert.Equal(expected, buffer);
		}

		[Fact]
		public void NextBytesNull()
		{
			Assert.Throws<ArgumentNullException>(() => _rng.NextBytes(null));
		}

		[Fact]
		public void NextMaxValueZero()
		{
			Assert.Equal(0, _rng.Next(0));
		}

		[Fact]
		public void NextMaxValueOne()
		{
			Assert.Equal(0, _rng.Next(1));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(100)]
		[InlineData(1000000)]
		[InlineData(int.MaxValue)]
		public void NextMaxInRange(int maxValue)
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.Next(maxValue);
				Assert.InRange(r, 0, maxValue - 1);
			}
		}

		[Theory]
		[InlineData(-100, 0)]
		[InlineData(0, 1)]
		[InlineData(0, 2)]
		[InlineData(0, 100)]
		[InlineData(0, 1000000)]
		[InlineData(-1000000, 1000000)]
		[InlineData(int.MinValue, 0)]
		[InlineData(0, int.MaxValue)]
		[InlineData(int.MinValue, int.MaxValue)]
		public void NextMinMaxInRange(int minValue, int maxValue)
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.Next(minValue, maxValue);
				Assert.InRange(r, minValue, maxValue - 1);
			}
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(-100)]
		[InlineData(0)]
		[InlineData(100)]
		[InlineData(int.MaxValue - 1)]
		public void NextMinMaxEqual(int value)
		{
			Assert.Equal(value, _rng.Next(value, value));
			Assert.Equal(value, _rng.Next(value, value + 1));
		}

		[Fact]
		public void NextMaxMaxOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(-1));
		}

		[Fact]
		public void NextMinMaxMaxOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _rng.Next(0, -1));
		}

		[Fact]
		public void NextDoubleInRange()
		{
			for (int i = 0; i < Repetitions; i++)
			{
				var r = _rng.NextDouble();
				Assert.InRange(r, 0, 1.0);
				Assert.NotEqual(1.0, r);
			}
		}

		// test values are first round output from https://raw.githubusercontent.com/imneme/pcg-c/master/test-high/expected/check-pcg32s.out
		static readonly uint[] TestValues = { 0xc2f57bd6u, 0x6b07c4a9u, 0x72b7b29bu, 0x44215383u, 0xf5af5eadu, 0x68beb632 };
		const int Repetitions = 1000;

		Random _rng;
	}
}