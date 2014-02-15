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

namespace Exhibeat.Components
{
    class SlidingBackground : AComponent
    {
        private Color col;
        private int ColorUpdateTick = 0;
        private Texture2D texture;
        private Rectangle dest;
        private Rectangle dest2;
        private Rectangle dest3;
        private int slide_speed = 1; //percent/second
        private int elapsed_ms = 0;
        private float scroll_percent_x = 1;

        public SlidingBackground(Texture2D tex)
        {
            if (ExhibeatSettings.SlidingBackgroundColorChange)
            {
                col = new Color(255, 1, 0);
            }
            else
                col = Color.White;
            texture = tex;
            dest = new Rectangle(0, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
            dest2 = new Rectangle(ExhibeatSettings.WindowWidth, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
            dest3 = new Rectangle(ExhibeatSettings.WindowWidth, 0, ExhibeatSettings.WindowWidth, ExhibeatSettings.WindowHeight);
        }

        private void ResetPositions()
        {
            dest.X = 0;
            scroll_percent_x = 1;
        }

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            dest.X = (int)(scroll_percent_x * ExhibeatSettings.WindowWidth);
            dest2.X = dest.X - ExhibeatSettings.WindowWidth;
            dest3.X = dest.X + ExhibeatSettings.WindowWidth;
            spriteBatch.Draw(texture, dest, col);
            spriteBatch.Draw(texture, dest2, null, col, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(texture, dest3, null, col, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (ExhibeatSettings.SlidingBackgroundColorChange)
                UpdateColor(gameTime);
            elapsed_ms += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed_ms < 5)
                return;
            scroll_percent_x -= (float)(((float)slide_speed / 10f * (float)elapsed_ms) / 1000f);
            if (scroll_percent_x <= -1)
                ResetPositions();
            elapsed_ms = 0;
        }

        public Color getCurrentColor()
        {
            return col;
        }

        private void UpdateColor(GameTime gameTime)
        {
            ColorUpdateTick += gameTime.ElapsedGameTime.Milliseconds;

            if (ColorUpdateTick >= 10)
            {
                if (col.R == 255 && col.G < 255 && col.B == 0)
                    col.G += 2;
                else if (col.G == 255 && col.R > 0 && col.B == 0)
                    col.R--;
                else if (col.G == 255 && col.B < 255 && col.R == 0)
                    col.B++;
                else if (col.B == 255 && col.G > 1 && col.R == 0)
                    col.G -= 2;
                else if (col.B == 255 && col.R < 255 && col.G == 1)
                    col.R++;
                else if (col.R == 255 && col.B > 0 && col.G == 1)
                    col.B--;
                ColorUpdateTick = 0;
            }
        }
    }
}
