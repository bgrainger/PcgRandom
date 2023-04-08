namespace Pcg.Tests;

public class Pcg32Tests
{
	[Fact]
	public void Test()
	{
		var rng = new Pcg32(42, 54);

		foreach (var round in Pcg32Rounds)
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

	// Taken from https://github.com/imneme/pcg-c/blob/master/test-high/expected/check-pcg32.out
	static readonly TestRoundOutput[] Pcg32Rounds =
	{
		new TestRoundOutput(new[] { 0xa15c02b7u, 0x7b47f409u, 0xba1d3330u, 0x83d2f293u, 0xbfa4784bu, 0xcbed606eu },
			"HHTTTHTHHHTHTTTHHHHHTTTHHHTHTHTHTTHTTTHHHHHHTTTTHHTTTTTHTTTTTTTHT",
			new[] { 3, 4, 1, 1, 2, 2, 3, 2, 4, 3, 2, 4, 3, 3, 5, 2, 3, 1, 3, 1, 5, 1, 4, 1, 5, 6, 4, 6, 6, 2, 6, 3, 3 },
			"Qd Ks 6d 3s 3d 4c 3h Td Kc 5c Jh Kd Jd As 4s 4h Ad Th Ac Jc 7s Qs 2s 7h Kh 2d 6c Ah 4d Qh 9h 6s 5s 2c 9c Ts 8d 9s 3c 8c Js 5d 2h 6h 7d 8s 9d 5h 8h Qc 7c Tc"),
		new TestRoundOutput(new[] { 0x74ab93adu, 0x1c1da000u, 0x494ff896u, 0x34462f2fu, 0xd308a3e5u, 0x0fa83babu },
			"HHHHHHHHHHTHHHTHTHTHTHTTTTHHTTTHHTHHTHTTHHTTTHHHHHHTHTTHTHTTTTTTT",
			new[] { 5, 1, 1, 3, 3, 2, 4, 5, 3, 2, 2, 6, 4, 3, 2, 4, 2, 4, 3, 2, 3, 6, 3, 2, 3, 4, 2, 4, 1, 1, 5, 4, 4 },
			"7d 2s 7h Td 8s 3c 3d Js 2d Tc 4h Qs 5c 9c Th 2c Jc Qd 9d Qc 7s 3s 5s 6h 4d Jh 4c Ac 4s 5h 5d Kc 8h 8d Jd 9s Ad 6s 6c Kd 2h 3h Kh Ts Qh 9h 6d As 7c Ks Ah 8c"),
		new TestRoundOutput(new[] { 0x39af5f9fu, 0x04196b18u, 0xc3c3eb28u, 0xc076c60cu, 0xc693e135u, 0xf8f63932 },
			"HTTHHTTTTTHTTHHHTHTTHHTTHTHHTHTHTTTTHHTTTHHTHHTTHTTHHHTHHHTHTTTHT",
			new[] { 5, 1, 5, 3, 2, 2, 4, 5, 3, 3, 1, 3, 4, 6, 3, 2, 3, 4, 2, 2, 3, 1, 5, 2, 4, 6, 6, 4, 2, 4, 3, 3, 6 },
			"Kd Jh Kc Qh 4d Qc 4h 9d 3c Kh Qs 8h 5c Jd 7d 8d 3h 7c 8s 3s 2h Ks 9c 9h 2c 8c Ad 7s 4s 2s 5h 6s 4c Ah 7h 5s Ac 3d 5d Qd As Tc 6h 9s 2d 6c 6d Td Jc Ts Th Js"),
		new TestRoundOutput(new[] { 0x55ce6851u, 0x97a7726du, 0x17e10815u, 0x58007d43u, 0x962fb148u, 0xb9bb55bd },
			"HHTHHTTTTHTHHHHHTTHHHTTTHHTHTHTHTHHTTHTHHHHHHTHHTHHTHHTTTTHHTHHTT",
			new[] { 6, 6, 3, 2, 3, 4, 2, 6, 4, 2, 6, 3, 2, 3, 5, 5, 3, 4, 4, 6, 6, 2, 6, 5, 4, 4, 6, 1, 6, 1, 3, 6, 5 },
			"Qd 8h 5d 8s 8d Ts 7h Th Qs Js 7s Kc 6h 5s 4d Ac Jd 7d 7c Td 2c 6s 5h 6d 3s Kd 9s Jh Kh As Ah 9h 3c Qh 9c 2d Tc 9d 2s 3d Ks 4h Qc Ad Jc 8c 2h 3h 4s 4c 5c 6c"),
		new TestRoundOutput(new[] { 0xfcef7cd6u, 0x1b488b5au, 0xd0daf7eau, 0x1d9a70f7u, 0x241a37cfu, 0x9a3857b7 },
			"HHHHTHHTTHTTHHHTTTHHTHTHTTTTHTTHTHTTTHHHTHTHTTHTTHTHHTHTHHHTHTHTT",
			new[] { 5, 4, 1, 2, 6, 1, 3, 1, 5, 6, 3, 6, 2, 1, 4, 4, 5, 2, 1, 5, 6, 5, 6, 4, 4, 4, 5, 2, 6, 4, 3, 5, 6 },
			"4d 9s Qc 9h As Qs 7s 4c Kd 6h 6s 2c 8c 5d 7h 5h Jc 3s 7c Jh Js Ks Tc Jd Kc Th 3h Ts Qh Ad Td 3c Ah 2d 3d 5c Ac 8s 5s 9c 2h 6c 6d Kh Qd 8d 7d 2s 8h 4h 9d 4s"),
	};
}
