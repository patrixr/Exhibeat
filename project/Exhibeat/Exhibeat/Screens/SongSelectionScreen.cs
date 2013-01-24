using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humble;
using Humble.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exhibeat.Screens
{
    /// <summary>
    /// Ce screen propose au joueur les chansons
    /// TODO : prevoir un screen pour le tps de chargement des chansons
    /// Ce screen push les GameScreens pour lancer les parties
    /// </summary>
    class SongSelectionScreen : Screen
    {
        public SongSelectionScreen(HumbleGame game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
