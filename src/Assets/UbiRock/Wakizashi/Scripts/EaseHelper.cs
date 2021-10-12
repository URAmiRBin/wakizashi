using System;
using UnityEngine;

namespace UbiRock.Utils {
    public enum EaseType { CircInOut, CircOut, Cubic, Linear }
    class Easing {
        public static float CircInOut(float x) {
            if (x < 0.5f)
                return (float)((1 - Math.Sqrt(1 - Math.Pow(2*x, 2))) / 2);
            return (float)((Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2);
        }

        public static float CircOut(float x) {
            return (float)Math.Sqrt(1 - Math.Pow(x - 1, 2));
        }

        public static float Cubic(float x) {
            return 1 - (float)Math.Pow(1 - x, 3);
        }

        public static float Linear(float x) => x;

        public static float Ease(float value, EaseType type = EaseType.Linear) {
            switch (type) {
                case EaseType.CircInOut :
                    return CircInOut(value);
                case EaseType.CircOut:
                    return CircOut(value);
                case EaseType.Cubic:
                    return Cubic(value);
                default:
                    return Linear(value);
            }
        }
    }
}
