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
using Exhibeat.Screens;

namespace Exhibeat.Components.SpiningMenu
{
    class GSpiningMenu : AComponent
    {
        public static int MAX_GITEM_ON_DISPLAY = 8;
        public static float GITEM_DISTANCE_FROM_CENTER = 300;

        private List<GSpiningItem> _gSpiningItemList = new List<GSpiningItem>();
        private List<SpiningItem> _spiningItemList = new List<SpiningItem>();

        private int _currentItemIdx = 0;

        private event Action UpPressed = null;
        private event Action DownPressed = null;
        private event Action SpacePressed = null;
        private event Action EnterPressed = null;

        private Keys[] keys = { Keys.Up, Keys.Down, Keys.Space, Keys.Enter };
        
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        private Texture2D _selectedItemTexture = null;
        private Texture2D _itemTexture = null;
        private Texture2D _centerMenuTexture = null; //Color(143, 205, 242)

        private SpriteFont _exhibeatFont = null;

        private SpriteBatch _spriteBatch = null;
        private ContentManager _contentManager;
        private SongSelectionScreen _parentScreen;


        private Vector2 _position = new Vector2(0, 0);
        private float _scale = 1f;
        bool toUpdate = true;
        bool toInitialize = true;

        public GSpiningMenu(SongSelectionScreen parentScreen, SpriteBatch spriteBatch, ContentManager content)
        {
            this._parentScreen = parentScreen;
            this._spriteBatch = spriteBatch;
            this._contentManager = content;

            _selectedItemTexture = _contentManager.Load<Texture2D>("SongSelectionScreen\\SelectedSong");
            _itemTexture = _contentManager.Load<Texture2D>("SongSelectionScreen\\Song");
            _centerMenuTexture = _contentManager.Load<Texture2D>("SongSelectionScreen\\HexSong");
            _exhibeatFont = _contentManager.Load<SpriteFont>("exhibeatFont");

            for (int i = 0; i < MAX_GITEM_ON_DISPLAY; i++)
            {
                SpiningItem item = new SpiningItem();
                _gSpiningItemList.Add(new GSpiningItem(this, item));
            }
            this.initializeKeyEvent();
        }

        public override void Initialize()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.drawCenter(spriteBatch);
            this.drawItems(spriteBatch);
        }
        public void drawCenter(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(_centerMenuTexture, new Vector2(_position.X, _position.Y), null, Color.White, 0f, new Vector2(_centerMenuTexture.Width / 2, _centerMenuTexture.Height / 2), this._scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
        public void drawItems(SpriteBatch spriteBatch)
        {
            foreach (GSpiningItem gItem in _gSpiningItemList)
            {
                gItem.Draw(spriteBatch);
            } 
        }

        public override void Update(GameTime gameTime)
        {
            this.processEvent();

            if (toInitialize)
                this.initializeGItemList();
            if (toUpdate)
                this.updateGItemList();

           

            foreach (GSpiningItem gItem in _gSpiningItemList)
            {
                gItem.Update(gameTime);
            } 
        }
        public void initializeGItemList()
        {
            int ang = 360 / (GSpiningMenu.MAX_GITEM_ON_DISPLAY + 0);
            int currentAng = 0;
            for (int i = 0; i < _gSpiningItemList.Count(); i++)
            {

                double x = this._position.X + (GSpiningMenu.GITEM_DISTANCE_FROM_CENTER * Math.Cos(currentAng * (Math.PI / 180)));
                double y = this._position.Y + (GSpiningMenu.GITEM_DISTANCE_FROM_CENTER * Math.Sin(currentAng * (Math.PI / 180)));
                Vector2 position = new Vector2((float)x, (float)y);

                _gSpiningItemList[i]._itemScale = _scale;
                _gSpiningItemList[i]._position = position;
                _gSpiningItemList[i]._itemAng = currentAng;
                _gSpiningItemList[i]._itemTargetAng = currentAng;
                _gSpiningItemList[i]._itemTexture = _itemTexture;
                _gSpiningItemList[i]._font = _exhibeatFont;
                _gSpiningItemList[i]._destRect = new Rectangle((int)x, (int)y, ExhibeatSettings.WindowWidth / 5, ExhibeatSettings.WindowHeight / 10);
                currentAng += ang;
            }

            _currentItemIdx = 4;
            _gSpiningItemList[_currentItemIdx]._isSelected = true;
            toInitialize = false;
        }
        public void updateGItemList()
        {
            for (int i = 0; i < _gSpiningItemList.Count(); i++)
            {
                if (i < _spiningItemList.Count())
                {
                    _gSpiningItemList[i]._item = _spiningItemList[i];
                }
            }

            //toUpdate = false;
        }

        public void addSpiningItem(SpiningItem toAdd)
        {
            // TODO CHECK IF EXIST
            _spiningItemList.Add(toAdd);
        }
        public void removeSpiningItem(SpiningItem toRemove)
        {
            // TODO CHECK IF EXIST
            _spiningItemList.Remove(toRemove);
        }
        #region SETTER / GETTER
        public void setCenterMenuPosition(Vector2 position)
        {
            this._position = position;
        }
        public Vector2 getCenterMenuPosition()
        {
            return this._position;
        }
        public void setMenuScale(float scale)
        {
            this._scale = scale;
        }
        public float getMenuScale()
        {
            return this._scale;
        }
        #endregion
        #region KEY EVENT
        public void processEvent()
        {
            _currentKeyboardState = Keyboard.GetState();
            foreach (Keys key in keys)
                if (_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key))
                    RaiseEventByName(key.ToString() + "Pressed");
            _previousKeyboardState = _currentKeyboardState;
        }
        private void initializeKeyEvent()
        {
            _currentItemIdx = 0;
            UpPressed += new Action(handleEventKeyUp);
            DownPressed += new Action(handleEventKeyDown);
            SpacePressed += new Action(handleEventKeySpace);
            EnterPressed += new Action(handleEventKeyEnter);
            _currentKeyboardState = _previousKeyboardState = Keyboard.GetState();
        }
        private void RaiseEventByName(string eventName)
        {
            var eventDelegate = (MulticastDelegate)GetType().GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
             if (eventDelegate != null)
                foreach (var handler in eventDelegate.GetInvocationList())
                    handler.Method.Invoke(handler.Target, null);
        }

        private void handleEventKeyUp()
        {
            toUpdate = true;
            _gSpiningItemList[_currentItemIdx]._isSelected = false;
            _currentItemIdx = (_currentItemIdx - 1) < 0 ? _gSpiningItemList.Count - 1 : _currentItemIdx - 1;
            _gSpiningItemList[_currentItemIdx]._isSelected = true;
            for (int i = 0; i < _gSpiningItemList.Count(); i++)
            {
                _gSpiningItemList[i]._itemTargetAng += 360 / (GSpiningMenu.MAX_GITEM_ON_DISPLAY);
            }
        }
        private void handleEventKeyDown()
        {
            toUpdate = true;
            _gSpiningItemList[_currentItemIdx]._isSelected = false;
            _currentItemIdx = (_currentItemIdx + 1) >= _gSpiningItemList.Count ? 0 : _currentItemIdx + 1;
            _gSpiningItemList[_currentItemIdx]._isSelected = true;
            for (int i = 0; i < _gSpiningItemList.Count(); i++)
            {
                _gSpiningItemList[i]._itemTargetAng -= 360 / (GSpiningMenu.MAX_GITEM_ON_DISPLAY);
            }
        }
        private void handleEventKeySpace()
        {
           _parentScreen.startGame(_currentItemIdx);
        }
        private void handleEventKeyEnter()
        {
        }
        #endregion
    }
}
