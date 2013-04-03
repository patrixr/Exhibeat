using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
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

            mapReader = new MapReader();
            mapReader.Initialize(Content);
            mapReader.RegisterNewReciever(pad);

            mapReader.Read("test.exi");
            mapReader.Play();

            //visualizer = new Visualizer(Content, 500, 0, 300, 100, 100);
            visualizer = new Visualizer(Content, 0, 0, 0, ExhibeatSettings.WindowHeight / 4, 30);
            mapReader.RegisterNewReciever(visualizer);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            visualizer.Update(gameTime);
            pad.Update(gameTime);
            mapReader.Update(gameTime);

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
          
            
            SpriteBatch.End();
            base.Draw();
        }
    }
}
