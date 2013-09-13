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
    public class VisualizerBar : AComponent
    {
        private Texture2D square;
        private Color col;
        private Color displayCol;
        private Rectangle dest;
        private int Bottom = 0;
        private int MaxHeight = 0;
        private int Width = 0;
        private int Height = 0;
        private int AimedHeight = 30;
        private int X = 0;
        private int ElapsedTime = 0;
        private int ColorUpdateTick = 0;
        private int moveSpeed = 500; // px/sec

        #region CONSTRUCTION
        public VisualizerBar(ContentManager contentman, int bottom, int x, int width, int maxheight)
            : base()
        {
            square = contentman.Load<Texture2D>("square");
            col = new Color(0, 255, 0, 0.5f);
            displayCol = new Color(0, 255, 0, 0.5f);
            Bottom = bottom;
            MaxHeight = maxheight;
            Height = 30;
            Width = width;
            X = x;
            dest = new Rectangle(x, bottom - Height, width, Height);
        }
        #endregion
        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
        }

        float rot = 0.0f;

        public override void Draw(SpriteBatch spriteBatch)
        {

            rot += 0.0f;

            int tmp = (int)(((float)Height / (float)MaxHeight) * (float)(col.R));
            displayCol.R = (byte)tmp;
            tmp = (int)(((float)Height / (float)MaxHeight) * (float)(col.G));
            displayCol.G = (byte)tmp;
            tmp = (int)(((float)Height / (float)MaxHeight) * (float)(col.B));
            displayCol.B = (byte)tmp;
            
            
            //col.R = (byte)tmp;


            spriteBatch.Draw(square, dest, null, displayCol, rot, Vector2.Zero, SpriteEffects.None, 0);
        }

        //private int test = 0;

        public override void Update(GameTime gameTime)
        {
            UpdateColor(gameTime);
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            //test++;
            if (AimedHeight != Height)
            {
                //if (test % 2 == 0)
                  //  return;
                if (ElapsedTime < 5)
                    return;
                if (AimedHeight > Height)
                {
                    Height += (moveSpeed * ElapsedTime) / 1000;
                    if (Height > AimedHeight)
                        Height = AimedHeight;
                    ElapsedTime = 0;
                }
                else
                {
                    Height -= (moveSpeed * ElapsedTime) / 1000 / 2;
                    if (Height < 0)
                        Height = 0;
                    ElapsedTime = 0;
                }

                dest.Y = Bottom - Height;
                dest.Height =  Height;
                if (AimedHeight == Height)
                    ElapsedTime = 0;
            }
            else
            {
                if (ElapsedTime >= 100)
                {
                    SetHeight(0);
                    ElapsedTime = 0;
                }
            }
        }

        #endregion

        private void UpdateColor(GameTime gameTime)
        {
            ColorUpdateTick += gameTime.ElapsedGameTime.Milliseconds;

            if (ColorUpdateTick >= 10)
            {
                if (col.R == 255 && col.G < 255 && col.B == 0)
                    col.G++;
                else if (col.G == 255 && col.R > 0 && col.B == 0)
                    col.R--;
                else if (col.G == 255 && col.B < 255 && col.R == 0)
                    col.B++;
                else if (col.B == 255 && col.G > 0 && col.R == 0)
                    col.G--;
                else if (col.B == 255 && col.R < 255 && col.G == 0)
                    col.R++;
                else if (col.R == 255 && col.B > 0 && col.G == 0)
                    col.B--;
                ColorUpdateTick = 0;
            }
        }

        public void SetHeight(int height)
        {
            if (height < 0)
                height = 0;
            else if (height > MaxHeight)
                height = MaxHeight;
            AimedHeight = height;
        }
    }
}
