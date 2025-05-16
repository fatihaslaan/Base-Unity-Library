using System;
using UnityEngine;

namespace AG.Base.Util
{
    public static class Helper
    {
        //Get Position Difference Regardless Of It's Rotation
        public static float GetPositionDifferenceByRotation(float rotation, float xPosition, float yPosition)
        {
            return (Mathf.Sin(rotation * Mathf.Deg2Rad) * yPosition) + (Mathf.Cos(rotation * Mathf.Deg2Rad) * xPosition);
        }

        //Adjust Value To Limit It Within A Specified Value Based On A Max Value
        public static float AdjustValue(this float value, Func<float, float> CalculateMaxValueFunction, float valueLimit)
        {
            if (CalculateMaxValueFunction(value) > valueLimit)
            {
                value *= 0.9f;
                return AdjustValue(value, CalculateMaxValueFunction, valueLimit);
            }
            return value;
        }

        //Calculates Value For An Item At A Specific Index In A Sequence
        public static float GetSequenceValueByItemIndex(this float value, int index, int totalCount)
        {
            return (index * value) - GetSequenceValue(value, totalCount);
        }

        //Calculates Value Of An Item At Border In A Sequence
        public static float GetSequenceValue(this float value, int totalCount)
        {
            return (totalCount - 1) * value / 2;
        }
    }
}