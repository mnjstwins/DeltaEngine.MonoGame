﻿using System.Collections.Generic;
using System.Globalization;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class SliderTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			slider = new Slider(Center);
			slider.Add(new FontText(FontXml.Default, "", new Rectangle(0.5f, 0.7f, 0.2f, 0.1f)));
			slider.Start<DisplaySliderValue>();
			InitializeMouse();
		}

		private Slider slider;
		private static readonly Rectangle Center = Rectangle.FromCenter(0.5f, 0.5f, 0.5f, 0.1f);

		private class DisplaySliderValue : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Slider slider in entities)
					slider.Get<FontText>().Text = slider.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		private void InitializeMouse()
		{
			mouse = Resolve<Mouse>() as MockMouse;
			if (mouse == null)
				return; //ncrunch: no coverage

			mouse.SetPosition(Point.Zero);
			AdvanceTimeAndUpdateEntities();
		}

		private MockMouse mouse;

		[Test, ApproveFirstFrameScreenshot]
		public void RenderSliderZeroToOneHundred() {}

		[Test, CloseAfterFirstFrame]
		public void DefaultProperties()
		{
			Assert.IsTrue(slider.IsEnabled);
			Assert.AreEqual(Color.White, slider.Color);
			Assert.AreEqual(Color.White, slider.Pointer.Color);
			Assert.AreEqual(0, slider.MinValue);
			Assert.AreEqual(100, slider.Value);
			Assert.AreEqual(100, slider.MaxValue);
		}

		[Test, CloseAfterFirstFrame]
		public void DisablingSliderDisablesPointer()
		{
			slider.IsEnabled = false;
			Assert.IsFalse(slider.IsEnabled);
			Assert.IsFalse(slider.Pointer.IsEnabled);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderDisabledSlider()
		{
			slider.IsEnabled = false;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(Color.Grey, slider.Color);
			Assert.AreEqual(Color.Grey, slider.Pointer.Color);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderSliderMinusFiveToFive()
		{
			slider.MinValue = -5;
			slider.Value = 0;
			slider.MaxValue = 5;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderGrowingSlider()
		{
			slider.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Slider slider in entities)
				{
					var center = slider.DrawArea.Center + new Point(0.01f, 0.01f) * Time.Delta;
					var size = slider.DrawArea.Size * (1.0f + Time.Delta / 10);
					slider.DrawArea = Rectangle.FromCenter(center, size);
				}
			}
		}

		//ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void UpdateValues()
		{
			slider.MinValue = 1;
			slider.Value = 2;
			slider.MaxValue = 3;
			Assert.AreEqual(1, slider.MinValue);
			Assert.AreEqual(2, slider.Value);
			Assert.AreEqual(3, slider.MaxValue);
		}

		[Test, CloseAfterFirstFrame]
		public void ValidatePointerSize()
		{
			var pointer = new Material(Shader.Position2DColorUv, "DefaultSlider");
			var width = pointer.MaterialRenderSize.AspectRatio * 0.1f;
			var pointerSize = new Size(width, 0.1f);
			Assert.AreEqual(pointerSize, slider.Pointer.DrawArea.Size);
		}

		[Test, CloseAfterFirstFrame]
		public void ValidatePointerCenter()
		{
			var position = new Point(0.42f, 0.52f);
			DragMouse(position);
			Assert.AreEqual(new Point(0.42f, 0.5f), slider.Pointer.DrawArea.Center);
		}

		private void DragMouse(Point position)
		{
			SetMouseState(State.Pressing, position + new Point(0.1f, 0.1f));
			SetMouseState(State.Pressing, position);
		}

		private void SetMouseState(State state, Point position)
		{
			if (mouse == null)
				return; //ncrunch: no coverage

			mouse.SetPosition(position);
			mouse.SetButtonState(MouseButton.Left, state);
			AdvanceTimeAndUpdateEntities();
		}

		[Test]
		public void RenderVerticalSlider()
		{
			slider.Rotation = 90;
		}

		[Test]
		public void Render45DegreeSlider()
		{
			slider.Rotation = 45;
		}

		[Test]
		public void RenderSpinningSlider()
		{
			slider.Start<Spin>();
		}

		//ncrunch: no coverage start
		private class Spin : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Slider slider in entities)
					slider.Rotation += 20 * Time.Delta;
			}
		}

		//ncrunch: no coverage end

		[Test]
		public void RenderSpinningSliderAttachedToMouse()
		{
			slider.Start<Spin>();
			new Command(point => slider.DrawArea = Rectangle.FromCenter(point, slider.DrawArea.Size)).
				Add(new MouseMovementTrigger());
		}

		[Test, CloseAfterFirstFrame]
		public void VerifyValueChangedEvent()
		{
			int sliderValue = -1;
			slider.ValueChanged += value => sliderValue = value;
			var position = new Point(0.42f, 0.52f);
			DragMouse(position);
			Assert.AreEqual(slider.Value, sliderValue);
		}
	}
}