
using System;
using Raytracing.Primitives;

namespace Raytracing.Objects {
    public class Primitive {
        public Primitive() {
            
        }

        public Primitive(PrimitiveType type, Vector3D center, double radius) {
            Type = type;
            Center = center;
            Radius = radius;

            Material = new Material();
            // set vectors for texture mapping
            _ve = new Vector3D(1, 0, 0);
            _vn = new Vector3D(0, 1, 0);
            _vc = Vn.CrossProduct(Ve);
        }

        public Primitive(PrimitiveType type, Vertex v1, Vertex v2, Vertex v3) {
            Type = type;
            Material = null;
            _vertices = new[] {v1, v2, v3};
            // init precomp
            Vector3D posA = _vertices[0].Position;
            Vector3D posB = _vertices[1].Position;
            Vector3D posC = _vertices[2].Position;
            Vector3D c = posB - posA;
            Vector3D b = posC - posA;

            _n = b.CrossProduct(c);
            int u, v;
            if (Math.Abs(_n.X) > Math.Abs(_n.Y)) {
                _k = Math.Abs(_n.X) > Math.Abs(_n.Z) ? 0 : 2;
            }
            else {
                _k = Math.Abs(_n.Y) > Math.Abs(_n.Z) ? 1 : 2;
            }
            u = (_k + 1) % 3;
            v = (_k + 2) % 3;
            // precomp
            double krec = 1f/_n[_k];
            _nu = _n[u] * krec;
            _nv = _n[v] * krec;
            _nd = _n.DotProduct(posA)*krec;
            // first line equation
            double reci = 1f/(b[u]*c[v] - b[v]*c[u]);
            _bnu = b[u]*reci;
            _bnv = -b[v]*reci;
            // second line equation
            _cnu = c[v]*reci;
            _cnv = -c[u]*reci;
            // finalize normal
            _n = _n.Normalize();
            _vertices[0].Normal = _n;
            _vertices[1].Normal = _n;
            _vertices[2].Normal = _n;
        }

        public Material Material { get; set; }
        public PrimitiveType Type { get; private set; }

        #region Sphere properties

        private Vector3D _center;
        public Vector3D Center {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _center;
            }
            set {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                _center = value;
            }
        }

