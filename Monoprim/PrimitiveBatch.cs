using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monoprim;

public sealed class PrimativeBatch :IDisposable
{
    public readonly GraphicsDevice GraphicsDevice;
    private bool _hasBegan = false;
    private BasicEffect _effect;
    private List<Drawable> _drawables = [];
    private bool _disposed = false;

    public PrimativeBatch(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
        ArgumentNullException.ThrowIfNull(graphicsDevice);

        var data = new Color[1];
        Array.Fill(data, Color.White);
        var texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(data);

        _effect = new BasicEffect(graphicsDevice);
        _effect.TextureEnabled = true;
        _effect.Texture = texture;

        _effect.Projection = Matrix.CreateOrthographic(
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height,
            0.001F,
            100.0F
        );
        _effect.View = Matrix.Identity;
        _effect.World = Matrix.Identity;
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            if (_effect != null)
                _effect.Dispose();

            _disposed = true;
        }
    }

    #nullable enable
    private void Prepare(
        RenderTarget2D ?renderTarget = null,
        BlendState ?blendState = null,
        DepthStencilState ?depthStencilState = null,
        RasterizerState ?rasterizerState = null)
    {
        GraphicsDevice.BlendState = blendState ?? BlendState.Opaque;
        GraphicsDevice.DepthStencilState = depthStencilState ?? DepthStencilState.Default;
        GraphicsDevice.RasterizerState = rasterizerState ?? RasterizerState.CullNone;
        if (renderTarget != null)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
        }
    }

    /// <summary>
    /// Renders every primitive added
    /// </summary>
    /// <param name="viewMatrix"></param>
    /// <param name="restorePreviousState"></param>
    /// <param name="renderTarget"></param>
    /// <param name="blendState"></param>
    /// <param name="depthStencilState"></param>
    /// <param name="rasterizerState"></param>
    public void Present(
        Matrix? viewMatrix = null,
        bool restorePreviousState = true,
        RenderTarget2D ?renderTarget = null,
        BlendState ?blendState = null,
        DepthStencilState ?depthStencilState = null,
        RasterizerState ?rasterizerState = null
        )
    {
        var previousRenderTarget = renderTarget;
        var previousBlendState = blendState;
        var previousDepthStencilState = depthStencilState;
        var previousRasterizerState = rasterizerState;

        Prepare(renderTarget, blendState, depthStencilState, rasterizerState);
    }
#nullable disable
}