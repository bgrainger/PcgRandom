namespace Pcg.Tests
{
    public sealed class TestRoundOutput
    {
		public TestRoundOutput(uint[] randomNumbers, string coins, int[] rolls, string cards)
		{
			RandomNumbers = randomNumbers;
			Coins = coins.Select(x => x == 'H' ? 1 : 0).ToArray();
			Rolls = rolls;
			Cards = cards;
		}

	    public uint[] RandomNumbers { get; }

	    public int[] Coins { get; }

		public int[] Rolls { get; }

		public string Cards { get; }
	}
}
