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
    class LifeBar : AComponent, ITimeEventReciever
    {
        private ContentManager content;

        private Vector2 position;

        private Texture2D texture_base;
        private Texture2D texture_bar;

        private Rectangle bar_dest;

        private float current_completion;
        private float     completion;
        public float      Completion
        {
            get { return completion; }
            set
            {
                if (value > 1.0f)
                    completion = 1.0f;
                else if (value < 0f)
                    completion = 0f;
                else
                    completion = value;
            }
        }

        #region CONSTRUCTION
        public LifeBar(ContentManager contentman, int x = 0, int y = 0)
            : base()
        {
            content = contentman;

            completion = 0.5f;
            current_completion = 0f;

            position = new Vector2(x, y);

            texture_base = content.Load<Texture2D>("lifebar_base");
            texture_bar = content.Load<Texture2D>("lifebar_bar");
            
            bar_dest = new Rectangle(0, 0, 0, 0);
            bar_dest.X = (int)position.X + (int)(0.01f * texture_base.Width);
            bar_dest.Y = (int)position.Y + (int)(0.01f * texture_base.Height);
            bar_dest.Height = (int)(0.99f * texture_base.Height);
            bar_dest.Width = (int)(current_completion * texture_base.Width);           
        }
        #endregion
        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture_base, position, Color.White);
            spriteBatch.Draw(texture_bar, bar_dest, Color.Turquoise);
        }

        public override void Update(GameTime gameTime)
        {
            if (completion > current_completion)
            {
                current_completion += 0.01f;
                if (current_completion > completion)
                    current_completion = completion;
            }
            else if (completion < current_completion)
            {
                current_completion -= 0.01f;
                if (current_completion < completion)
                    current_completion = completion;
            }

            bar_dest.Width = (int)(current_completion * 0.99f * texture_base.Width);
        }

        #endregion

        public void NewSongEvent(songEvent ev, object param)
        {
            
        }

        public void NewUserEvent(userEvent ev, object param)
        {
            if (ev == userEvent.NOTEVERYGOOD)
                Completion = Completion + 0.05f;
        }
    }
}
