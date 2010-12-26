using System;
using System.Drawing;
using Raytracing.Objects;

namespace Raytracing {
    public class Engine {
        private Vector3D _origin;
        private Vector3D _p1;
        private Vector3D _p2;
        private Vector3D _p3;
        private Vector3D _p4;
        private Vector3D _dX;
        private Vector3D _dY;

        private readonly Scene _scene;
        private Twister _twister;
        private Bitmap _frameBuffer;
        
        private int _width;
        private int _height;
        private int _currCol, _currRow;
        private int _xTiles, _yTiles;

        class KdStackItem {
            public KdTreeNode Node { get; set; }
            public double T { get; set; }
            public Vector3D Pb { get; set; }
            public int Prev { get; set; }
        }

        private int[] _mod;
        private KdStackItem[] _stack;

        public int RaysCast { get; private set; }
        public int Intersections { get; private set; }

        public Engine() {
            _scene = new Scene();
            
            _mod = new int[64];
            _stack = new KdStackItem[64];
            for (int i = 0; i < 64; i++)
                _stack[i] = new KdStackItem();
        }

        public Scene Scene {
            get { return _scene; }
        }

        public Bitmap FrameBuffer {
            get { return _frameBuffer; }
        }

        public void SetTarget(int width, int height) {
            _frameBuffer = new Bitmap(width, height);
            _width = width;
            _height = height;
        }

        private RayHitDetection FindNearest(Ray ray, ref double distance, ref Primitive primitive) {
            RayHitDetection retval = RayHitDetection.Miss;
            double tnear = 0;
            double tfar = distance;
            double t;

            Vector3D p1 = _scene.Extends.Position;
            Vector3D p2 = p1 + _scene.Extends.Size;
            Vector3D d = ray.Direction;
            Vector3D o = ray.Origin;
            for (int i = 0; i < 3; i++) {
                if (d[i] < 0) {
                    if (o[i] < p1[i])
                        return RayHitDetection.Miss;
                }
                else if (o[i] > p2[i])
                    return RayHitDetection.Miss;
            }

            //Clip ray segment to box
            for (int i = 0; i < 3; i++) {
                double pos = o[i] + tfar * d[i];
                if (d[i] < 0) {
                    // clip end point
                    if (pos < p1[i]) tfar = tnear + (tfar - tnear) * ((o[i] - p1[i]) / (o[i] - pos));
                    // clip start point
                    if (o[i] > p2[i]) tnear += (tfar - tnear) * ((o[i] - p2[i]) / (tfar * d[i]));
                }
                else {
                    // clip end point
                    if (pos > p2[i]) tfar = tnear + (tfar - tnear) * ((p2[i] - o[i]) / (pos - o[i]));
                    // clip start point
                    if (o[i] < p1[i]) tnear += (tfar - tnear) * ((p1[i] - o[i]) / (tfar * d[i]));
                }
                if (tnear > tfar) return RayHitDetection.Miss;
            }

            // init stack
	        int entrypoint = 0, exitpoint = 1;
	        // init traversal
	        KdTreeNode farchild;
            KdTreeNode currnode = _scene.KdTree.Root;

	        _stack[entrypoint].T = tnear;
	        if (tnear > 0.0f)
                _stack[entrypoint].Pb = o + d * tnear;
		    else
                _stack[entrypoint].Pb = o;

            _stack[exitpoint].T = tfar;
            _stack[exitpoint].Pb = o + d * tfar;
            _stack[exitpoint].Node = null;

            // traverse kd-tree
            while (currnode != null) {
                while (currnode.IsLeaf) {
                    //while (!currnode.IsLeaf) {
                    double splitpos = currnode.SplitPosition;
                    int axis = currnode.Axis;
                    if (_stack[entrypoint].Pb[axis] <= splitpos) {
                        if (_stack[exitpoint].Pb[axis] <= splitpos) {
                            currnode = currnode.Left;
                            continue;
                        }
                        if (_stack[exitpoint].Pb[axis] == splitpos) {
                            currnode = currnode.Right;
                            continue;
                        }
                        currnode = currnode.Left;
                        farchild = currnode.Right;
                    }
                    else {
                        if (_stack[exitpoint].Pb[axis] > splitpos) {
                            currnode = currnode.Right;
                            continue;
                        }
                        farchild = currnode.Left;
                        currnode = farchild.Right;
                    }
                    t = (splitpos - o[axis]) / d[axis];
                    int tmp = exitpoint++;
                    if (exitpoint == entrypoint) exitpoint++;
                    _stack[exitpoint].Prev = tmp;
                    _stack[exitpoint].T = t;
                    _stack[exitpoint].Node = farchild;
                    _stack[exitpoint].Pb[axis] = splitpos;
                    int nextaxis = _mod[axis + 1];
                    int prevaxis = _mod[axis + 2];
                    _stack[exitpoint].Pb[nextaxis] = o[nextaxis] + t * d[nextaxis];
                    _stack[exitpoint].Pb[prevaxis] = o[prevaxis] + t * d[prevaxis];
                }
                ObjectList list = currnode.List;
                double dist = _stack[exitpoint].T;
                while (list != null && list.Primitive != null) {
                    Primitive pr = list.Primitive;
                    RayHitDetection result;
                    Intersections++;
                    if ((result = pr.Intersect(ray, ref dist)) != RayHitDetection.Miss) {
                        retval = result;
                        distance = dist;
                        primitive = pr;
                    }
                    list = list.Next;
                }

                if (retval != RayHitDetection.Miss)
                    return retval;

                entrypoint = exitpoint;
                currnode = _stack[exitpoint].Node;
                exitpoint = _stack[entrypoint].Prev;
            }

            return RayHitDetection.Miss;
        }

