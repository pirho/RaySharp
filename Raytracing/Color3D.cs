using System;
using System.Drawing;

namespace Raytracing {
    public sealed class Color3D {
        public Color3D() : this(0, 0, 0) {
            
        }

        public Color3D(double r, double g, double b) {
            R = r;
            G = g;
            B = b;
        }

        public Color3D(int r, int g, int b) {
            R = r/255.0;
            G = g/255.0;
            B = b/255.0;
        }

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public void SetColor(double r, double g, double b) {
            R = r;
            G = g;
            B = b;
        }

        public void SetColor(Color3D c) {
            R = c.R;
            G = c.G;
            B = c.B;
        }

        public Color ToColor() {
            int r = (int)(R * 256) > 255 ? 255 : (int)(R * 256);
            int g = (int)(G * 256) > 255 ? 255 : (int)(G * 256);
            int b = (int)(B * 256) > 255 ? 255 : (int)(B * 256);
            return Color.FromArgb(r, g, b);
        }

        public static Color3D operator *(double f, Color3D fc) {
            double r = f * fc.R;
            double g = f * fc.G;
            double b = f * fc.B;
            return new Color3D(r, g, b);
        }

        public static Color3D operator *(Color3D fc, double f) {
            double r = f * fc.R;
            double g = f * fc.G;
            double b = f * fc.B;
            return new Color3D(r, g, b);
        }

        public static Color3D operator *(Color3D f1, Color3D f2) {
            double r = f1.R * f2.R;
            double g = f1.G * f2.G;
            double b = f1.B * f2.B;
            return new Color3D(r, g, b);
        }

        public static Color3D operator +(Color3D f1, Color3D f2) {
            double r = f1.R + f2.R;
            double g = f1.G + f2.G;
            double b = f1.B + f2.B;
            return new Color3D(r, g, b);
        }

        public static Color3D operator -(Color3D f1, Color3D f2) {
            double r = f1.R - f2.R;
            double g = f1.G - f2.G;
            double b = f1.B - f2.B;
            return new Color3D(r, g, b);
        }

        public override string ToString() {
            return string.Format("({0}, {1}, {2})", R, G, B);
        }
    }
}
