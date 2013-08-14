using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Exhibeat.Shaders
{
    public interface IEffect
    {
        void start();
        void end();
        void applyEffect(SpriteBatch spriteBatch, Texture2D predraw = null);
    }
}
