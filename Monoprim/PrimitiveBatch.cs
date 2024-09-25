using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monoprim;

/// <summary>
/// A "primitive" (pun not intended) cousin to <c>SpriteBatch</c> for drawing primitives.
/// e.g. Triangles, polygons, rectangles, ect.
/// </summary>
public class PrimitiveBatch
{
    private GraphicsDevice _graphicsDevice;
    private readonly BasicEffect _basicEffect;

    private int _primitiveCount;
    private readonly List<VertexBuffer> _vertexBuffers;
    private readonly List<IndexBuffer> _indexBuffers;
    private readonly List<Effect> _effects;

    private const int VerticesPerTriangle = 3;

    public PrimitiveBatch(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _basicEffect = new(_graphicsDevice)
        {
            VertexColorEnabled = true,

            View = Matrix.CreateLookAt(
                new Vector3(0, 0, -1),
                new Vector3(0, 0, 0),
                new Vector3(0, -1, 0)
            ),

            World = Matrix.CreateTranslation(
                (float)-_graphicsDevice.Viewport.Width / 2,
                (float)-_graphicsDevice.Viewport.Height / 2,
                0.0F
            ),

            Projection = Matrix.CreateOrthographic(
                _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height,
                1.0F,
                100.0F
            )
        };

        _vertexBuffers = new();
        _indexBuffers = new();
        _effects = new();
    }

    public void Present(
        BlendState blendState = null,
        SamplerState samplerState = null,
        DepthStencilState depthStencilState = null,
        RasterizerState rasterizerState = null
    )
    {
        if (_primitiveCount == 0)
        {
            return;
        }

        #region Save GraphicsDevice state.
        var previousBlendState = _graphicsDevice.BlendState;
        var previousSamplerState = _graphicsDevice.SamplerStates[0];
        var previousDepthStencilState = _graphicsDevice.DepthStencilState;
        var previousRasterizerState = _graphicsDevice.RasterizerState;
        #endregion

        SetGraphicsDeviceState(_graphicsDevice, blendState ?? BlendState.Opaque, samplerState ?? SamplerState.PointClamp,depthStencilState ?? DepthStencilState.Default, rasterizerState ?? RasterizerState.CullCounterClockwise );

        Render(_graphicsDevice, _basicEffect, _vertexBuffers.ToArray(), _indexBuffers.ToArray(), _primitiveCount, _effects.ToArray());

        //Now we can restore the previous state the GraphicsDevice was in.
        SetGraphicsDeviceState(_graphicsDevice, previousBlendState, previousSamplerState, previousDepthStencilState, previousRasterizerState);
    }

    private static void SetGraphicsDeviceState(GraphicsDevice graphicsDevice, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
    {
        graphicsDevice.BlendState = blendState;
        graphicsDevice.SamplerStates[0] = samplerState;
        graphicsDevice.DepthStencilState = depthStencilState;
        graphicsDevice.RasterizerState = rasterizerState;
    }

    public void Draw(Primitive primitive)
    {
        _vertexBuffers.Add(primitive.GetVertexBuffer());
        _indexBuffers.Add(primitive.GetIndexBuffer());
        _effects.Add(primitive.GetEffect());
        _primitiveCount++;
    }

    private static void Render(GraphicsDevice graphicsDevice, Effect baseEffect, VertexBuffer []vertexBuffers, IndexBuffer []indexBuffers, int primitiveCount,Effect []primitiveEffects)
    {
        foreach (var baseEffectPass in baseEffect.CurrentTechnique.Passes)
        {
            baseEffectPass.Apply();
            for (var primitiveIndex = 0; primitiveIndex < primitiveCount; primitiveIndex++)
            {
                graphicsDevice.Indices = indexBuffers[primitiveIndex];
                graphicsDevice.SetVertexBuffer(vertexBuffers[primitiveIndex]);
                var currentPrimitiveCount = VerticesPerTriangle / vertexBuffers[primitiveIndex].VertexCount;
                var primitiveEffect = primitiveEffects[primitiveIndex];

                if (primitiveEffect != null)
                {
                    foreach (var primitiveEffectPass in primitiveEffect.CurrentTechnique.Passes)
                    {
                        primitiveEffectPass.Apply();
                    }
                }
                else
                {
                    graphicsDevice.DrawIndexedPrimitives
                    (
                        PrimitiveType.TriangleList,
                        0,
                        0,
                        currentPrimitiveCount
                    );
                }
            }
        }
    }


    public ref GraphicsDevice GetGraphicsDevice()
    {
        return ref _graphicsDevice;
    }
}
