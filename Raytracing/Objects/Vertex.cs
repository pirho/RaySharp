
namespace Raytracing.Objects {
    public class Vertex {
        public Vertex() {
            
        }

        public Vertex(Vector3D pos, double u, double v) {
            Position = pos;
            U = u;
            V = v;
        }

        public Vector3D Normal { get; set; }
        public Vector3D Position { get; set; }
        public double U { get; set; }
        public double V { get; set; }

        public void SetUV(double u, double v) {
            U = u;
            V = v;
        }

        public override string ToString() {
            return string.Format("Normal = {0}, Position = {1}, U = {2}, V = {3}", Normal, Position, U, V);
        }
    }
}
