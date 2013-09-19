using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exhibeat.Gameplay;
using Microsoft.Xna.Framework.Input;
using Exhibeat.Parser;
using Exhibeat.Settings;
using System.Reflection;
using Exhibeat.Components.SpiningMenu;
using Humble.Components;
using Microsoft.Xna.Framework.Content;

namespace Exhibeat.Components.SpiningMenu
{
    class GSpiningItem : AComponent
    {
        public SpiningItem _item = null;
        private GSpiningMenu _menu = null;

        public Texture2D _itemTexture { get; set; }
        public int _itemAng { get; set;}
        public int _itemTargetAng { get; set; }
        public float _itemScale { get; set; }
        public Rectange _itemSize { get; set; }
        public Vector2 _position { get; set; }
        public Rectangle _destRect;
        public SpriteFont _font { get; set; }
        private Color _color = Color.White;
        public bool _isSelected = false;

        public GSpiningItem()
        {
            _item = new SpiningItem();
        }

        public GSpiningItem(GSpiningMenu menu, SpiningItem item)
        {
            _item = item;
            _menu = menu;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _color = _isSelected ? new Color(255,255,255) : new Color(255,255,255, 0.05f);
            spriteBatch.Draw(_itemTexture, _destRect, null, _color, 0, new Vector2(_itemTexture.Width / 2, _itemTexture.Height / 2), SpriteEffects.None, 0);

            Vector2 textPos = new Vector2(_position.X - _itemTexture.Width / 6, _position.Y - _itemTexture.Height / 6);
            String title, subTitle;

            if (_item.title.Length * _font.MeasureString("B").X > (_itemTexture.Width / 2) - 80)
            {
                title = _item.title.Substring(0, ((_itemTexture.Width / 2) - 80) / (int)_font.MeasureString("B").X);
            }
            else
            {
                title = _item.title;
            }

            if (_item.subTitle.Length * _font.MeasureString("B").X > (_itemTexture.Width / 2) - 80)
            {
                subTitle = _item.subTitle.Substring(0, ((_itemTexture.Width / 2) - 80) / (int)_font.MeasureString("B").X);
            }
            else
            {
                subTitle = _item.subTitle;
            }
            spriteBatch.DrawString(_font, title, textPos, Color.Black);
            spriteBatch.DrawString(_font, subTitle, new Vector2(textPos.X, textPos.Y + _font.MeasureString("B").Y), Color.Chocolate);
            spriteBatch.End();

            if (_isSelected)
            {
                _menu.showSelectedSongInfo(_item);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_itemTargetAng != _itemAng)
            {
                _itemAng = _itemTargetAng > _itemAng ? _itemAng + 1 : _itemAng - 1;
                _position = getPositionFromAng(_itemAng);
                _destRect.X = (int)_position.X;
                _destRect.Y = (int)_position.Y;
            }
        }

        public Vector2 getPositionFromAng(int ang)
        {
            double x = _menu.getCenterMenuPosition().X + (GSpiningMenu.GITEM_DISTANCE_FROM_CENTER * Math.Cos(ang * (Math.PI / 180)));
            double y = _menu.getCenterMenuPosition().Y + (GSpiningMenu.GITEM_DISTANCE_FROM_CENTER * Math.Sin(ang * (Math.PI / 180)));

            return new Vector2((float)x, (float)y);
        }
    }
}
