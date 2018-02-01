using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Reflection;
using TileTools;

namespace MonoGameTest
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private Tilemap _map;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			string contentFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Content.RootDirectory);

			_map = new Tilemap(new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Point(64, 64));
			_map.LoadMap(Path.Combine(contentFolder, "test.tmx"),
				new Tilemap.TilesetRender[] {
					new Tilemap.TilesetRender(Content.Load<Texture2D>("set01"), 4, 4),
					new Tilemap.TilesetRender(Content.Load<Texture2D>("set02"), 4, 4),
				}
			);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			float dt = gameTime.DeltaTime();
			float speed = 100.0f;

			KeyboardState keyboard = Keyboard.GetState();
			if (keyboard.IsKeyDown(Keys.Left))
			{
				_map.ViewOffset.X += speed * dt;
			}
			if (keyboard.IsKeyDown(Keys.Right))
			{
				_map.ViewOffset.X -= speed * dt;
			}
			if (keyboard.IsKeyDown(Keys.Up))
			{
				_map.ViewOffset.Y += speed * dt;
			}
			if (keyboard.IsKeyDown(Keys.Down))
			{
				_map.ViewOffset.Y -= speed * dt;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			_map.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
