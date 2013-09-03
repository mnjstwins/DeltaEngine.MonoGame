﻿using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;

namespace FountainApp
{
	public class Program : App
	{
		public Program()
		{
			new ParticleFountain(new Point(0.5f, 0.6f));
		}

		public static void Main()
		{
			new Program().Run();
		}
	}
}