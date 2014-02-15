using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Exhibeat.Settings;

namespace Exhibeat.Screens
{
    class TitleScreen : Screen
    {
        private Texture2D background;
        private Texture2D title_logo;
        private Texture2D press_start;
        private Rectangle bg_start;
        private Rectangle bg_end;

        protected HumbleGame _game;
           public TitleScreen(HumbleGame game)
            : base(game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            bg_start = new Rectangle(0, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
            bg_end = new Rectangle(ExhibeatSettings.WindowWidth, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
            background = Content.Load<Texture2D>("Title screenbg2");
            title_logo = Content.Load<Texture2D>("Title Logo");
            press_start = Content.Load<Texture2D>("Press Start");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                ScreenManager.GetInstance().pushScreen(new SongSelectionScreen(_game));
            if (bg_start.X + background.Width <= 0)
                bg_start.X = bg_end.X + background.Width;
            if (bg_end.X + background.Width <= 0)
               bg_end.X = bg_start.X + background.Width;
            if ((gameTime.TotalGameTime.Milliseconds % 1) == 0)
            {
                bg_start.X -= 1;
                bg_end.X -= 1;
            }
            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, bg_start, Color.White);
            SpriteBatch.Draw(background, bg_end, Color.White);
            SpriteBatch.Draw(title_logo, new Vector2(ExhibeatSettings.WindowWidth / 2 - 400,
                                                     ExhibeatSettings.WindowHeight / 2 - 250),
                             null, Color.White, 0, Vector2.Zero, 0.6f,
                             SpriteEffects.None, 0);
            SpriteBatch.Draw(press_start, new Vector2(ExhibeatSettings.WindowWidth / 2 - 207,
                                                     ExhibeatSettings.WindowHeight - 100),
                             null, Color.White, 0, Vector2.Zero, 0.5f,
                             SpriteEffects.None, 0);
            SpriteBatch.End();
            base.Draw();
        }
    }
}
