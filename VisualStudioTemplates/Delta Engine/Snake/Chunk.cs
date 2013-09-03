using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering;
using DeltaEngine.Rendering.Shapes;

namespace $safeprojectname$
{
	public class Chunk : FilledRect
	{
		public Chunk(int gridSize, float blockSize, Color color) : base(new Rectangle(new 
			Point(blockSize * (int)(gridSize / 2.0f), blockSize * (int)(gridSize / 2.0f)), new 
				Size(blockSize)), color)
		{
			this.gridSize = gridSize;
			this.blockSize = blockSize;
			random = new PseudoRandom();
		}

		private readonly int gridSize;
		private readonly float blockSize;
		private readonly PseudoRandom random;

		public void SpawnAtRandomLocation()
		{
			int x = random.Get(2, gridSize - 2);
			int y = random.Get(2, gridSize - 2);
			var newRandomPos = new Point(x * blockSize, y * blockSize);
			var newDrawArea = new Rectangle(newRandomPos, new Size(blockSize));
			DrawArea = newDrawArea;
		}

		public bool IsCollidingWithSnake(List<FilledRect> snakeBodies)
		{
			return snakeBodies.Any(IsBodyColliding);
		}

		private bool IsBodyColliding(Entity2D body)
		{
			var otherRect = DrawArea.Reduce(new Size(0.01f));
			return body.DrawArea.IsColliding(body.Rotation, otherRect, 0.0f);
		}
	}
}