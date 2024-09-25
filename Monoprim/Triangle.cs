using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monoprim;

public class Triangle : Primitive
{
    private const int VertexCount = 3;

    private static readonly short[] Indices = { 0, 1, 2 };

    private VertexPositionColor []_vertices;

    private Rectangle _bounds;
    private int _angle;

    public Triangle(GraphicsDevice graphicsDevice, Rectangle rectangle, Color []colors) : base(graphicsDevice, 3, 3)
    {
        _bounds = rectangle;
        _vertices = new VertexPositionColor[VertexCount];
        UpdateVertices(in _vertices, rectangle, colors);
        UpdateVertexBuffer(in _vertexBuffer, _vertices);
        _indexBuffer.SetData(Indices);
    }


    public void Transform(Rectangle rect, int degrees)
    {
        _bounds = rect;
        _angle = degrees;

        _bounds.Center.ToVector2().Rotate(_angle);

        UpdateVerticesPositions
        (
            in _vertices,
            _bounds
        );
    }

    public void Translate(Vector2 offset)
    {
        _bounds.X += (int)offset.X;
        _bounds.Y += (int)offset.Y;
        Transform(_bounds, _angle);
    }

    public void Resize(Vector2 size)
    {
        _bounds.Width = (int)size.X;
        _bounds.Height = (int)size.Y;
        Transform(_bounds, _angle);
    }

    public void Rotate(int degrees)
    {
        _bounds.Location.ToVector2().Rotate(_angle);
    }

    private static void UpdateVerticesPositions(in VertexPositionColor[] vertices, Rectangle rect)
    {
        UpdateVertices(in vertices, rect, [vertices[0].Color, vertices[1].Color, vertices[2].Color]);
    }

    private static void UpdateVerticesColors(in VertexPositionColor[] vertices, Color[] colors, int vertexCount)
    {

    }

    private static void UpdateVertices(in VertexPositionColor[] vertices, Rectangle rect, Color []colors)
    {
        vertices[0].Position.X = rect.Left;
        vertices[0].Position.Y = rect.Top;
        vertices[0].Color = colors[0];
        vertices[1].Position.X = rect.Right;
        vertices[1].Position.Y = rect.Top;
        vertices[1].Color = colors[1];
        vertices[2].Position.X = rect.Left;
        vertices[2].Position.Y = rect.Bottom;
        vertices[2].Color = colors[2];
    }

    private static void UpdateVertexBuffer(in DynamicVertexBuffer vertexBuffer, VertexPositionColor[] vertices)
    {
        vertexBuffer.SetData(vertices);
    }
}