#define IMPORTANCE

using System;

namespace Raytracing {
    public static class Constants {
        public static int Samples = 32;
        public const int MaxTreeDepth = 20;
        public static int TileSize = 32;

        public const int TextHeight = 20;

        public const double Pi = Math.PI;
        public const double OneOverPi = 1 / Pi;
        public const double TwoOverPi = 2 / Pi;
        public const double PiOver180 = Pi/180;

        public const double Epsilon = 0.0000001f;
        public const double Maxdouble = double.MaxValue;
#if VERYHIGHPRECISION
        public static int TraceDepth = 16;
#else
        public static int TraceDepth = 4;
#endif

        public static bool ReflectionsEnabled = true;
        public static bool RefractionsEnabled = true;
    }
}