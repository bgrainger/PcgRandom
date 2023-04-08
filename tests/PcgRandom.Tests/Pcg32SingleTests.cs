namespace Pcg.Tests
{
	public class Pcg32SingleTests
	{
		[Fact]
		public void Test()
		{
			var rng = new Pcg32Single(42);

			foreach (var round in Pcg32SingleRounds)
			{
				foreach (var expected in round.RandomNumbers)
					Assert.Equal(expected, rng.GenerateNext());

				rng.Advance((ulong) -round.RandomNumbers.Length);

				foreach (var expected in round.RandomNumbers)
					Assert.Equal(expected, rng.GenerateNext());

				foreach (var coin in round.Coins)
					Assert.Equal(coin, (int) rng.GenerateNext(2));

				foreach (var roll in round.Rolls)
					Assert.Equal(roll, (int) rng.GenerateNext(6) + 1);

				var cards = Enumerable.Range(0, 52).ToArray();
				for (var i = cards.Length; i > 1; i--)
				{
					var chosen = rng.GenerateNext((uint) i);
					var card = cards[chosen];
					cards[chosen] = cards[i - 1];
					cards[i - 1] = card;
				}

				string actual = string.Join(" ", cards.Select(x => $"{"A23456789TJQK"[x / 4]}{"hcds"[x % 4]}"));
				Assert.Equal(round.Cards, actual);
			}
		}

		// Taken from https://github.com/imneme/pcg-c/blob/master/test-high/expected/check-pcg32s.out
		static readonly TestRoundOutput[] Pcg32SingleRounds =
		{
			new TestRoundOutput(new[] { 0xc2f57bd6u, 0x6b07c4a9u, 0x72b7b29bu, 0x44215383u, 0xf5af5eadu, 0x68beb632u },
				"THTHHHTTHHTTHTTHTHHHTHTTTHTTHTTHTTTHHTTTTTHHTTTHTTHTHHTHHHTTHTTTH",
				new[] { 4, 1, 3, 3, 6, 6, 5, 1, 3, 4, 4, 3, 2, 2, 5, 4, 1, 3, 3, 3, 1, 4, 6, 4, 6, 6, 1, 6, 1, 2, 3, 6, 6 },
				"2d 5c 3h 6d Js 9c 4h Ts Qs 5d Ks 5h Ad Ac Qh Th Jd Kc Tc 7s Ah Kd 7h 3c 4d 8s 2c 3d Kh 8h Jc 6h 4c 8d Qc 7c Td 2s 3s 4s 7d Qd Jh As 6c 8c 5s 2h 6s 9d 9s 9h"),
			new TestRoundOutput(new[] { 0x0573afccu, 0x2cab16dbu, 0x6af6f55au, 0xe916bec2u, 0x1ca9b4a4u, 0xbb2778ebu },
				"THHHTHTTTHHHTTTTTTHTTHTHTHHHTHHHTHTHTTHTTTTTHTHHTHHTTHHHHHTTTHTTH",
				new[] { 1, 5, 3, 3, 5, 1, 5, 6, 5, 6, 6, 3, 5, 5, 6, 6, 2, 6, 4, 1, 5, 6, 3, 6, 5, 5, 1, 3, 2, 4, 5, 1, 1 },
				"9c Ad 5d 7d Ah 8c Th Kd 5c Js 7c Kc Kh 6c Ks Tc Td 3d 7h 2d 5s 9s 3h As 9d 8h 4s 6h Ts 2c Jh 3c 8s 4h 5h 6s Jd 8d 3s 6d 7s 4d Ac Qc 4c 2h Qh 9h Qd 2s Qs Jc"),
			new TestRoundOutput(new[] { 0x114306f3u, 0xb9bf0d91u, 0x1aed8e5eu, 0x587de8b7u, 0x7477c8bdu, 0xd853ec9du },
				"HTHHTHHHHTHTHTTHTHTHHTHTTHHHTTTTHHTTTTTTHTHTTTHTHTTTHTHHHHTTTTTTT",
				new[] { 1, 5, 4, 2, 1, 4, 6, 3, 2, 1, 6, 3, 6, 4, 3, 1, 4, 4, 2, 5, 5, 3, 3, 2, 6, 1, 6, 3, 2, 6, 5, 6, 3 },
				"Ah 8d Ad Jd 2d 3h Jh 7c Kc Ks 3d As 4s 3s 8h Qc 7d Td 6c 8c 4d 5c 9d Qh Js Ac Kd 5s 6d Ts 9h 9s 9c 2c 5h 3c 5d Th 4c 6s 7s Qd 7h 2h Tc 6h 4h 8s Qs Jc Kh 2s"),
			new TestRoundOutput(new[] { 0xb982cd46u, 0x01cc6f94u, 0x0ad658aeu, 0xf6c6c97eu, 0xd1b772ddu, 0x0098599eu },
				"HTTHTTHHHHTHTHHHTTHTHTHTTTHTHTHHTHTHTTTTHHTTHHHTHTTHHTTTHHHTTHHHH",
				new[] { 4, 4, 5, 4, 2, 1, 4, 2, 2, 5, 2, 5, 6, 6, 2, 1, 6, 6, 2, 6, 6, 3, 6, 2, 1, 4, 1, 1, 1, 1, 5, 1, 5 },
				"6s Td 3h Js 7h Jh Ac Kh Th 4h 3c 6d Qs Ah 8h Kc Tc 2h 8c 2c Jd 2s Qh 4d 3d Ks 7s 9d 5d 2d 5s 5h Jc 3s 9s Qd Qc 7d 6h As 8s 4s 4c 8d 9c 6c 5c Ad 7c 9h Kd Ts"),
			new TestRoundOutput(new[] { 0xef3c7322u, 0xa1ff2188u, 0x3f564b42u, 0x91c90425u, 0x17711b95u, 0xf43aa1f7u },
				"HTTHHHTTHTTTHTHHTHTHTHHTHHTTTHTTHTHHTHTTTTTHTHTTHHHHTHTHTHHTHHTHT",
				new[] { 4, 1, 6, 3, 3, 2, 5, 6, 3, 2, 6, 5, 3, 1, 5, 5, 4, 6, 4, 4, 2, 5, 5, 4, 1, 5, 2, 4, 5, 5, 5, 3, 5 },
				"6c 8d 4d Jc 9d As 9s 3c 9c Th Ks Qs 4c Js Ah Qc Ac Kd Td Qd Kh Kc Tc Jd 6s 5h 8c 8s Ad 5s 4s Ts 3h 3s 7h 7d 8h 2c 2d 5c 6h 2h 3d 7c 9h 7s 4h 2s Jh 6d Qh 5d"),
		};
	}
}
