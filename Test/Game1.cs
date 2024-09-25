using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monoprim;

namespace Test;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private PrimitiveBatch _primitiveBatch;
    private SpriteFont _font;
    private SimpleFpsCounter _fps;
    private Triangle _tri;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.SynchronizeWithVerticalRetrace = false;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnWindowResize;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new(GraphicsDevice);
        _primitiveBatch = new(GraphicsDevice);
        _font = Content.Load<SpriteFont>("NotoSansRegular");

        _fps = new();

        _tri = new(
            GraphicsDevice,
            new(0, 0, 32, 32),
            [Color.Brown, Color.Brown, Color.Brown]
        );

        _tri.Rotate(45);

        _primitiveBatch.Draw(_tri);
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _fps.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _fps.DrawFps(_spriteBatch, _font, new Vector2(20, 20), Color.White);
        _spriteBatch.End();

        _primitiveBatch.Present();
        base.Draw(gameTime);
    }

    private void OnWindowResize(object sender, EventArgs e)
    {
        _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
    }
}