using Raytracing.Objects;

namespace Raytracing {
    public class ObjectList {
        public ObjectList() {
            Primitive = null;
            Next = null;
        }

        public ObjectList(Primitive p, ObjectList next) {
            Primitive = p;
            Next = next;
        }

        public Primitive Primitive { get; set; }
        public ObjectList Next { get; set; }
    }
}
