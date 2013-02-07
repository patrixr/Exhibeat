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

namespace Exhibeat.Components
{
    public class HexPad : AComponent, ITimeEventReciever
    {
        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set {
                scale = value;
                foreach (HexTile t in tiles)
                    t.Scale = value;
                UpdateTilePositions();
            }
        }

        private ContentManager  content;
        private Texture2D texture_basic;

        private List<HexTile> tiles;

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

        #region CONSTRUCTION
        public HexPad(ContentManager contentman, float x = 0, float y = 0) : base()
        {
            position = new Vector2(x, y);
            tiles = new List<HexTile>(7);
            content = contentman;
        }
        #endregion
        
        #region PRIVATE METHODS

        private void UpdateTilePositions()
        {
            for (int i = 0; i < 7; i++)
            {
                tiles[i].X = X + texture_basic.Width / 2 + ExhibeatSettings.TilePositions[i].X * texture_basic.Width * Scale;
                tiles[i].Y = Y + texture_basic.Height / 2 + ExhibeatSettings.TilePositions[i].Y * texture_basic.Height * Scale;
            }
        }

        #endregion

        #region COMPONENT OVERRIDE

        public override void Initialize()
        {
            texture_basic = content.Load<Texture2D>("hexagon_base");
            for (int i = 0; i < 7; i++)
            {
                tiles.Add(new HexTile(content,
                    X + texture_basic.Width/2 + ExhibeatSettings.TilePositions[i].X * texture_basic.Width * Scale,
                    Y + texture_basic.Height/2 + ExhibeatSettings.TilePositions[i].Y * texture_basic.Height * Scale));
            }

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
            foreach (HexTile tile in tiles)
            {
                tile.Update(gameTime);
            }
        }

        #endregion

        #region TIMERECEIVER IMPLEMENTATION
        public void NewSongEvent(songEvent ev, int param)
        {
            throw new NotImplementedException();
        }

        public void NewUserEvent(userEvent ev, int param)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
