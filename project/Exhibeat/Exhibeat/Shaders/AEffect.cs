using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exhibeat.Shaders
{
    abstract class AEffect : IEffect
    {
        protected RenderTarget2D _buffer;
        protected ContentManager _content;
        protected GraphicsDevice _device;

        public AEffect(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _content = content;
            _device = graphicsDevice;

           _buffer = new RenderTarget2D(_device, _device.Viewport.Width, _device.Viewport.Height);
        }

        public abstract void start();
        public abstract void end();
        public abstract void applyEffect(SpriteBatch spriteBatch);
    }
}
