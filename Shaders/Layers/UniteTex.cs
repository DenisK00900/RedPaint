using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RedPaint.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class UniteTex
    {
        public static Texture2D CombineTextures(List<Texture2D> textures, List<float> alphaWeights = null)
        {
            if (textures == null || textures.Count == 0)
                throw new ArgumentException("Список текстур не может быть пустым");

            int width = textures[0].Width;
            int height = textures[0].Height;

            foreach (var texture in textures)
            {
                if (texture.Width != width || texture.Height != height)
                    throw new ArgumentException("Все текстуры должны быть одинакового размера");
            }

            if (alphaWeights != null && alphaWeights.Count != textures.Count)
                throw new ArgumentException("Коэффициенты alphaWeights должны соответствовать количеству текстур");

            Color[] combinedData = new Color[width * height];
            for (int i = 0; i < combinedData.Length; i++)
                combinedData[i] = Color.Transparent;

            Color[] textureData = new Color[width * height];

            for (int t = 0; t < textures.Count; t++)
            {
                var texture = textures[t];
                float weight = (alphaWeights != null && t < alphaWeights.Count)
                    ? MathHelper.Clamp(alphaWeights[t], 0f, 1f)
                    : 1f;

                texture.GetData(textureData);

                for (int i = 0; i < textureData.Length; i++)
                {
                    Color source = textureData[i];
                    Color destination = combinedData[i];

                    float sourceAlpha = (source.A / 255f) * weight;
                    float destAlpha = destination.A / 255f;

                    float resultAlpha = sourceAlpha + destAlpha * (1 - sourceAlpha);

                    if (resultAlpha > 0)
                    {
                        byte r = (byte)((source.R * sourceAlpha + destination.R * destAlpha * (1 - sourceAlpha)) / resultAlpha);
                        byte g = (byte)((source.G * sourceAlpha + destination.G * destAlpha * (1 - sourceAlpha)) / resultAlpha);
                        byte b = (byte)((source.B * sourceAlpha + destination.B * destAlpha * (1 - sourceAlpha)) / resultAlpha);
                        byte a = (byte)(resultAlpha * 255);

                        combinedData[i] = new Color(r, g, b, a);
                    }
                }
            }

            Texture2D resultTexture = new Texture2D(textures[0].GraphicsDevice, width, height);
            resultTexture.SetData(combinedData);
            return resultTexture;
        }

        public static Texture2D CombineTexturesSimple(List<Texture2D> textures, List<float> alphaWeights = null)
        {
            if (textures == null || textures.Count == 0)
                throw new ArgumentException("Список текстур не может быть пустым");

            int width = textures[0].Width;
            int height = textures[0].Height;

            if (alphaWeights != null && alphaWeights.Count != textures.Count)
                throw new ArgumentException("Коэффициенты alphaWeights должны соответствовать количеству текстур");

            Color[] combinedData = new Color[width * height];
            Color[] textureData = new Color[width * height];

            for (int i = 0; i < combinedData.Length; i++)
                combinedData[i] = Color.Transparent;

            for (int t = 0; t < textures.Count; t++)
            {
                var texture = textures[t];
                float weight = (alphaWeights != null && t < alphaWeights.Count)
                    ? MathHelper.Clamp(alphaWeights[t], 0f, 1f)
                    : 1f;

                texture.GetData(textureData);

                for (int i = 0; i < textureData.Length; i++)
                {
                    Color source = textureData[i];
                    byte weightedAlpha = (byte)(source.A * weight);

                    if (weightedAlpha > 0)
                    {
                        combinedData[i] = new Color(source.R, source.G, source.B, weightedAlpha);
                    }
                }
            }

            Texture2D resultTexture = new Texture2D(textures[0].GraphicsDevice, width, height);
            resultTexture.SetData(combinedData);
            return resultTexture;
        }
    }
}