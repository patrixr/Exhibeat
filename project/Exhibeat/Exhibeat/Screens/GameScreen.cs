using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Humble.Components;
using Humble.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exhibeat.Components;
using Exhibeat.Settings;
using Exhibeat.Gameplay;

namespace Exhibeat.Screens
{
    /// <summary>
    /// Le jeu en lui meme
    /// Ce screen sera cree au dessus du SongSelectionScreen
    /// TODO : prendre la chanson a jouer en parametre lors de la creation
    /// </summary>
    class GameScreen : Screen
    {
        private HexPad pad;
        private Visualizer visualizer;
        private LifeBar lifebar;
        //private AnimatedSprite runner;

        private MapReader mapReader;

        public GameScreen(HumbleGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            pad = new HexPad(Content, ExhibeatSettings.WindowWidth / 2, ExhibeatSettings.WindowHeight / 2);
            pad.Initialize();
            pad.CenteredOrigin = true;
            pad.Scale = 1;

            lifebar = new LifeBar(Content,30,30);

            mapReader = new MapReader();
            mapReader.Initialize(Content);
            mapReader.RegisterNewReciever(pad);

            mapReader.Read("test.exi");
            mapReader.Play();

            //visualizer = new Visualizer(Content, 500, 0, 800, 100, 100);
            visualizer = new Visualizer(Content, 0, 0, 0, ExhibeatSettings.WindowHeight / 4, 30);
            mapReader.RegisterNewReciever(visualizer);

            //runner = new AnimatedSprite(Content.Load<Texture2D>("running-test"), Content.Load<SpriteSheet>("running-test-sheet"), new Vector2(100, 100), false);
            //runner.Position = new Vector2(0, 0/* ExhibeatSettings.WindowHeight - Content.Load<Texture2D>("running-test").Height / 2*/);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            visualizer.Update(gameTime);
            pad.Update(gameTime);
            mapReader.Update(gameTime);
            lifebar.Update(gameTime);
            //runner.Update(gameTime);

            /*Vector2 p = runner.Position;
            if (p.X < 300)
                p.X += 1;
            runner.Position = p;*/

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


            visualizer.Draw(SpriteBatch);
            pad.Draw(SpriteBatch);
            lifebar.Draw(SpriteBatch);

            //runner.Draw(SpriteBatch);

            SpriteBatch.End();
            base.Draw();
        }
    }
}
