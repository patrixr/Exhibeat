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
    /// Sera cree en dessous d'un autre screen pour gerer le background
    /// Ceci est optionnel, il a ete cree en tant que squelette pour la creation du jeu
    /// </summary>
    class AnimatedBackgroundScreen : Screen
    {
        public AnimatedBackgroundScreen(HumbleGame game)
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
