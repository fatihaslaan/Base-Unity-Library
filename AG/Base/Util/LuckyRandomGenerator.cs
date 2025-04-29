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

        public float GetRandomValue(int maxVal)
        {
            return _random.Next(maxVal);
        }

        public float GetLuckyRandomValue(int maxVal)
        {
            return GetRandomValue(maxVal) * luck.Value;
        }
    }
}