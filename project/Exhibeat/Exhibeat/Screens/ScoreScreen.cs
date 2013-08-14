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

namespace Exhibeat.Screens
{
    class ScoreScreen : Screen
    {
        private ScoreGraph graph;

        public ScoreScreen(HumbleGame game, ScoreLogger sl)
            : base(game)
        {
            graph = new ScoreGraph(sl.GetGraphValues(), game.Content, 30, 30);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
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
            graph.Draw(this.SpriteBatch);
            SpriteBatch.End();
            base.Draw();
        }
    }
}
