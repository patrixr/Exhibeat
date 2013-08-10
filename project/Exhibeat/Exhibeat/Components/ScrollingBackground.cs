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
    public class ScrollingBackground : AComponent
    {
        private ContentManager content;
        private Texture2D tex;
        private float scroll_percent_x = 0;
        private int width,height;
        private int scroll_speed = 2; // percent/second
        private Rectangle dest = new Rectangle();
        private Rectangle source = new Rectangle();
        private int elapsed_ms = 0;
        private float percent_visble = 0.2f;

        #region CONSTRUCTION
        public ScrollingBackground(ContentManager contentman, String tex_name = null)
            : base()
        {
            width = ExhibeatSettings.WindowWidth;
            height = ExhibeatSettings.WindowHeight;
            if (tex_name == null)
                tex = contentman.Load<Texture2D>("wp_front");
            else
                tex = contentman.Load<Texture2D>(tex_name);
            dest.Y = 0;
            dest.Width = (int)(percent_visble * (float)width);
            dest.Height = height;
            source.Y = 0;
            source.Height = tex.Height;
            source.Width = (int)(percent_visble * (float)tex.Width); ;
        }
        #endregion

        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            dest.X = (int)(scroll_percent_x * width);
            source.X = (int)(scroll_percent_x * tex.Width);
            spriteBatch.Draw(tex, dest, source, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            elapsed_ms += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed_ms < 5)
                return;
            scroll_percent_x += (float)(((float)scroll_speed / 10f * (float)elapsed_ms) / 1000f);
            if (scroll_percent_x > 1)
                scroll_percent_x = (float)(-percent_visble);
            elapsed_ms = 0;
        }

        #endregion

    }
}