        private double _radius;
        public double Radius {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _radius;
            }
            set {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                _radius = value;
                _rRadius = 1f/_radius;
                _sqRadius = _radius*_radius;
            }
        }

        private double _rRadius;
        public double RRadius {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _rRadius;
            }
        }

        private double _sqRadius;
        public double SqRadius {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _sqRadius;
            }
        }

        private Vector3D _ve;
        public Vector3D Ve {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _ve;
            }
        }

        private Vector3D _vn;
        public Vector3D Vn {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _vn;
            }
        }

        private Vector3D _vc;
        public Vector3D Vc {
            get {
                if (Type != PrimitiveType.Sphere)
                    throw new NotSupportedException();

                return _vc;
            }
        }

        #endregion

        #region Vertex properties

        private Vector3D _n;
        public Vector3D N {
            get {
                if (Type != PrimitiveType.Vertex)
                    throw new NotSupportedException();

                return _n;
            }
            set {
                if (Type != PrimitiveType.Vertex)
                    throw new NotSupportedException();

                _n = value;
            }
        }

        private Vertex[] _vertices;
        public Vertex GetVertex(int index) {
            if (Type != PrimitiveType.Vertex)
                throw new NotSupportedException();

            if (index < 0 || index > _vertices.Length)
                throw new ArgumentOutOfRangeException();

            return _vertices[index];
        }

        public void SetVertex(int index, Vertex vertex) {
            if (Type != PrimitiveType.Vertex)
                throw new NotSupportedException();

            if (index < 0 || index > _vertices.Length)
                throw new ArgumentOutOfRangeException();

            _vertices[index] = vertex;
        }

        private double _u;
        private double _v;

        private double _nu;
        private double _nv;
        private double _nd;

        private int _k;

        private double _bnu;
        private double _bnv;

        private double _cnu;
        private double _cnv;
        #endregion

        public RayHitDetection Intersect(Ray ray, ref double dist) {
            if (Type == PrimitiveType.Sphere)
                return IntersectSphere(ray, ref dist);

            return IntersectVertex(ray, ref dist);
        }

        private RayHitDetection IntersectSphere(Ray ray, ref double dist) {
            Vector3D v = ray.Origin - Center;
            double b = -(v.DotProduct(ray.Direction));
            double det = (b * b) - v.DotProduct(v) + SqRadius;
            RayHitDetection retval = RayHitDetection.Miss;
            if (det > 0) {
                det = (double)Math.Sqrt(det);
                double i1 = b - det;
                double i2 = b + det;
                if (i2 > 0) {
                    if (i1 < 0) {
                        if (i2 < dist) {
                            dist = i2;
                            retval = RayHitDetection.InsidePrimitive;
                        }
                    }
                    else {
                        if (i1 < dist) {
                            dist = i1;
                            retval = RayHitDetection.Hit;
                        }
                    }
                }
            }
            return retval;
        }

        private readonly int[] _modulo = new[] { 0, 1, 2, 0, 1 };
        private RayHitDetection IntersectVertex(Ray ray, ref double dist) {
            Vector3D O = ray.Origin;
            Vector3D D = ray.Direction;
            Vector3D A = _vertices[0].Position;

            double lnd = 1.0f / (D[_k] + _nu * D[_modulo[_k + 1]] + _nv * D[_modulo[_k + 2]]);
            double t = (_nd - O[_k] - _nu * O[_modulo[_k + 1]] - _nv * O[_modulo[_k + 2]]) * lnd;
            if (!(dist > t && t > 0))
                return RayHitDetection.Miss;

            double hu = O[_modulo[_k + 1]] + t * D[_modulo[_k + 1]] - A[_modulo[_k + 1]];
            double hv = O[_modulo[_k + 2]] + t * D[_modulo[_k + 2]] - A[_modulo[_k + 2]];
            
            _u = hv * _bnu + hu * _bnv;
            double beta = _u;
            if (beta < 0)
                return RayHitDetection.Miss;

            _v = hu*_cnu + hv*_cnv;
            double gamma = _v;
            if (gamma < 0)
                return RayHitDetection.Miss;

            if ((_u + _v) > 1)
                return RayHitDetection.Miss;

            dist = t;
            return D.DotProduct(_n) > 0 ? RayHitDetection.InsidePrimitive : RayHitDetection.Hit;
        }

        public Vector3D GetNormal(Vector3D pos) {
            if (Type == PrimitiveType.Sphere)
                return (pos - Center)*RRadius;

            Vector3D n1 = _vertices[0].Normal;
            Vector3D n2 = _vertices[1].Normal;
            Vector3D n3 = _vertices[2].Normal;
            Vector3D n = (n1 + _u * (n2 - n1) + _v * (n3 - n1)).Normalize();
            
            return n;
        }

        public Color3D GetColor(Vector3D pos) {
            Color3D retval;
            if (Material == null)
                return new Color3D();

            if (Material.Texture == null)
                return Material.Color;

            if (Type == PrimitiveType.Sphere) {
                Vector3D vp = (pos - Center)*RRadius;
                double phi = (double)Math.Acos(vp.DotProduct(_vn));
                double u;
                double v = phi*Material.VScaleReci*Constants.OneOverPi;
                double theta = (double)(Math.Acos(_ve.DotProduct(vp)/Math.Sin(phi))*Constants.TwoOverPi);
                if (double.IsNaN(theta)) {
                    if (_ve.DotProduct(vp)/Math.Sin(phi) > 1)
                        theta = (double)(Math.Acos(1) * Constants.TwoOverPi);
                    else
                        theta = (double)(Math.Acos(-1) * Constants.TwoOverPi);
                }
                if (_vc.DotProduct(vp) >= 0)
                    u = (1f - theta)*Material.UScaleReci;
                else
                    u = theta * Material.UScaleReci;
                retval = Material.Texture.GetTexel(u, v)*Material.Color;
            }
            else {
                double u1 = _vertices[0].U;
                double v1 = _vertices[0].V;
                double u2 = _vertices[1].U;
                double v2 = _vertices[1].V;
                double u3 = _vertices[2].U;
                double v3 = _vertices[2].V;
                double u = u1 + _u * (u2 - u1) + _v * (u3 - u1);
                double v = v1 + _u * (v2 - v1) + _v * (v3 - v1);
                retval = Material.Texture.GetTexel(u, v)*Material.Color;
            }

            return retval;
        }

        private bool PlaneBoxOverlap(Vector3D normal, Vector3D vert, Vector3D maxBox) {
            Vector3D vmin = new Vector3D();
            Vector3D vmax = new Vector3D();
            for (int q = 0; q < 3; q++) {
                double v = vert[q];
                if (normal[q] > 0.0f) {
                    vmin[q] = -maxBox[q] - v;
                    vmax[q] = maxBox[q] - v;
                }
                else {
                    vmin[q] = maxBox[q] - v;
                    vmax[q] = -maxBox[q] - v;
                }
            }
            if (normal.DotProduct(vmin) > 0.0f) return false;
            if (normal.DotProduct(vmax) >= 0.0f) return true;
            return false;
        }

        private bool IntersectTriBox(Vector3D boxCenter, Vector3D boxHalfSize, Vector3D pv0, Vector3D pv1, Vector3D pv2) {
            Vector3D v0 = pv0 - boxCenter;
            Vector3D v1 = pv1 - boxCenter;
            Vector3D v2 = pv2 - boxCenter;
            Vector3D e0 = v1 - v0;
            Vector3D e1 = v2 - v1;
            Vector3D e2 = v0 - v2;
            double fex = Math.Abs(e0[0]);
            double fey = Math.Abs(e0[1]);
            double fez = Math.Abs(e0[2]);

            //AXISTEST_X01
            double p0 = e0[2]*v0[1] - e0[1]*v0[2];
            double p2 = e0[2]*v2[1] - e0[1]*v2[2];
            double min, max;
            if (p0 < p2) {
                min = p0;
                max = p2;
            }
            else {
                min = p2;
                max = p0;
            }
            double rad = fez*boxHalfSize[1] + fey*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Y02
            p0 = -e0[2]*v0[0] + e0[0]*v0[2];
            p2 = -e0[2]*v2[0] + e0[0]*v2[2];
            if (p0 < p2) {
                min = p0;
                max = p2;
            }
            else {
                min = p2;
                max = p0;
            }
            rad = fez*boxHalfSize[0] + fex*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Z12
            double p1 = e0[1]*v1[0] - e0[0]*v1[1];
            p2 = e0[1]*v2[0] - e0[0]*v2[1];
            if (p2 < p1) {
                min = p2;
                max = p1;
            }
            else {
                min = p1;
                max = p2;
            }
            rad = fey*boxHalfSize[0] + fex*boxHalfSize[1];
            if (min > rad || max < -rad)
                return false;

            fex = Math.Abs(e1[0]);
            fey = Math.Abs(e1[1]);
            fez = Math.Abs(e1[2]);

            //AXISTEST_X01
            p0 = e1[2]*v0[1] - e1[1]*v0[2];
            p2 = e1[2]*v2[1] - e1[1]*v2[2];
            if (p0 < p2) {
                min = p0;
                max = p2;
            }
            else {
                min = p2;
                max = p0;
            }
            rad = fez*boxHalfSize[1] + fey*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Y02
            p0 = -e1[2]*v0[0] + e1[0]*v0[2];
            p2 = -e1[2]*v2[0] + e1[0]*v2[2];
            if (p0 < p2) {
                min = p0;
                max = p2;
            }
            else {
                min = p2;
                max = p0;
            }
            rad = fez*boxHalfSize[0] + fex*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Z0
            p0 = e1[1]*v0[0] - e1[0]*v0[1];
            p1 = e1[1]*v1[0] - e1[0]*v1[1];
            if (p0 < p1) {
                min = p0;
                max = p1;
            }
            else {
                min = p1;
                max = p0;
            }
            rad = fey*boxHalfSize[0] + fex*boxHalfSize[1];
            if (min > rad || max < -rad)
                return false;

            fex = Math.Abs(e2[0]);
            fey = Math.Abs(e2[1]);
            fez = Math.Abs(e2[2]);

            //AXISTEST_X2
            p0 = e2[2]*v0[1] - e2[1]*v0[2];
            p1 = e2[2]*v1[1] - e2[1]*v1[2];
            if (p0 < p1) {
                min = p0;
                max = p1;
            }
            else {
                min = p1;
                max = p0;
            }
            rad = fez*boxHalfSize[1] + fey*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Y1
            p0 = -e2[2]*v0[0] + e2[0]*v0[2];
            p1 = -e2[2]*v1[0] + e2[0]*v1[2];
            if (p0 < p1) {
                min = p0;
                max = p1;
            }
            else {
                min = p1;
                max = p0;
            }
            rad = fez*boxHalfSize[0] + fez*boxHalfSize[2];
            if (min > rad || max < -rad)
                return false;

            //AXISTEST_Z12
            p1 = e2[1]*v1[0] - e2[0]*v1[1];
            p2 = e2[1]*v2[0] - e2[0]*v2[1];
            if (p2 < p1) {
                min = p2;
                max = p1;
            }
            else {
                min = p1;
                max = p2;
            }
            rad = fey*boxHalfSize[0] + fex*boxHalfSize[1];
            if (min > rad || max < -rad)
                return false;

            FindMinMax(v0[0], v1[0], v2[0], ref min, ref max);
            if (min > boxHalfSize[0] || max < -boxHalfSize[0])
                return false;
            
            FindMinMax(v0[1], v1[1], v2[1], ref min, ref max);
            if (min > boxHalfSize[1] || max < -boxHalfSize[1])
                return false;

            FindMinMax(v0[2], v1[2], v2[2], ref min, ref max);
            if (min > boxHalfSize[2] || max < -boxHalfSize[2])
                return false;

            Vector3D normal = e0.CrossProduct(e1);
            return PlaneBoxOverlap(normal, v0, boxHalfSize);
        }

        private static void FindMinMax(double x0, double x1, double x2, ref double min, ref double max) {
            max = x0;
            min = max;
            if (x1 < min)
                min = x1; 
            if (x1 > max) 
                max = x1; 
            if (x2 < min) 
                min = x2; 
            if (x2 > max) 
                max = x2;
        }

        private bool IntersectSphereBox(Vector3D center, AxisAlignedBox box) {
            double dmin = 0;
            Vector3D spos = center;
            Vector3D bpos = box.Position;
            Vector3D bsize = box.Size;

            for (int i = 0; i < 3; i++) {
                if (spos[i] < bpos[i]) {
                    dmin = dmin + (spos[i] - bpos[i]) * (spos[i] - bpos[i]);
                }
                else if (spos[i] > (bpos[i] + bsize[i])) {
                    dmin = dmin + (spos[i] - (bpos[i] + bsize[i])) * (spos[i] - (bpos[i] + bsize[i]));
                }
            }
            return (dmin <= SqRadius);
        }

        public bool IntersectBox(AxisAlignedBox box) {
            if (Type == PrimitiveType.Sphere)
                return IntersectSphereBox(Center, box);

            return IntersectTriBox(box.Position + box.Size*.5f, box.Size*.5f, _vertices[0].Position,
                                   _vertices[1].Position, _vertices[2].Position);
        }

        public void CalculateRange(ref double pos1, ref double pos2, int axis) {
            if (Type == PrimitiveType.Sphere) {
                pos1 = Center[axis] - Radius;
                pos2 = Center[axis] + Radius;
                return;
            }

            Vector3D vPos1 = _vertices[0].Position;
            pos1 = vPos1[axis];
            pos2 = vPos1[axis];
            for (int i = 1; i < 3; i++) {
                Vector3D pos = _vertices[i].Position;
                if (pos[axis] < pos1) pos1 = pos[axis];
                if (pos[axis] > pos2) pos2 = pos[axis];
            }
        }
    }
}