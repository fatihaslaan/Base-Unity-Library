using System;
using AG.Base.Variable;

namespace AG.Base.Util
{
    public class LuckyRandomGenerator
    {
        protected FloatVariable luck;
        private Random _random;

        public LuckyRandomGenerator(FloatVariable luck)
        {
            _random = new Random();
            this.luck = luck;
        }

        public int GetRandomValue(int minVal, int maxVal)
        {
            return _random.Next(minVal, maxVal);
        }

        public int GetRandomValue(int maxVal)
        {
            return _random.Next(maxVal);
        }

        public float GetLuckyRandomValue(int minVal, int maxVal)
        {
            return GetRandomValue(minVal, maxVal) * luck.Value;
        }

        public float GetLuckyRandomValue(int maxVal)
        {
            return GetRandomValue(maxVal) * luck.Value;
        }
    }
}