using System;
using ProjectZ.Base;
using Microsoft.Xna.Framework;

namespace ProjectZ.InGame.Things
{
    public static class GameMath
    {
        private static Random _rand = new Random();

        public static int GetRandomInt(int min, int max)
        {
            return _rand.Next(min, max + 1);
        }

        public static float GetRandomFloat(float min, float max)
        {
            return (float)(min + (max - min) * Game1.RandomNumber.NextDouble());
        }

        public static RectangleF RectToRectF(Rectangle rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle RectFToRect(RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
