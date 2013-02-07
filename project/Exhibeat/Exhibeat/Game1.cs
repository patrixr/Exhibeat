using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Humble;
using Humble.Components;
using Exhibeat.Screens;

// ADD
using Exhibeat.Gameplay;
using Exhibeat.Parser;

namespace Exhibeat
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : HumbleGame
    {

        private FPSDisplay fpsDisplay;

        public Game1()
        {
        }

        protected override void Initialize()
        {
#if DEBUG
            fpsDisplay = new FPSDisplay(Content.Load<SpriteFont>("plain"));
#endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Err mgmnt
            this.screenManager.pushScreen(new MainMenuScreen(this));

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
#if DEBUG
            fpsDisplay.Update(gameTime);
#endif

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
#if DEBUG
            fpsDisplay.Draw(this.spriteBatch);
#endif

            base.Draw(gameTime);
        }
    }
}
