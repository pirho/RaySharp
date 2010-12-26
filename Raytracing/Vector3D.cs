using System;

namespace Raytracing {
    public class Vector3D {
        public Vector3D() : this(0, 0, 0) {
           
        }

        public Vector3D(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public readonly static Vector3D Zero = new Vector3D(0, 0, 0);

        //Returns the negative counterpart of Vector3D v
        public static Vector3D operator -(Vector3D v) {
            return new Vector3D(-v.X, -v.Y, -v.Z);
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3D operator *(double f, Vector3D v1) {
            return new Vector3D(v1.X * f, v1.Y * f, v1.Z * f);
        }

        public static Vector3D operator *(Vector3D v1, double f) {
            return new Vector3D(v1.X * f, v1.Y * f, v1.Z * f);
        }

        public static Vector3D operator *(Vector3D v1, Vector3D v2) {
            return new Vector3D(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
		
        public double Length {
            get { return (double)Math.Sqrt(SqrLength); }
        }

        public double SqrLength {
            get { return X * X + Y * Y + Z * Z; }
        }

        public double DotProduct(Vector3D v) {
            return X*v.X + Y*v.Y + Z*v.Z;
        }

        public Vector3D CrossProduct(Vector3D v) {
            return new Vector3D(Y*v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
        }

        public Vector3D Normalize() {
            double length = Length;
            if (length == 0)
                return Zero;

            double l = (double)(1.0 / Length);
            return new Vector3D(X*l, Y*l, Z*l);
        }

        public double this[int index] {
            get {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException();

                if (index == 0)
                    return X;
                return index == 1 ? Y : Z;
            }
            set {
                if (index < 0 || index > 2)
                    throw new ArgumentOutOfRangeException();

                if (index == 0)
                    X = value;
                if (index == 1)
                    Y = value;
                Z = value;
            }
        }

        public override string ToString() {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
    }
}
