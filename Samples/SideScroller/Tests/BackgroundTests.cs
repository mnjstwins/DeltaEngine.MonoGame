﻿using DeltaEngine.Platforms;
using NUnit.Framework;

namespace SideScroller.Tests
{
	internal class BackgroundTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			parallaxBackground = new ParallaxBackground(4, layerImageNames, layerScrollFactors);
		}

		private ParallaxBackground parallaxBackground;

		[Test]
		public void CreateParallaxBackground()
		{
			parallaxBackground.BaseSpeed = 0.3f;
			parallaxBackground.RenderLayer = 1;
		}

		private readonly string[] layerImageNames = new[]
		{ "SkyBackground", "Mountains_Back", "Mountains_Middle", "Mountains_Front" };
		private readonly float[] layerScrollFactors = new[] { 0.4f, 0.6f, 1.0f, 1.4f };

		[Test]
		public void DefaultRenderLayerIs0()
		{
			Assert.AreEqual(0, parallaxBackground.RenderLayer);
		}

		[Test]
		public void ScrollableBackgroundIsPauseable()
		{
			Assert.IsTrue(parallaxBackground.IsPauseable);
		}
	}
}