        private int FindOccluder(Ray ray, ref double distance) {
            double tnear = Constants.Epsilon;
            double t;
            Vector3D o = ray.Origin;
            Vector3D d = ray.Direction;
            //init stack
            int entrypoint = 0;
            int exitpoint = 0;
            // init traversal
	        KdTreeNode farchild;
            KdTreeNode currnode = _scene.KdTree.Root;
	        _stack[entrypoint].T = tnear;
            _stack[entrypoint].Pb = o;
            _stack[exitpoint].T = distance;
            _stack[exitpoint].Pb = o + d * distance;
            _stack[exitpoint].Node = null;
	        // traverse kd-tree
	        while (currnode != null) {
                while (currnode.IsLeaf) {
                    //while (!currnode.IsLeaf) {
			        double splitpos = currnode.SplitPosition;
			        int axis = currnode.Axis;
                    if (_stack[entrypoint].Pb[axis] <= splitpos) {
                        if (_stack[exitpoint].Pb[axis] <= splitpos) {
                            currnode = currnode.Left;
					        continue;
				        }
                        if (_stack[exitpoint].Pb[axis] == splitpos) {
                            currnode = currnode.Right;
					        continue;
				        }
                        currnode = currnode.Left;
                        farchild = currnode.Right;
			        }
			        else {
                        if (_stack[exitpoint].Pb[axis] > splitpos) {
                            currnode = currnode.Right;
					        continue;
				        }
                        farchild = currnode.Left;
                        currnode = farchild.Right;
			        }
			        t = (splitpos - o[axis]) / d[axis];
			        int tmp = exitpoint;
			        if (++exitpoint == entrypoint)
                        exitpoint++;

                    _stack[exitpoint].Prev = tmp;
                    _stack[exitpoint].T = t;
                    _stack[exitpoint].Node = farchild;
                    _stack[exitpoint].Pb[axis] = splitpos;
			        int nextaxis = _mod[axis + 1];
			        int prevaxis = _mod[axis + 2];
                    _stack[exitpoint].Pb[nextaxis] = o[nextaxis] + t * d[nextaxis];
                    _stack[exitpoint].Pb[prevaxis] = o[prevaxis] + t * d[prevaxis];
		        }
	            ObjectList list = currnode.List;
		        double dist = distance; // m_Stack[exitpoint].t;
                while (list != null && list.Primitive != null) {
			        Intersections++;
			        if (list.Primitive.Intersect(ray, ref dist) != RayHitDetection.Miss)
                        return 1;

		            list = list.Next;
		        }

		        entrypoint = exitpoint;
                currnode = _stack[exitpoint].Node;
                exitpoint = _stack[entrypoint].Prev;
	        }

	        return 0;
        }

