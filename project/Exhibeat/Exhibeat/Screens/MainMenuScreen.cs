using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exhibeat.Screens
{
    class MainMenuScreen : Screen
    {

#if DEBUG
        private SpriteFont plainfont;
#endif


        public MainMenuScreen(HumbleGame game) : base(game)
        {
        }

        public override void Initialize()
        {
#if DEBUG
            plainfont = Content.Load<SpriteFont>("plain");
#endif

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
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
#if DEBUG
            SpriteBatch.DrawString(plainfont, "Menu Screen", new Vector2(0, 20), Color.White);
#endif
            SpriteBatch.End();

            base.Draw();
        }
    }
}
