
namespace Raytracing {
    public enum RayHitDetection {
        Miss = 0, //The ray missed the primitive
        Hit = 1, //The ray hit the primitive
        InsidePrimitive = -1 //The ray started inside the primitive
    }
}
