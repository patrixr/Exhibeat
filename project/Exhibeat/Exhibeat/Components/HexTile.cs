using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble.Components;
using Humble.Animations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Exhibeat.Settings;
using Exhibeat.Shaders;

namespace Exhibeat.Components
{
    public class HexTile : AComponent
    {

        class VisualNote
        {
            public int total_time;
            public int time_left;
            public float current_scale;
#if ANIMATED_TILE
            public SpriteSheet sprite_sheet;
#endif
            public Color color = Color.Gray;

            public void Update(int time_elapsed_ms)
            {
                float start_scale = ExhibeatSettings.TileGrowthStartScale;

                time_left -= time_elapsed_ms;
                if (!ExhibeatSettings.TileGrowth || time_left <= 0)
                    current_scale = 1;
                else
                {
                    if (start_scale < 1)
                        current_scale =  (float)(total_time - time_left) / total_time;
                    else
                        current_scale = 1 + start_scale - start_scale * (float)(total_time - time_left) / total_time;
                }

                float tmp = ((float)current_scale / (start_scale + 1)) * 255f;
                byte col = (byte)tmp;
                color.B = (col > (byte)255 ? (byte)255 : col);
                tmp = ((float)current_scale / (start_scale + 1)) * 120f;
                col = (byte)tmp;
                color.G = col;
                tmp = (1 - (float)current_scale / (start_scale + 1)) * 200f;
                col = (byte)tmp;
                color.R = col;

            }
        }

        private int clapSongIdx;

        public float glowOpacity = 0f;
        public float glowTargetOpacity = 0f;

        public bool CenteredOrigin = false;
        private float Scale = 1;

        public float TileScale
        {
            get { return Scale; }
            set { Scale = value; }
        }

        public int NoteCount { get { return notes.Count; } }

        private ContentManager content;
        private Texture2D texture_base;

        BlurEffect      blurEffect;

        Color glowColor = Color.Orange;

#if ANIMATED_TILE
        private SpriteSheet spritesheet;
#endif
        private Texture2D texture_move;

        private Texture2D texture_press;
        private Vector2 press_origin;
        private bool pressAnimation = false;

        private List<VisualNote> notes;
        private Vector2 note_origin;

        private Vector2 position;
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }   
        }
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public HexTile(ContentManager contentmanager, float X = 0, float Y = 0) : base()
        {
            position = new Vector2(X, Y);
            content = contentmanager;
            texture_base = content.Load<Texture2D>("hexagon_base");
            texture_press = content.Load<Texture2D>("hexagon_press");
#if ANIMATED_TILE
            texture_move = content.Load<Texture2D>(ExhibeatSettings.GrowthAnimationName);
            spritesheet = content.Load<SpriteSheet>(ExhibeatSettings.GrowthAnimationName + "_sheet");
#else
            texture_move = content.Load<Texture2D>("hexagon_empty");
#endif
            note_origin = new Vector2(texture_base.Width / 2, texture_base.Height / 2);
            press_origin = new Vector2(texture_press.Width / 2, texture_press.Height / 2);
            notes = new List<VisualNote>();

            clapSongIdx = ExhibeatSettings.GetAudioManager().open(ExhibeatSettings.ResourceFolder + "taiko-normal-hitclap.wav");
            blurEffect = null;
        }

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
            
             if (glowOpacity > 0f)
                 spriteBatch.Draw(texture_press, position, null, glowColor * glowOpacity, 0, press_origin, Scale, SpriteEffects.None, 0);

             spriteBatch.Draw(texture_base, position, null, Color.White, 0, note_origin, Scale, SpriteEffects.None, 0);
          

            foreach (VisualNote note in notes)
            {
#if ANIMATED_TILE
                spriteBatch.Draw(texture_move, position, note.sprite_sheet.GetBlitArea(), note.color, 0, note_origin,
                    note.current_scale, SpriteEffects.None, 0);
#else
                spriteBatch.Draw(texture_move, position, null, note.color, 0, note_origin, note.current_scale * Scale, SpriteEffects.None, 0);
#endif
            }

          /* spriteBatch.End();
            blurEffect.applyEffect(spriteBatch);
            spriteBatch.Begin();*/
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                VisualNote note = notes[i];
                note.Update(ExhibeatSettings.TimeElapsed /*gameTime.ElapsedGameTime.Milliseconds*/);
                if (note.time_left < 0)
                {
                    notes.Remove(note);
                    ExhibeatSettings.GetAudioManager().stop(clapSongIdx);
                    ExhibeatSettings.GetAudioManager().play(clapSongIdx);
                    //Press();
                    i--;
                }
#if ANIMATED_TILE
                else
                    note.sprite_sheet.Update(gameTime);
#endif
            }

            if (pressAnimation)
            {
                if (glowTargetOpacity > 0f) // apparition
                {
                    if (glowOpacity >= 0.7f)
                        glowTargetOpacity = 0f;
                    else
                        glowOpacity += 0.05f;
                }
                else // disparition
                {
                    if (glowOpacity > 0f)
                        glowOpacity -= 0.01f;
                    else
                    {
                        glowOpacity = 0f;
                        pressAnimation = false;
                    }

                }
            }
        }

        public void Press(Color col)
        {
            pressAnimation = true;
            glowTargetOpacity = 1f;
            glowColor = col;
        }

        public void Press()
        {
            pressAnimation = true;
            glowTargetOpacity = 1f;
            glowColor = Color.Orange;
        }

        public void NewNote(int duration)
        {
            notes.Add(new VisualNote() { current_scale = 0, total_time = duration, time_left = duration
#if ANIMATED_TILE
                                        , sprite_sheet = new SpriteSheet(spritesheet)
#endif
            });
        }
    }
}
