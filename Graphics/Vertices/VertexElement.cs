﻿using System;
using System.IO;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// An element of a rendering vertex.
	/// </summary>
	public class VertexElement : IEquatable<VertexElement>
	{
		private VertexElement() {} //ncrunch: no coverage

		public VertexElement(VertexElementType elementType)
		{
			ElementType = elementType;
			InitializeDataFromType();
		}

		public VertexElementType ElementType { get; private set; }

		private void InitializeDataFromType()
		{
			switch (ElementType)
			{
			case VertexElementType.Position3D:
			case VertexElementType.Normal:
				ComponentCount = 3;
				Size = 12;
				break;
			case VertexElementType.Position2D:
			case VertexElementType.TextureUV:
				ComponentCount = 2;
				Size = 8;
				break;
			case VertexElementType.Color:
				ComponentCount = 4;
				Size = 4;
				break;
			default:
				throw new ElementTypeNotYetSupported(ElementType);
			}
		}

		public class ElementTypeNotYetSupported : Exception
		{
			public ElementTypeNotYetSupported(VertexElementType elementType)
				: base(elementType.ToString()) {}
		}

		public int Size { get; private set; }
		public int ComponentCount { get; private set; }

		public bool Equals(VertexElement other)
		{
			return ElementType == other.ElementType && ComponentCount == other.ComponentCount &&
				Size == other.Size;
		}

		public override string ToString()
		{
			return ElementType + "*" + ComponentCount;
		}

		public void SaveData(BinaryWriter writer, Vector vector)
		{
			writer.Write(vector.X);
			writer.Write(vector.Y);
			writer.Write(vector.Z);
		}

		public void SaveData(BinaryWriter writer, Point point)
		{
			writer.Write(point.X);
			writer.Write(point.Y);
		}

		public void SaveData(BinaryWriter writer, Color color)
		{
			writer.Write(color.R);
			writer.Write(color.G);
			writer.Write(color.B);
			writer.Write(color.A);
		}

		public int Offset { get; internal set; }
	}
}