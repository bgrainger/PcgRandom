using NUnitLite;
using System;
using System.Reflection;

namespace PcgRandom.Tests
{
	public static class Program
	{
		public static int Main(string[] args)
		{
#if DNXCORE50
			return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#else
			return new AutoRun().Execute(args);
#endif
		}
	}
}