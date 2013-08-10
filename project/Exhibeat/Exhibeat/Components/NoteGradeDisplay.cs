using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble.Components;
using Humble.Components.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Exhibeat.Settings;
using Exhibeat.Rhythm;
using Exhibeat.Gameplay;

namespace Exhibeat.Components
{
    public class NoteGradeDisplay : AComponent
    {

        class GradeSprite
        {
            public Texture2D tex;
            public bool appear = false;
            public bool isGrowing = false;
            public int timeout_ms = 0;
            public int elapsed_ms = 0;
            public Rectangle dest;
            public int x_dest;
            private ParticleEmitter particle_emitter;
            public int moveSpeed = 300; //px/s

            public GradeSprite(ContentManager content, int y, Texture2D texture, bool particles = false)
            {
                tex = texture;
                x_dest = ExhibeatSettings.WindowWidth - tex.Width;
                dest = new Rectangle(ExhibeatSettings.WindowWidth,
                    y, tex.Width, tex.Height);

                if (particles)
                {
                    RandomParticleGenerator pg = new RandomParticleGenerator(true, 0.1f, 5f, 1000);
                 
                    pg.Textures.Add(content.Load<Texture2D>("square"));
                    
                   
                    particle_emitter = new ParticleEmitter(pg, new Vector2(ExhibeatSettings.WindowWidth - texture.Width, y));
                    
                    particle_emitter.GenerationCount = 40;
                }
                else
                    particle_emitter = null;
            }

            public void Update(GameTime gameTime)
            {
                if (particle_emitter != null)
                    particle_emitter.Stop();
                if (appear)
                {
                    if (isGrowing)
                    {
                        if (dest.X > x_dest) // not fully grown
                        {
                            //dest.X -= 2 * gameTime.ElapsedGameTime.Milliseconds;
                            dest.X = x_dest;
                        }
                        else
                        {
                            dest.X = x_dest;
                            if (timeout_ms == 0)
                            {
                                if (particle_emitter != null && gameTime.IsRunningSlowly == false)
                                    particle_emitter.Start();
                            }
                            if (timeout_ms > 300)
                            {
                                timeout_ms = 0;
                                isGrowing = false;
                            }
                            else
                            {
                                timeout_ms += gameTime.ElapsedGameTime.Milliseconds;
                            }
                        }
                    }
                    else
                    {
                        elapsed_ms += gameTime.ElapsedGameTime.Milliseconds;
                        if (elapsed_ms > 10)
                        {
                            if (dest.X < ExhibeatSettings.WindowWidth)
                                dest.X += (int)((moveSpeed * elapsed_ms) / 1000);
                            else
                            {
                                dest.X = ExhibeatSettings.WindowWidth;
                                appear = false;
                            }
                            elapsed_ms = 0;
                        }
                    }
                }
              if (particle_emitter != null)
                 particle_emitter.Update(gameTime);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                if (particle_emitter != null)
                    particle_emitter.Draw(spriteBatch);
                spriteBatch.Draw(tex, dest, Color.White);
            }

            public void Appear()
            {
                appear = true;
                timeout_ms = 0;
                isGrowing = true;
                
            }
        }

        private List<GradeSprite> grades;

        public NoteGradeDisplay(ContentManager content)
            : base()
        {

            int y;

            y = ExhibeatSettings.WindowHeight / 4;

            grades = new List<GradeSprite>();
            grades.Add(new GradeSprite(content, y, content.Load<Texture2D>("score_excellent"), true));
            y += content.Load<Texture2D>("score_excellent").Height;
            grades.Add(new GradeSprite(content, y, content.Load<Texture2D>("score_perfect"), true));
            y += content.Load<Texture2D>("score_perfect").Height;
            grades.Add(new GradeSprite(content, y, content.Load<Texture2D>("score_great")));
            y += content.Load<Texture2D>("score_great").Height;
            grades.Add(new GradeSprite(content, y, content.Load<Texture2D>("score_miss")));
            y += content.Load<Texture2D>("score_miss").Height;
        }

        public void DisplayVeryGood()
        {
            grades[0].Appear();
        }

        public void DisplayGood()
        {
            grades[1].Appear();
        }

        public void DisplayNormal()
        {
            grades[2].Appear();
        }

        public void DisplayBad()
        {
        }

        public void DisplayFail()
        {
            grades[3].Appear();
        }

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (GradeSprite gr in grades)
            {
                gr.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GradeSprite gr in grades)
            {
                gr.Update(gameTime);
            }
        }
    }
}
