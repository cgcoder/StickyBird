using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Juicy.Engine;

namespace StickyBird
{
    public enum TextureShape
    {
        RECTANGLE, CIRCLE
    }
    
    public class TextureCreator
    {
        private SpriteManager sprManager;
        private GraphicsDevice device;
        private BasicEffect effect;

        public TextureCreator(SpriteManager sprManager, GraphicsDevice device)
        {
            this.sprManager = sprManager;
            this.device = device;

            effect = new BasicEffect(device);
        }

        public Texture2D CreateRectangleTexture(int repeatCount, string textureName, Color color)
        {
            /*VertexPositionColorTexture[] triangle1 = new VertexPositionColorTexture[3];
            VertexPositionColorTexture[] triangle2 = new VertexPositionColorTexture[3];

            VertexPositionColor[] outline1 = new VertexPositionColor[3];
            VertexPositionColor[] outline2 = new VertexPositionColor[3];

            triangle1[0].Position = new Vector3(0, 0, 0);
            triangle1[0].TextureCoordinate = new Vector2(0, 0);
            triangle1[0].Color = Color.White;

            triangle1[1].Position = new Vector3(width, 0, 0);
            triangle1[1].TextureCoordinate = new Vector2(1, 0);
            triangle1[1].Color = Color.White;

            triangle1[2].Position = new Vector3(width, height, 0);
            triangle1[2].TextureCoordinate = new Vector2(1, 1);
            triangle1[2].Color = Color.White;

            outline1[0].Position = new Vector3(0, 0, 0);
            outline1[0].Color = Color.White;
            outline1[1].Position = new Vector3(width, 0, 0);
            outline1[1].Color = Color.White;
            outline1[2].Position = new Vector3(width, height, 0);
            outline1[2].Color = Color.White;

            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0f);
            PresentationParameters pp = device.PresentationParameters;
            RenderTarget2D texture = new RenderTarget2D(device, width + 2, height + 2, false, SurfaceFormat.Color,
                                                        DepthFormat.None, pp.MultiSampleCount,
                                                        RenderTargetUsage.DiscardContents);
            device.RasterizerState = RasterizerState.CullNone;
            device.SamplerStates[0] = SamplerState.LinearClamp;

            device.SetRenderTarget(texture);
            device.Clear(Color.Transparent);
            effect.Projection = Matrix.CreateOrthographic(width + 2f, -height - 2f, 0f, 1f);
            effect.View = halfPixelOffset;
            render shape;
            effect.TextureEnabled = true;
            effect.Texture = sprManager.GetSprite(textureName);
            effect.VertexColorEnabled = true;
            effect.Techniques[0].Passes[0].Apply();
            for (int i = 0; i < 1; ++i)
            {
                device.DrawUserPrimitives(PrimitiveType.TriangleList, triangle1, 0, 1);
            }
            render outline;
            effect.TextureEnabled = false;
            effect.Techniques[0].Passes[0].Apply();
            device.DrawUserPrimitives(PrimitiveType.LineStrip, outline1, 0, 2);
            device.SetRenderTarget(null);

            return texture;
             * */

            return null;

        }
    }
}
