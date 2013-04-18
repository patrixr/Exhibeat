using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Exhibeat.Settings;
using Exhibeat.Rhythm;
using Exhibeat.Gameplay;
using Exhibeat.Shaders;

namespace Exhibeat.Components
{
    class Visualizer : AComponent, ITimeEventReciever
    {

        List<VisualizerBar> bars = new List<VisualizerBar>();
        private int Bottom;
        private int Height;
        private int Width;
        private int BarCount;
        private int BarWidth;
        private int Left;

        ContentManager content;
        //BlurEffect blurEffect;

        #region CONSTRUCTION
        public Visualizer(ContentManager contentman, int bottom = 0, int left = 0, int width = 0, int height = 0, int barCount = 10)
            : base()
        {
            content = contentman;

            if (bottom == 0)
                bottom = ExhibeatSettings.WindowHeight;
            if (width == 0)
                width = ExhibeatSettings.WindowWidth;
            if (height == 0)
                height = ExhibeatSettings.WindowHeight / 2;
            if (barCount < 0)
                barCount = 0;
            else if (barCount > ExhibeatSettings.WindowWidth)
                barCount = ExhibeatSettings.WindowWidth;

            Bottom = bottom;
            Height = height;
            Width = width;
            BarCount = barCount;
            BarWidth = width / barCount + 1;
            Left = left;

            for (int i = 0; i <= barCount; i++)
            {
                bars.Add(new VisualizerBar(contentman, bottom, left + i * BarWidth, BarWidth, height)); 
            }
        }
        #endregion
        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /*if (blurEffect == null)
               blurEffect = new BlurEffect(spriteBatch.GraphicsDevice, content);
           spriteBatch.End();
           blurEffect.start();
           spriteBatch.Begin();*/

            foreach (VisualizerBar bar in bars)
            {
                bar.Draw(spriteBatch);
            }

            /*spriteBatch.End();
            blurEffect.applyEffect(spriteBatch);
            spriteBatch.Begin();*/
        }

        public override void Update(GameTime gameTime)
        {
            foreach (VisualizerBar bar in bars)
            {
                bar.Update(gameTime);
            }
            
        }

        #endregion

        public void NewSongEvent(songEvent ev, object param)
        {
            Random r = new Random();
            if (ev == songEvent.NEWNOTE)
            {
                NoteEventParameter p = (NoteEventParameter)param;
                int i = 0;
                foreach (VisualizerBar bar in bars)
                {
                    bar.SetHeight(r.Next(Height));
                    i++;
                }
            }
        }

        public void NewUserEvent(userEvent ev, object param)
        {
           // throw new NotImplementedException();
        }
    }
}