        private Primitive Raytrace(Ray ray, ref Color3D acc, int depth, double rIndex, ref double distance, double samples, double sScale) {
            //trace primary ray
            distance = Constants.Maxdouble;
            Primitive prim = null;
            
            //find the nearest intersection
            RayHitDetection result = FindNearest(ray, ref distance, ref prim);

            //No hit? Return null then
            if (result == RayHitDetection.Miss || prim == null)
                return null;

            //Determine color at point of intersection
            Vector3D pi = ray.Origin + ray.Direction * distance;
            Color3D color = prim.GetColor(pi);
            Vector3D n = prim.GetNormal(pi);
            
            //trace lights
            foreach (Light light in _scene.Lights) {
                Vector3D l = null;
                double shade = CalcShade(light, pi, ref l, samples, sScale);
                if (shade > 0) {
                    //calculate diffuse shading
                    if (prim.Material.Diffuse > 0) {
                        double dot = l.DotProduct(n);
                        if (dot > 0) {
                            double diff = dot * prim.Material.Diffuse * shade;
                            //add diffuse component to ray color
                            acc += diff*color*light.Color;
                        }
                    }
                    // determine specular component using Schlick's BRDF approximation
                    if (prim.Material.Specular > 0) {
                        //point light source: sample once for specular highlight
                        Vector3D r = l - 2 * l.DotProduct(n) * n;
                        double dot = ray.Direction.DotProduct(r);
                        if (dot > 0) {
                            double spec = dot*prim.Material.Specular*shade/(50 - 50*dot + dot);
                            //add specular component to ray color
                            acc += spec*light.Color;
                        }
                    }
                }
            }

            //Calculate reflections
            if (Constants.ReflectionsEnabled) {
                double refl = prim.Material.Reflection;
                if (refl > 0 && depth < Constants.TraceDepth) {
                    double drefl = prim.Material.DiffuseReflection;
                    if (drefl > 0 && depth < 3) {
                        //calculate diffuse reflection
                        Vector3D rp = ray.Direction - 2*ray.Direction.DotProduct(n)*n;
                        Vector3D rn1 = new Vector3D(rp.Z, rp.Y, -rp.X);
                        Vector3D rn2 = rp.CrossProduct(rn1);
                        refl *= sScale;
                        for (int i = 0; i < Constants.Samples; i++) {
                            double xoffs, yoffs;
                            do {
                                xoffs = (_twister.Rand - .5)*drefl;
                                yoffs = (_twister.Rand - .5)*drefl;
                            } while (xoffs*xoffs + yoffs*yoffs > drefl*drefl);

                            Vector3D r = (rp + rn1*xoffs + rn2*yoffs*drefl).Normalize();
                            Color3D rCol = new Color3D(0, 0, 0);
                            double dist = 0;
                            Raytrace(new Ray(pi + r*Constants.Epsilon, r), ref rCol, depth + 1, rIndex, ref dist,
                                     samples*.25, sScale*4);
                            RaysCast++;
                            acc += refl*rCol*color;
                        }
                    }
                    else {
                        //calculate perfect reflection
                        Vector3D r = ray.Direction - 2*ray.Direction.DotProduct(n)*n;
                        Color3D rCol = new Color3D(0, 0, 0);
                        double dist = 0;
                        Raytrace(new Ray(pi + r*Constants.Epsilon, r), ref rCol, depth + 1, rIndex, ref dist, samples*.5,
                                 sScale*2);
                        RaysCast++;
                        acc += refl*rCol*color;
                    }
                }
            }

            //Calculate refraction
            if (Constants.RefractionsEnabled) {
                double refr = prim.Material.Refraction;
                if (refr > 0 && depth < Constants.TraceDepth) {
                    double localRIndex = prim.Material.RefractionIndex;
                    double indexDiff = rIndex/localRIndex;
                    Vector3D vN = prim.GetNormal(pi)*(double) result;
                    double cosI = -(vN.DotProduct(ray.Direction));
                    double cosT2 = 1f - indexDiff*indexDiff*(1f - cosI*cosI);
                    if (cosT2 > 0) {
                        Vector3D t = indexDiff*ray.Direction + (indexDiff*cosI - Math.Sqrt(cosT2))*vN;
                        Color3D rCol = new Color3D();
                        double dist = 0;
                        Raytrace(new Ray(pi + t*Constants.Epsilon, t), ref rCol, depth + 1, localRIndex, ref dist,
                                 samples*.5, sScale*2);
                        RaysCast++;

                        //Apply Beer's law
                        Color3D absorbance = prim.Material.Color*.15*-dist;
                        Color3D transparency = new Color3D(Math.Exp(absorbance.R), Math.Exp(absorbance.G),
                                                           Math.Exp(absorbance.B));
                        acc += rCol*transparency;
                    }
                }
            }

            return prim;
        }

        private double CalcShade(Light light, Vector3D ip, ref Vector3D direction, double samples, double sScale) {
            double retVal = 0;
            if (light.Type == Light.LightType.Point) {
                //Handle point light source
                retVal = 0;
                direction = light.Position - ip;
                double tdist = direction.Length;
                direction *= 1/tdist;
                tdist *= 1 - 4*Constants.Epsilon;
                RaysCast++;
                if (FindOccluder(new Ray(ip + direction * Constants.Epsilon, direction), ref tdist) == 0)
                    return 1;
            }
            else if (light.Type == Light.LightType.Area) {
                // Monte Carlo rendering
                retVal = 0;
                direction = (light.Position - ip).Normalize();
                Vector3D deltax = light.CellX;
                Vector3D deltay = light.CellY;
                for (int i = 0; i < samples; i++) {
                    Vector3D lp = light.GetGrid(i & 15) + _twister.Rand*deltax + _twister.Rand*deltay;
                    Vector3D dir = lp - ip;
                    double ldist = dir.Length;
                    dir *= 1/ldist;
                    ldist *= 1 - 4*Constants.Epsilon;
                    RaysCast++;
                    if (FindOccluder(new Ray(ip + dir * Constants.Epsilon, dir), ref ldist) == 0)
                        retVal += sScale;
                }
            }

            return retVal;
        }

