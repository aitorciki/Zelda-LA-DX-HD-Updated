using System;

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
    }
}
