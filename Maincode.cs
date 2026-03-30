using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RedPaint;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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

        public InputManager _input;

        public ImageManager _image;

        public SelectManager _select;

        public List<AbstrEntity> entities;

        public PanelHolder mainHolder;

        public Maincode()
        {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);
            _settings = new SettingsManager(this);
            _data = new StaticData(this);
            _updateManager = new UpdateManager(this);
            _drawManager = new DrawManager(this);
            _entityManager = new EntityManager(this);
            _input = new InputManager(this);
            _image = new ImageManager(this);
            _select = new SelectManager(this);

            entities = new List<AbstrEntity>();
        }

        protected override void Initialize()
        {
            _settings.SetFullScreen();
            _settings.SetResolution();
            _settings.ApplyChanges();

            IsMouseVisible = true;

            _entityManager.AddEntity(new BoardPanel(this));

            mainHolder = new PanelHolder(this);
            mainHolder.SetPos(new Vector2(0, 60f));
            mainHolder.size = _data.res - new Vector2(0, 60f);
            mainHolder.origin = Vector2.Zero;
            mainHolder.isShow = false;

            _entityManager.AddEntity(mainHolder);

            _entityManager.AddEntity(new VersionShow(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            _input.Update();

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
