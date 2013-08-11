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
    class ScoreGraph : AComponent
    {
        private List<float> values;
        private int width;
        private int height;
        private Texture2D tex_bars, tex_dot;
        private Rectangle dest;
        private RenderTarget2D graph = null;

        public ScoreGraph(List<float> vals, ContentManager content, int x, int y)
        {
            tex_bars = content.Load<Texture2D>("graphbars");
            tex_dot = content.Load<Texture2D>("square");
            width = tex_bars.Width;
            height = tex_bars.Height;
            dest = new Rectangle(x, y, width, height);

            if (vals.Count <= width)
                values = vals;
            else
            {
                values = new List<float>();
                int spacing = vals.Count / width;
                int i = 0;
                while (values.Count < width && i < vals.Count)
                {
                    values.Add(vals[i]);
                    i += spacing;
                }
            }

        }

        private void generateGraph(SpriteBatch spriteBatch)
        {
            if (tex_bars == null || tex_dot == null || graph == null)
                return;

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(graph);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();


            Rectangle dot_dest = new Rectangle();
            dot_dest.Width = tex_dot.Width;
            Color col = new Color(0, 0, 0);
            for (int i = 0; i < values.Count; i++)
            {
                dot_dest.X = 10 + i;
                dot_dest.Y = (int)(height - 20 - (values[i] * height));
                dot_dest.Height = (int)(values[i] * height);
                col.B = (byte)(((float)dot_dest.Height / (float)height) * (float)255);
                col.R = (byte)(255 - (((float)dot_dest.Height / (float)height) * (float)255));
                dot_dest.Height = tex_dot.Height;
                spriteBatch.Draw(tex_dot, dot_dest, col);
            }

            spriteBatch.Draw(tex_bars, Vector2.Zero, Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
           
        }

        public override void Initialize()
        {
         
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (graph == null)
            {
                graph = new RenderTarget2D(spriteBatch.GraphicsDevice, width, height);
                if (graph == null)
                    return;
                generateGraph(spriteBatch);
            }
            spriteBatch.Draw((Texture2D)graph, dest, Color.White);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
