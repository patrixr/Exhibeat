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
        
        public bool CenteredOrigin = false;
        public float Scale = 1;

        public int NoteCount { get { return notes.Count; } }

        private ContentManager content;
        private Texture2D texture_base;

#if ANIMATED_TILE
        private SpriteSheet spritesheet;
#endif
        private Texture2D texture_move;
        
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
#if ANIMATED_TILE
            texture_move = content.Load<Texture2D>(ExhibeatSettings.GrowthAnimationName);
            spritesheet = content.Load<SpriteSheet>(ExhibeatSettings.GrowthAnimationName + "_sheet");
#else
            texture_move = content.Load<Texture2D>("hexagon_empty");
#endif
            note_origin = new Vector2(texture_base.Width / 2, texture_base.Height / 2);
            notes = new List<VisualNote>();

            clapSongIdx = ExhibeatSettings.GetAudioManager().open(ExhibeatSettings.ResourceFolder + "taiko-normal-hitclap.wav");
        }

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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
                    i--;
                }
#if ANIMATED_TILE
                else
                    note.sprite_sheet.Update(gameTime);
#endif
            }
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
