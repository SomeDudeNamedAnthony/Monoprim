using Microsoft.Xna.Framework.Graphics;

namespace Monoprim;

/// <summary>
/// A generic type for all primitives.
/// </summary>
public class Primitive
{
    private readonly GraphicsDevice _graphicsDevice;

    //Both this; and the IndexBuffer are made dynamic as they might need to move.
    protected readonly DynamicVertexBuffer _vertexBuffer;
    protected readonly DynamicIndexBuffer _indexBuffer;

    //This is really only needed for lines and ellipses.
    //Though nothing stops you from allowing users to define an effect.
    private readonly Effect _effect;

    protected Primitive(GraphicsDevice graphicsDevice, int vertexCount, int indexCount, Effect effect = null)
    {
        _graphicsDevice = graphicsDevice;

        _vertexBuffer = new(
            _graphicsDevice,
            VertexPositionColor.VertexDeclaration,
            vertexCount,
            BufferUsage.None
        );

        _indexBuffer = new(
            _graphicsDevice,
            IndexElementSize.SixteenBits,
            indexCount,
            BufferUsage.None
        );

        _effect = effect;
    }

    public GraphicsDevice GetGraphicsDevice()
    {
        return _graphicsDevice;
    }

    internal VertexBuffer GetVertexBuffer()
    {
        return _vertexBuffer;
    }

    internal IndexBuffer GetIndexBuffer()
    {
        return _indexBuffer;
    }

    public Effect GetEffect()
    {
        return _effect;
    }
}