﻿using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Shapes.Tests
{
	public class FilledRectTests : TestWithMocksOrVisually
	{
		[Test, ApproveFirstFrameScreenshot]
		public void RenderRect()
		{
			new FilledRect(new Rectangle(0.3f, 0.3f, 0.4f, 0.4f), Color.Blue);
		}

		[Test]
		public void RenderManyRects()
		{
			var random = Core.Randomizer.Current;
			for (int num = 0; num < 20; num++)
				new FilledRect(new Rectangle(random.Get(0.0f, 0.8f), random.Get(0.2f, 0.8f), 0.2f, 0.2f),
					Color.GetRandomColor());
		}

		[Test]
		public void ControlRectanglesWithMouseAndWhenTheyCollideTheyChangeColor()
		{
			var r1 = new FilledRect(new Rectangle(0.2f, 0.2f, 0.1f, 0.1f), Color.Red) { Rotation = 45 };
			var r2 = new FilledRect(new Rectangle(0.6f, 0.6f, 0.1f, 0.2f), Color.Red) { Rotation = 70 };
			r1.Start<CollidingChangesColor>();
			r2.Start<CollidingChangesColor>();
			new Command(point => r1.Center = point).Add(new MouseButtonTrigger(MouseButton.Left,
				State.Pressed));
			new Command(point => r2.Center = point).Add(new MouseButtonTrigger(MouseButton.Right,
				State.Pressed));
		}

		private class CollidingChangesColor : UpdateBehavior
		{
			public CollidingChangesColor()
				: base(Priority.First) {}

			//ncrunch: no coverage start
			public override void Update(IEnumerable<Entity> entities)
			{
				var copyOfEntities = new List<Entity>(entities);
				foreach (Entity2D entity1 in entities)
					foreach (Entity2D entity2 in copyOfEntities)
						if (entity1 != entity2)
							UpdateColorIfColliding(entity1, entity2);
			}

			private static void UpdateColorIfColliding(Entity2D entity1, Entity2D entity2)
			{
				entity1.Color = entity1.DrawArea.IsColliding(entity1.Rotation, entity2.DrawArea,
					entity2.Rotation) ? Color.Yellow : Color.Red;
			} //ncrunch: no coverage end
		}

		[Test, CloseAfterFirstFrame]
		public void DefaultRectIsRectangleZeroAndWhite()
		{
			var rect = new FilledRect(Rectangle.One, Color.White);
			Assert.AreEqual(Rectangle.One, rect.DrawArea);
			Assert.AreEqual(Color.White, rect.Color);
		}

		[Test, CloseAfterFirstFrame]
		public void RectangleCornersAreCorrect()
		{
			var rect = new FilledRect(Rectangle.One, Color.White);
			var corners = new List<Vector2D>
			{
				Vector2D.Zero,
				Vector2D.UnitX,
				Vector2D.One,
				Vector2D.UnitY
			};
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(corners, rect.Points);
		}

		[Test]
		public void ToggleRectVisibilityOnClick()
		{
			var rect = new FilledRect(Rectangle.FromCenter(Vector2D.Half, new Size(0.2f)), Color.Orange);
			new Command(() => ChangeVisibility(rect)).Add(new MouseButtonTrigger(MouseButton.Left,
				State.Releasing));
		}

		//ncrunch: no coverage start (only executed when triggering the command)
		private static void ChangeVisibility(FilledRect rect)
		{
			rect.IsVisible = !rect.IsVisible;
		} //ncrunch: no coverage end

		[Test, ApproveFirstFrameScreenshot]
		public void RenderRedRectOverBlue()
		{
			new FilledRect(new Rectangle(0.3f, 0.3f, 0.4f, 0.4f), Color.Red) { RenderLayer = 1 };
			new FilledRect(new Rectangle(0.4f, 0.4f, 0.4f, 0.4f), Color.Blue) { RenderLayer = 0 };
		}

		[Test]
		public void RenderGrowingRotatingRectangle()
		{
			var rect = new FilledRect(new Rectangle(0.3f, 0.3f, 0.1f, 0.1f), Color.Red);
			rect.Start<Grow>();
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (FilledRect rect in entities)
				{
					rect.DrawArea = new Rectangle(rect.TopLeft, rect.Size * (1 + Time.Delta / 10));
					rect.Rotation += 10 * Time.Delta;
				}
			}
		} //ncrunch: no coverage end

		[Test]
		public void RenderSpinningRectangleAttachedToMouse()
		{
			var rect = new FilledRect(new Rectangle(0.5f, 0.5f, 0.4f, 0.1f), Color.Blue);
			rect.Start<Spin>();
			new Command(point => rect.DrawArea = Rectangle.FromCenter(point, rect.DrawArea.Size)).Add(
				new MouseMovementTrigger());
		}

		//ncrunch: no coverage start
		private class Spin : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (FilledRect rect in entities)
					rect.Rotation += 5 * Time.Delta;
			}
		} //ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void RenderingHiddenRectangleDoesNotThrowException()
		{
			new FilledRect(Rectangle.One, Color.Red) { IsVisible = false };
			Assert.DoesNotThrow(() => AdvanceTimeAndUpdateEntities());
		}
	}
}