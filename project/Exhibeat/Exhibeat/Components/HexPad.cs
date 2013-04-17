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
    public class HexPad : AComponent, ITimeEventReciever
    {
        private ContentManager  content;
        private Texture2D texture_basic;

        private List<HexTile> tiles;

        #region PROPERTIES

        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                foreach (HexTile t in tiles)
                    t.TileScale = value;
                UpdateTilePositions();
            }
        }

        private bool centered_origin = true;
        public bool CenteredOrigin
        {
            get { return centered_origin; }
            set { centered_origin = value; UpdateTilePositions(); }
        }
        
        private Vector2 real_position;
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
                UpdateTilePositions();
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
                UpdateTilePositions();
            }
        }

        public int Height
        {
            get
            {
                return (int)(ExhibeatSettings.TilePositions[6].Y * texture_basic.Height * Scale + texture_basic.Height * Scale);
            }
        }

        public int Width
        {
            get
            {
                return (int)(3f * texture_basic.Width * Scale);
            }
        }
        #endregion

        #region CONSTRUCTION
        public HexPad(ContentManager contentman, float x = 0, float y = 0) : base()
        { 
            position = new Vector2(x, y);
            real_position = new Vector2(x, y);
            tiles = new List<HexTile>(7);
            content = contentman;
            texture_basic = contentman.Load<Texture2D>("hexagon_base");
            for (int i = 0; i < 7; i++)
            {
                tiles.Add(new HexTile(contentman));
            }
            UpdateTilePositions();
        }
        #endregion
        
        #region PRIVATE METHODS

        private void UpdateTilePositions()
        {
            if (CenteredOrigin)
            {
                real_position.X = position.X - (Width / 2);
                real_position.Y = position.Y - (Height / 2);
                for (int i = 0; i < 7; i++)
                {
                    tiles[i].X = real_position.X + (texture_basic.Width * Scale / 2) + ExhibeatSettings.TilePositions[i].X * texture_basic.Width * Scale;
                    tiles[i].Y = real_position.Y + (texture_basic.Height * Scale / 2) + ExhibeatSettings.TilePositions[i].Y * texture_basic.Height * Scale;
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    tiles[i].X = position.X + (texture_basic.Width * Scale / 2) + ExhibeatSettings.TilePositions[i].X * texture_basic.Width * Scale;
                    tiles[i].Y = position.Y + (texture_basic.Height * Scale / 2) + ExhibeatSettings.TilePositions[i].Y * texture_basic.Height * Scale;
                }
            }
        }

        #endregion

        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach ( HexTile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            int note_count = 0;


            foreach (HexTile tile in tiles)
            {
                tile.Update(gameTime);
                note_count += tile.NoteCount;
            }
        }

        #endregion

        #region TIMERECEIVER IMPLEMENTATION
        public void NewSongEvent(songEvent ev, Object param)
        {
            if (ev == songEvent.NEWNOTE)
            {
                NoteEventParameter p = (NoteEventParameter)param;
                tiles[p.note].NewNote(p.delayms);
            }
        }
        public void NewUserEvent(userEvent ev, Object param)
        {
            if (ev == userEvent.NOTEPRESSED)
            {
                NoteEventParameter p = (NoteEventParameter)param;
                tiles[p.note].Press();
            }
            else if (ev == userEvent.NOTERELEASED)
            {
                //NoteEventParameter p = (NoteEventParameter)param;
                //tiles[p.note].NoteReleased();
            }
            else
                throw new NotImplementedException();
        }
        #endregion
    }
}
