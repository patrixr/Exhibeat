using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Exhibeat.Shaders
{
    class BlurEffect :  AEffect
    {
        private const float BLUR_BUFFER_SCALE = 1f / 2f;

        private Effect _blur;
        private RenderTarget2D _horzBuffer;
        private RenderTarget2D _vertBuffer;

        public BlurEffect(GraphicsDevice graphicsDevice, ContentManager content) : base(graphicsDevice, content)
        {
            int bbWidth;
            int bbHeight;

            _blur = _content.Load<Effect>("GaussianBlur");
            bbWidth = (int)(graphicsDevice.Viewport.Width * BLUR_BUFFER_SCALE);
            bbHeight = (int)(graphicsDevice.Viewport.Height * BLUR_BUFFER_SCALE);
            _horzBuffer = new RenderTarget2D(_device, bbWidth, bbHeight);
            _vertBuffer = new RenderTarget2D(_device, bbWidth, bbHeight);
        }
        override public void start()
        {
            _device.SetRenderTarget(_buffer);
            _device.Clear(Color.Black);


        }
        override public void end()
        {
            _device.SetRenderTarget(null);
        }
        override public void applyEffect(SpriteBatch spriteBatch)
        {
            Texture2D scene;
            scene = (Texture2D)_buffer;

            // do the horizontal pass
            _device.SetRenderTarget(_horzBuffer);
            setBlurEffectParameters(1.0f / _horzBuffer.Width, 0f);
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, _blur);
            spriteBatch.Draw(scene, _horzBuffer.Bounds, Color.White);
            spriteBatch.End();
            // do the vertical pass
            _device.SetRenderTarget(_vertBuffer);
            setBlurEffectParameters(0f, 1.0f / _horzBuffer.Height);
            spriteBatch.Begin(0, BlendState.Opaque, null, null, null, _blur);
            spriteBatch.Draw((Texture2D)_horzBuffer, _vertBuffer.Bounds, Color.White);
            spriteBatch.End();
            // draw the blured scene whith the original one on the screen
            _device.SetRenderTarget(null);
            
            /*spriteBatch.Begin();
            spriteBatch.Draw(scene, new Vector2(0, 0), Color.White);
            spriteBatch.End();*/
            spriteBatch.Begin(0, BlendState.Opaque);
            spriteBatch.Draw((Texture2D)_vertBuffer, _buffer.Bounds, Color.White);
            spriteBatch.End();
        }
        private void setBlurEffectParameters(float dx, float dy)
        {
            // TODO: bufferzise effect settings
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = _blur.Parameters["SampleWeights"];
            offsetsParameter = _blur.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }
        private float ComputeGaussian(float n)
        {
            const float theta = 4;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }
}
