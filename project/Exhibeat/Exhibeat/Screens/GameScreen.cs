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
using Exhibeat.Shaders;
using Exhibeat.Rhythm;

namespace Exhibeat.Screens
{
    /// <summary>
    /// Le jeu en lui meme
    /// Ce screen sera cree au dessus du SongSelectionScreen
    /// TODO : prendre la chanson a jouer en parametre lors de la creation
    /// </summary>
    class GameScreen : Screen, ITimeEventReciever
    {
        private HexPad pad;
        private Visualizer visualizer;
        private LifeBar lifebar;
        private NoteGradeDisplay grades;
        private BlurEffect blurEffect = null;

        private Texture2D background;
        private Rectangle background_dest;
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

            grades = new NoteGradeDisplay(Content);
            lifebar = new LifeBar(Content,30,30);

            mapReader = new MapReader();
            mapReader.Initialize(Content);
            mapReader.RegisterNewReciever(pad);

            mapReader.Read("test.exi");
            mapReader.Play();

            mapReader.RegisterNewReciever(this);

            visualizer = new Visualizer(Content, 0, 0, 0, ExhibeatSettings.WindowHeight / 4, 30);
            mapReader.RegisterNewReciever(visualizer);

            //runner = new AnimatedSprite(Content.Load<Texture2D>("running-test"), Content.Load<SpriteSheet>("running-test-sheet"), new Vector2(100, 100), false);
            //runner.Position = new Vector2(0, 0/* ExhibeatSettings.WindowHeight - Content.Load<Texture2D>("running-test").Height / 2*/);

            background = Content.Load<Texture2D>("blueWP");
            background_dest = new Rectangle(0, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);

            base.Initialize();
        }

        int osef = 0;
        public override void Update(GameTime gameTime)
        {
            visualizer.Update(gameTime);
            pad.Update(gameTime);
            mapReader.Update(gameTime);
            lifebar.Update(gameTime);
            grades.Update(gameTime);
            //runner.Update(gameTime);

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

            //SHADERS START
            if (blurEffect == null)
                blurEffect = new BlurEffect(SpriteBatch.GraphicsDevice, Content);
            SpriteBatch.End();
            blurEffect.start();
            SpriteBatch.Begin();

            SpriteBatch.Draw(background, background_dest, Color.Navy);
            visualizer.Draw(SpriteBatch);
            //pad.Draw(SpriteBatch);
            //lifebar.Draw(SpriteBatch);

            // SHADERS END
            SpriteBatch.End();
            blurEffect.applyEffect(SpriteBatch);
            SpriteBatch.Begin();

            pad.Draw(SpriteBatch);
            lifebar.Draw(SpriteBatch);
            grades.Draw(SpriteBatch);

            //runner.Draw(SpriteBatch);

            SpriteBatch.End();
            base.Draw();
        }

        public void NewSongEvent(songEvent ev, object param)
        {
           
        }

        public void NewUserEvent(userEvent ev, object param)
        {
            switch (ev)
            {
                case userEvent.NOTEFAIL:
                    grades.DisplayFail();
                    break;
                case userEvent.NOTEBAD:
                    grades.DisplayBad();
                    break;
                case userEvent.NOTEGOOD:
                    grades.DisplayGood();
                    break;
                case userEvent.NOTENORMAL:
                    grades.DisplayNormal();
                    break;
                case userEvent.NOTEVERYGOOD:
                    grades.DisplayVeryGood();
                    break;
            }
        }
    }
}
