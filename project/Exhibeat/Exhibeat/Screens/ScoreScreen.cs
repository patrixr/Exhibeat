using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exhibeat.Gameplay;
using Exhibeat.Components;
using Exhibeat.Settings;
using Exhibeat.Rhythm;
using Microsoft.Xna.Framework.Input;

namespace Exhibeat.Screens
{
    class ScoreScreen : Screen
    {
        private ScoreGraph graph;
        private Texture2D background_tex;
        private Rectangle background_dest;
        private Texture2D excellentTex, greatTex, perfectTex, missTex;
        private Texture2D gradeTex;
        private Vector2 gradeDest;
        private Vector2 excellentDest, greatDest, perfectDest, missDest;
        private SpriteFont font;
        private ScoreLogger logger;
        private int timer = 0;
           
        public ScoreScreen(HumbleGame game, ScoreLogger sl)
            : base(game)
        {
            logger = sl;
            graph = new ScoreGraph(sl.GetGraphValues(), game.Content, 30, 30);
            font = game.Content.Load<SpriteFont>("scorefont");
            background_tex = game.Content.Load<Texture2D>("wp_back2");
            background_dest = new Rectangle(0, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
            excellentTex = game.Content.Load<Texture2D>("score_excellent");
            greatTex = game.Content.Load<Texture2D>("score_great");
            perfectTex = game.Content.Load<Texture2D>("score_perfect");
            missTex = game.Content.Load<Texture2D>("score_miss");

            gradeTex = game.Content.Load<Texture2D>(sl.getGradeTextureName());
            gradeDest = new Vector2(30 + graph.getWidth() / 2 - gradeTex.Width / 2, graph.getHeight() + 50);

            int spacingY = excellentTex.Height * 2;
            int stats_y = (int)(0.1f * (float)ExhibeatSettings.WindowHeight);
            int stats_x = graph.getWidth() + 250;
            excellentDest = new Vector2(stats_x, stats_y);
            perfectDest = new Vector2(stats_x, stats_y + 1*spacingY);
            greatDest = new Vector2(stats_x, stats_y + 2*spacingY);
            missDest = new Vector2(stats_x, stats_y + 3*spacingY);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= ExhibeatSettings.ScoreScreenDuration * 1000)
            {
                ScreenManager.Singleton.popScreen();
            }
            else if (timer >= 1000 &&
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenManager.Singleton.popScreen();
            }

            graph.Update(gameTime);
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
            SpriteBatch.Draw(background_tex, background_dest, Color.White);

            SpriteBatch.DrawString(font, logger.GetHitCount(userEvent.NOTEVERYGOOD) + "x", new Vector2(excellentDest.X - 100, excellentDest.Y), Color.White);
            SpriteBatch.DrawString(font, logger.GetHitCount(userEvent.NOTEGOOD) + "x", new Vector2(excellentDest.X - 100, perfectDest.Y), Color.White);
            SpriteBatch.DrawString(font, logger.GetHitCount(userEvent.NOTENORMAL) + "x", new Vector2(excellentDest.X - 100, greatDest.Y), Color.White);
            SpriteBatch.DrawString(font, logger.GetHitCount(userEvent.NOTEFAIL) + "x", new Vector2(excellentDest.X - 100, missDest.Y), Color.White);
            SpriteBatch.DrawString(font, "Accuracy : " + logger.getAccuracy().ToString("f") + " %", new Vector2(excellentDest.X - 100, missDest.Y + 200), Color.White);

            SpriteBatch.Draw(gradeTex, gradeDest, Color.White);

            SpriteBatch.Draw(excellentTex, excellentDest, Color.White);
            SpriteBatch.Draw(perfectTex, perfectDest, Color.White);
            SpriteBatch.Draw(greatTex, greatDest, Color.White);
            SpriteBatch.Draw(missTex, missDest, Color.White);

            graph.Draw(this.SpriteBatch);
            SpriteBatch.End();
            base.Draw();
        }
    }
}
