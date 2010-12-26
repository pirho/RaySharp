
namespace Raytracing {
    public sealed class Ray {
        public Ray() : this(Vector3D.Zero, Vector3D.Zero) {
            
        }

        public Ray(Vector3D origin, Vector3D direction) {
            Origin = origin;
            Direction = direction;
        }

        public Vector3D Origin { get; set; }
        public Vector3D Direction { get; set; }
    }
}
