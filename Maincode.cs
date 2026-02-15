using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RedPaint;
using System.Collections.Generic;

namespace RedPaint
{
    public class Maincode : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public StaticData _data;
        public SettingsManager _settings;

        public UpdateManager _updateManager;
        public DrawManager _drawManager;

        public EntityManager _entityManager;

        public List<AbstrEntity> entities;

        public Maincode()
        {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);
            _settings = new SettingsManager(this);
            _data = new StaticData(this);
            _updateManager = new UpdateManager(this);
            _drawManager = new DrawManager(this);
            _entityManager = new EntityManager(this);

            entities = new List<AbstrEntity>();
        }

        protected override void Initialize()
        {
            _settings.SetFullScreen();
            _settings.SetResolution();
            _settings.ApplyChanges();

            IsMouseVisible = true;

            _entityManager.AddEntity(new BoardPanel(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(0).Buttons.Back == ButtonState.Pressed)
                Exit();

            _entityManager.Apply();

            _updateManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_settings.GetCurrPalletre().baseColor1);

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null
            );

            _drawManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
