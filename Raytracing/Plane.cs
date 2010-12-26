
namespace Raytracing {
    public sealed class Plane {
        public Plane() : this(Vector3D.Zero, 0f ) {
            
        }

        public Plane(Vector3D normal, double depth) {
            Normal = normal;
            Depth = depth;
        }

        public Vector3D Normal { get; set; }
        public double Depth { get; set; }
    }
}
