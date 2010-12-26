
namespace Raytracing {
    public sealed class AxisAlignedBox {
        public AxisAlignedBox() : this(Vector3D.Zero, Vector3D.Zero) {
            
        }

        public AxisAlignedBox(Vector3D position, Vector3D size) {
            Position = position;
            Size = size;
        }

        public Vector3D Position { get; private set; }
        public Vector3D Size { get; private set; }

        public bool Intersect(AxisAlignedBox box) {
            Vector3D v1 = box.Position;
            Vector3D v2 = box.Position + box.Size;
            Vector3D v3 = Position;
            Vector3D v4 = Position + Size;

            return ((v4.X >= v1.X) && (v3.X <= v2.X) && // x-axis overlap
                    (v4.Y >= v1.Y) && (v3.Y <= v2.Y) && // y-axis overlap
                    (v4.Z >= v1.Z) && (v3.Z <= v2.Z)); // z-axis overlap
        }

        public bool Contains(Vector3D v) {
            Vector3D v1 = Position;
            Vector3D v2 = Position + Size;

            return ((v.X >= v1.X) && (v.X <= v2.X) &&
                    (v.Y >= v1.Y) && (v.Y <= v2.Y) &&
                    (v.Z >= v1.Z) && (v.Z <= v2.Z));
        }

        public double W {
            get {
                return Size.X;
            }
        }

        public double H {
            get {
                return Size.Y;
            }
        }

        public double D {
            get {
                return Size.Z;
            }
        }

        public double X {
            get {
                return Position.X;
            }
        }

        public double Y {
            get {
                return Position.Y;
            }
        }

        public double Z {
            get {
                return Position.Z;
            }
        }
    }
}
