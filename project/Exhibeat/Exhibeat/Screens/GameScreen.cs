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
using Microsoft.Xna.Framework.Input;

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
        private ScrollingBackground scrollingbackground;
        private ScoreLogger scoreLogger;

        // STATIC BACKGROUND
        private Texture2D background;
        private Rectangle background_dest;
        // OR SLIDING BACKGROUND
        private SlidingBackground slide_background;

        //NASTY SCORE
        private Dictionary<char, Texture2D> scoreDigits = new Dictionary<char,Texture2D>();
        private List<Rectangle> scoreDigitLocation = new List<Rectangle>();

        private MapReader mapReader;
        private MapPreview _mapPreview;
        //private AnimatedSprite runner;

        public GameScreen(HumbleGame game)
            : base(game)
        {

        }

        public GameScreen(HumbleGame game, MapPreview map)
            : base(game)
        {
            _mapPreview = map;
        }

        public override void Initialize()
        {
            pad = new HexPad(Content, ExhibeatSettings.WindowWidth / 2, ExhibeatSettings.WindowHeight / 2);
            pad.Initialize();
            pad.CenteredOrigin = true;
            pad.Scale = 1;

            grades = new NoteGradeDisplay(Content);

            scrollingbackground = new ScrollingBackground(Content);

            mapReader = new MapReader();
            mapReader.Initialize(Content);
            mapReader.RegisterNewReciever(pad);

            mapReader.Read(_mapPreview.FilePath);
            mapReader.Play();

            mapReader.RegisterNewReciever(this);

            scoreLogger = new ScoreLogger();

            visualizer = new Visualizer(Content, 0, 0, 0, ExhibeatSettings.WindowHeight / 3, 50);
            mapReader.RegisterNewReciever(visualizer);
            mapReader.RegisterNewReciever(scoreLogger);

            lifebar = new LifeBar(Content, scoreLogger, 30, 30);

            //runner = new AnimatedSprite(Content.Load<Texture2D>("running-test"), Content.Load<SpriteSheet>("running-test-sheet"), new Vector2(100, 100), false);
            //runner.Position = new Vector2(0, 0/* ExhibeatSettings.WindowHeight - Content.Load<Texture2D>("running-test").Height / 2*/);
            if (ExhibeatSettings.SlidingBackground)
                slide_background = new SlidingBackground(Content.Load<Texture2D>("wp_back"));
            else
            {
                background_dest = new Rectangle(0, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
                background = Content.Load<Texture2D>("wp_back");
            }

            //
            // SCORE
            //
            int width = ExhibeatSettings.WindowWidth - (30*8 + 150);
            for (int i = 0 ; i < 8 ; i++)
            {
                scoreDigitLocation.Add(new Rectangle(width, 30, 30, 45));
                width += 40;
            }
            
            //But baby...
            scoreDigits.Add('0', Content.Load<Texture2D>("default-0"));
            scoreDigits.Add('1', Content.Load<Texture2D>("default-1"));
            scoreDigits.Add('2', Content.Load<Texture2D>("default-2"));
            scoreDigits.Add('3', Content.Load<Texture2D>("default-3"));
            scoreDigits.Add('4', Content.Load<Texture2D>("default-4"));
            scoreDigits.Add('5', Content.Load<Texture2D>("default-5"));
            scoreDigits.Add('6', Content.Load<Texture2D>("default-6"));
            scoreDigits.Add('7', Content.Load<Texture2D>("default-7"));
            scoreDigits.Add('8', Content.Load<Texture2D>("default-8"));
            scoreDigits.Add('9', Content.Load<Texture2D>("default-9"));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mapReader.Update(gameTime);

            if (mapReader.getMapCompletion() >= 100 || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                mapReader.Stop();
                ScreenManager.Singleton.popScreen();
                ScreenManager.Singleton.pushScreen(new ScoreScreen(this.Game, scoreLogger));
            }

            if (ExhibeatSettings.SlidingBackground)
                slide_background.Update(gameTime);
            scrollingbackground.Update(gameTime);
            visualizer.Update(gameTime);
            pad.Update(gameTime);
            lifebar.Update(gameTime);
            grades.Update(gameTime);
            scoreLogger.Update(gameTime);
            Console.WriteLine(scoreLogger.getScore());
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

        private void DrawScore(bool colorChange = true)
        {
            int stats_y = (int)(0.1f * (float)ExhibeatSettings.WindowHeight);
            int stats_x = ExhibeatSettings.WindowWidth / 2 + 250;
            Vector2 scoreDest = new Vector2(stats_x, stats_y);

            //DISPLAY NASTY SCORE
           
            int c = scoreLogger.getScore().ToString().Length - 1;

            Color col;
            if (colorChange)
            {
                col = slide_background.getCurrentColor();
                col.G = 255;
                col.R = 255;
            }
            else
            {
                col = Color.White;
            }

            int i = 0;
            foreach (char digit in scoreLogger.getScore().ToString())
            {
                SpriteBatch.Draw(scoreDigits[digit], scoreDigitLocation[7 - c + i], col);
                i++;
            }
            c++;
            while (c < 8)
            {
                SpriteBatch.Draw(scoreDigits['0'], scoreDigitLocation[7 - c], col);
                c++;
            }
        }

        public override void Draw()
        {

           /* RenderTarget2D tmp_buf = new RenderTarget2D(SpriteBatch.GraphicsDevice, SpriteBatch.GraphicsDevice.Viewport.Width, SpriteBatch.GraphicsDevice.Viewport.Height);
            SpriteBatch.GraphicsDevice.SetRenderTarget(tmp_buf);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            SpriteBatch.Draw(background, background_dest, Color.White);
            SpriteBatch.End();*/

            ////SHADERS START
            if (blurEffect == null)
                 blurEffect = new BlurEffect(SpriteBatch.GraphicsDevice, Content);
            
            blurEffect.start();
            SpriteBatch.Begin();

            if (ExhibeatSettings.SlidingBackground)
                slide_background.Draw(SpriteBatch);
            else
                SpriteBatch.Draw(background, background_dest, Color.White);
            scrollingbackground.Draw(SpriteBatch);
            visualizer.Draw(SpriteBatch);

            //pad.Draw(SpriteBatch);
            lifebar.Draw(SpriteBatch);
            grades.Draw(SpriteBatch);
            DrawScore(false);

          
            // SHADERS END
            SpriteBatch.End();
            blurEffect.applyEffect(SpriteBatch);

        
            SpriteBatch.Begin();

            pad.Draw(SpriteBatch);
            lifebar.Draw(SpriteBatch);
            grades.Draw(SpriteBatch);
            DrawScore(false);

            
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
