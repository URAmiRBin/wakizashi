using System;

namespace UbiRock.Utils {
    public enum EaseType { CircInOut, Linear }
    class Easing {
        public static double CircInOut(float x) {
            if (x < 0.5f)
                return (1 - Math.Sqrt(1 - Math.Pow(2*x, 2))) / 2;
            return (Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2;
        }

        public static double Linear(float x) => x;

        public static double Ease(float value, EaseType type = EaseType.Linear) {
            switch (type) {
                case EaseType.CircInOut :
                    return CircInOut(value);
                default:
                    return Linear(value);
            }
        }
    }
}
