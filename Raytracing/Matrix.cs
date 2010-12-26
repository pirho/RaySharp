using System;

namespace Raytracing {
    public class Matrix {
        private readonly double[] _data = new double[16];

// ReSharper disable InconsistentNaming
        public static readonly int TX = 3;
        public static readonly int TY = 7;
        public static readonly int TZ = 11;

        public static readonly int D0 = 0;
        public static readonly int D1 = 5;
        public static readonly int D2 = 10;
        public static readonly int D3 = 15;

        public static readonly int SX = D0;
        public static readonly int SY = D0;
        public static readonly int SZ = D0;

        public static readonly int W = D3;
// ReSharper restore InconsistentNaming
        
        public Matrix()
        {
            Identity();
        }

        public double this[int index] {
            get {
                return _data[index];
            }
            set {
                _data[index] = value;
            }
        }

        public void Identity()
        {
            _data[1] = _data[2] = _data[TX] = _data[4] = _data[6] = _data[TY] =
        _data[8] = _data[9] = _data[TZ] = _data[12] = _data[13] = _data[14] = 0;
            _data[D0] = _data[D1] = _data[D2] = _data[W] = 1;
        }

        public void Rotate(Vector3D pos, double rX, double rY, double rZ) {
            Matrix t = new Matrix();
            t.RotateX(rZ);
            RotateY(rY);
            Concatenate(t);
            t.RotateZ(rX);
            Concatenate(t);
            Translate(pos);
        }

        public void RotateX(double rX) {
            double sX = (double)Math.Sin(rX * Constants.PiOver180);
            double cX = (double)Math.Cos(rX * Constants.PiOver180);
            Identity();
            _data[5] = cX;
            _data[6] = sX;
            _data[9] = -sX;
            _data[10] = cX;
        }

        public void RotateY(double rY) {
            double sY = (double)Math.Sin(rY * Constants.PiOver180);
            double cY = (double)Math.Cos(rY * Constants.PiOver180);
            Identity();
            _data[0] = cY;
            _data[2] = -sY;
            _data[8] = sY;
            _data[10] = cY;
        }

        public void RotateZ(double rZ) {
            double sZ = (double)Math.Sin(rZ * Constants.PiOver180);
            double cZ = (double)Math.Cos(rZ * Constants.PiOver180);
            Identity();
            _data[0] = cZ;
            _data[1] = sZ;
            _data[4] = -sZ;
            _data[5] = cZ;
        }

        public void Translate(Vector3D pos) {
            _data[TX] += pos.X;
            _data[TY] += pos.Y;
            _data[TZ] += pos.Z;
        }

        public void Concatenate(Matrix m2) {
            Matrix result = new Matrix();
            for (int c = 0; c < 4; c++)
                for (int r = 0; r < 4; r++)
                    result._data[r*4 + c] = _data[r*4]*m2._data[c] +
                                            _data[r*4 + 1]*m2._data[c + 4] +
                                            _data[r*4 + 2]*m2._data[c + 8] +
                                            _data[r*4 + 3]*m2._data[c + 12];
            for (int c = 0; c < 16; c++) _data[c] = result._data[c];
        }

        public Vector3D Transform(Vector3D v) {
            double x = _data[0] * v.X + _data[1] * v.Y + _data[2] * v.Z + _data[3];
            double y = _data[4] * v.X + _data[5] * v.Y + _data[6] * v.Z + _data[7];
            double z = _data[8] * v.X + _data[9] * v.Y + _data[10] * v.Z + _data[11];
            return new Vector3D(x, y, z);
        }

        public void Invert() {
            Matrix t = new Matrix();
            double tx = -_data[3];
            double ty = -_data[7];
            double tz = -_data[11];

            for (int h = 0; h < 3; h++)
                for (int v = 0; v < 3; v++)
                    t._data[h + v*4] = _data[v + h*4];

            for (int i = 0; i < 11; i++)
                _data[i] = t._data[i];

            _data[3] = tx*_data[0] + ty*_data[1] + tz*_data[2];
            _data[7] = tx*_data[4] + ty*_data[5] + tz*_data[6];
            _data[11] = tx*_data[8] + ty*_data[9] + tz*_data[10];
        }
    }
}
