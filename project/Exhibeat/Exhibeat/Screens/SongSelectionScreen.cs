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
using Exhibeat_SongDirectory_Scan;

namespace Exhibeat.Screens
{
    /// <summary>
    /// Ce screen propose au joueur les chansons
    /// TODO : prevoir un screen pour le tps de chargement des chansons
    /// Ce screen push les GameScreens pour lancer les parties
    /// </summary>
    class SongSelectionScreen : Screen
    {
        private HumbleGame _game = null;
        private GSpiningMenu _menu = null;
        private SpriteBatch _spriteBatch = null;
        private Texture2D _background = null;
        private Texture2D _title = null;
        public List<MapPreview> _songList = null;

        public SongSelectionScreen(HumbleGame game)
            : base(game)
        {
            _game = game;
        }

        public SpiningItem getItemFromPreview(MapPreview map)
        {
            SpiningItem item = new SpiningItem();

            item.title = map.Title;
            item.subTitle = map.Artist;
            item.text = map.Difficulty.ToString();

            _songList.Add(map);
            return item;
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _menu = new GSpiningMenu(this, _spriteBatch, _game.Content);
            _menu.setCenterMenuPosition(new Vector2(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height / 2));
            _menu.setMenuScale(0.5f);

            _background = _game.Content.Load<Texture2D>("SongSelectionScreen\\background");
            _title = _game.Content.Load<Texture2D>("SongSelectionScreen\\Title");

            _songList = new List<MapPreview>();
            // ADD DANS GET ITEM FROM PREVIEW
            List<string> tmpSongPath = new ExhibeatDirectoryScan().loadPathXML(ExhibeatSettings.ResourceFolder);
            foreach (string path in tmpSongPath)
            {
                _menu.addSpiningItem(getItemFromPreview(EXParser.getSongInfo(path)));
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _menu.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                ScreenManager.GetInstance().popScreen();
            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput()
        {
            _menu.processEvent();
            _menu.processEventGamePad();
            base.HandleInput();
        }

        public override void Draw()
        {
            _game.GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(_title, new Rectangle(0, 0, (int)(ExhibeatSettings.WindowWidth * 0.70), ExhibeatSettings.WindowHeight / 4), Color.White);
            _spriteBatch.End();

            _menu.Draw(_spriteBatch);
            base.Draw();
        }

        internal void startGame(int _currentItemIdx)
        {
            ScreenManager.GetInstance().pushScreen(new GameScreen(Game, _songList[_currentItemIdx]));
        }
    }
}