        public void InitRenderer(Vector3D pos, Vector3D target) {
            // set eye and screen plane position
	        _origin = new Vector3D( 0, 0, -5 );
	        _p1 = new Vector3D( -4,  3, 0 );
	        _p2 = new Vector3D(  4,  3, 0 );
	        _p3 = new Vector3D(  4, -3, 0 );
	        _p4 = new Vector3D( -4, -3, 0 );
	        // calculate camera matrix
	        Vector3D zaxis = (target - pos).Normalize();
	        Vector3D up = new Vector3D( 0, 1, 0 );
	        Vector3D xaxis = up.CrossProduct( zaxis );
	        Vector3D yaxis = xaxis.CrossProduct( -zaxis );
	        Matrix m = new Matrix();
	        m[0] = xaxis.X;
            m[1] = xaxis.Y;
            m[2] = xaxis.Z;
	        m[4] = yaxis.X;
            m[5] = yaxis.Y;
            m[6] = yaxis.Z;
	        m[8] = zaxis.X;
            m[9] = zaxis.Y;
            m[10] = zaxis.Z;
	        m.Invert();
	        m[3] = pos.X;
            m[7] = pos.Y;
            m[11] = pos.Z;
	        // move camera
	        _origin = m.Transform(_origin);
	        _p1 = m.Transform( _p1 );
	        _p2 = m.Transform( _p2 );
	        _p3 = m.Transform( _p3 );
	        _p4 = m.Transform( _p4 );
	        // calculate screen plane interpolation vectors
	        _dX = (_p2 - _p1) * (1.0 / _width);
	        _dY = (_p4 - _p1) * (1.0 / _height);
            
            // setup the tile renderer
            _currCol = 0;
            _currRow = 0; // 20 / Constants.TileSize;
            _xTiles = _width / Constants.TileSize;
            _yTiles = _height / Constants.TileSize; // (_height - 20) / Constants.TileSize;
            
            // reset counters
            Intersections = 0;
            RaysCast = 0;

            _twister = new Twister(0);
        }

        private Primitive RenderRay(Vector3D screenPos, ref Color3D acc) {
            Vector3D dir = (screenPos - _origin).Normalize();
            Ray r = new Ray(_origin, dir);
            RaysCast++;
            double dist = 0;
            //trace ray
            return Raytrace(r, ref acc, 1, 1.0, ref dist, Constants.Samples, 1.0 / Constants.Samples);
        }

        public bool RenderTiles() {
            //render scene
            long ticks = DateTime.Now.Ticks;
            // render remaining tiles
            int tx = _currCol, ty = _currRow;
            Vector3D tdir = _p1 + (tx * Constants.TileSize) * _dX + (ty * Constants.TileSize) * _dY;

            while (true) {
                Vector3D ldir = tdir;
                for (int y = 0; y < Constants.TileSize; y++) {
                    Vector3D pdir = ldir;
                    for (int x = 0; x < Constants.TileSize; x++) {
                        Color3D acc = new Color3D(0, 0, 0);
                        RenderRay(pdir, ref acc);
                        int red = (int) (acc.R*256);
                        int green = (int) (acc.G*256);
                        int blue = (int) (acc.B*256);
                        if (red > 255) red = 255;
                        if (green > 255) green = 255;
                        if (blue > 255) blue = 255;

                        int pX = tx*Constants.TileSize + x;
                        int pY = ty*Constants.TileSize + y;
                        _frameBuffer.SetPixel(pX, pY, Color.FromArgb(red, green, blue));
                        pdir += _dX;
                    }
                    ldir += _dY;
                }
                tdir += _dX*Constants.TileSize;
                if (++tx == _xTiles) {
                    tx = 0;
                    ty++;
                    tdir = _p1 + (ty*Constants.TileSize)*_dY;
                }

                if (ty < _yTiles) {
                    //See if we've been drawing for a while now
                    if (DateTime.Now.Ticks - ticks > 200) {
                        //Return temporary to windows to let it redraw itself
                        _currCol = tx;
                        _currRow = ty;
                        return false; //possibly still work to do
                    }
                }
                else
                    break;
            }

            //done
            return true;
        }
    }
}
