using System.Collections.Generic;
using System.IO;
using Raytracing.Objects;
using Raytracing.Primitives;

namespace Raytracing {
    public class Scene {

        public Scene() {
            Primitives = new List<Primitive>();
            Lights = new List<Light>();
            Extends = new AxisAlignedBox();
            
            _state = 0;
        }
        // state variable to split the init in bits
        private int _state;

        public List<Primitive> Primitives { get; private set; }
        public List<Light> Lights { get; private set; }
        public AxisAlignedBox Extends { get; private set; }
        public KdTree KdTree { get; private set; }

        public bool InitScene() {
            Material mat;
            int x;
            Vector3D p1, p2;
            switch (_state) {
                case 0:
                    //constructing geometry...
                    break;
                case 1:
                    // ground plane
                    mat = new Material();
                    //Changed to water
                    mat.SetParameters(0.3, 0.7, new Color3D(0, 128, 255), 0.2, 0.1);
                    mat.RefractionIndex = 1.33157;
                    mat.DiffuseReflection = .1;
                    //mat.SetParameters(0.0f, 0.0f, new Color3D(0.4f, 0.3f, 0.3f), 1.0f, 0.0f);
                    AddPlane(new Vector3D(-13, -4.4f, -5.5f), new Vector3D(13, -4.4f, -5.5f),
                              new Vector3D(13, -4.4f, 29), new Vector3D(-13, -4.4f, 29), mat);
                    // back plane
                    mat = new Material();
                    mat.SetParameters(0.0, 0.0, new Color3D(0.5, 0.3, 0.5), 0.6, 0.0);
                    AddPlane(new Vector3D(-13, -4.4f, 12), new Vector3D(13, -4.4f, 12),
                              new Vector3D(13, 7.4f, 12), new Vector3D(-13, 7.4f, 12), mat);

                    //AddPlane(new Vector3D(-13, -4.4f, 8), new Vector3D(13, -4.4f, 16),
                    //          new Vector3D(13, 7.4f, 16), new Vector3D(-13, 7.4f, 8), mat);

                    // ceiling plane
                    mat = new Material();
                    mat.SetParameters(0.0, 0.0, new Color3D(0.4, 0.7, 0.7), 0.5, 0.0);
                    AddPlane(new Vector3D(13, 7.4f, -5.5f), new Vector3D(-13, 7.4f, -5.5f),
                              new Vector3D(-13, 7.4f, 29), new Vector3D(13, 7.4f, 29), mat);
                    
                    // big sphere
		            // m_Primitive[m_Primitives] = new Primitive( Primitive::SPHERE, vector3( 2, 0.8f, 3 ), 2.5f );
		            // m_Primitive[m_Primitives++]->GetMaterial()->SetParameters( 0.2f, 0.8f, Color( 0.7f, 0.7f, 1.0f ), 0.2f, 0.8f );
		            // small sphere
                    Primitive p = new Primitive(PrimitiveType.Sphere, new Vector3D(-5.5f, -0.5f, 7), 2);
                    p.Material.SetParameters(0.5f, 0.0f, new Color3D(.7f, .7f, 1), .1f, .8f);
		            Primitives.Add(p);
	
                    Light l;
                    /*
                    // area lights
                    l = new Light(Light.LightType.Area, new Vector3D(-1, 6, 4), new Vector3D(1, 6, 4), new Vector3D(-1, 6, 6), new Color3D(0.7f, 0.7f, 0.7f));
                    Lights.Add(l);
                    l = new Light(Light.LightType.Area, new Vector3D(-1, 6, -1), new Vector3D(1, 6, -1), new Vector3D(-1, 6, 1), new Color3D(0.7f, 0.7f, 0.7f));
                    Lights.Add(l);
		            */
                    
                    // point lights
                    l = new Light(Light.LightType.Point, new Vector3D(0, 5, 5), new Color3D(0.4f, 0.4f, 0.4f));
                    Lights.Add(l);
                    l = new Light(Light.LightType.Point, new Vector3D(-3, 5, 1), new Color3D(0.6f, 0.6f, 0.8f));
                    Lights.Add(l);
	                

		            // extra sphere
                    p = new Primitive(PrimitiveType.Sphere, new Vector3D(-1.5f, -3.8f, 1), 1.5f);
                    p.Material.SetParameters(0.0f, 0.8f, new Color3D(1.0f, 0.4f, 0.4f), 0.2f, 0.8f);
                    Primitives.Add(p);

		            // grid
		            for ( x = 0; x < 8; x++ ) { 
                        for ( int y = 0; y < 7; y++ ) {
                            p = new Primitive(PrimitiveType.Sphere, new Vector3D(-4.5f + x*1.5f, -4.3f + y*1.5f, 10), 0.3f);
                            p.Material.SetParameters(0.0f, 0.0f, new Color3D(0.3f, 1.0f, 0.4f), 0.6f, 0.6f);
                            Primitives.Add(p);
		                }
                    }
		            for ( x = 0; x < 8; x++ ) {
                        for ( int y = 0; y < 8; y++ ) {
                            p = new Primitive(PrimitiveType.Sphere, new Vector3D(-4.5f + x*1.5f, -4.3f, 10.0f - y*1.5f), 0.3f);
                            p.Material.SetParameters(0.0f, 0.0f, new Color3D(0.3f, 1.0f, 0.4f), 0.6f, 0.6f);
                            Primitives.Add(p);
		                }
                    }
		            for ( x = 0; x < 16; x++ ) {
                        for ( int y = 0; y < 8; y++ ) {
                            p = new Primitive(PrimitiveType.Sphere, new Vector3D(-8.5f + x*1.5f, 4.3f, 10.0f - y), 0.3f);
                            p.Material.SetParameters(0.0f, 0.0f, new Color3D(0.3f, 1.0f, 0.4f), 0.6f, 0.6f);
                            Primitives.Add(p);
		                }
                    }
		            mat = new Material();
		            mat.SetParameters( 0.9f, 0, new Color3D( 0.9f, 0.9f, 1 ), 0.3f, 0.7f );
                    mat.RefractionIndex = 1.3f;
		            // mat->SetTexture( new Texture( "textures/wood.tga" ) );
                    //Load3DS("meshes\\knot.3ds", mat, new Vector3D(0, 0.5f, 4), 6);
		            break;
                case 2:
                    //building kdtree, please wait
                    break;
                case 3:
                    //Build the KD-tree
                    p1 = new Vector3D(-14, -6, -6);
                    p2 = new Vector3D(14, 8, 30);
		            Extends = new AxisAlignedBox(p1, p2 - p1);
		            KdTree = new KdTree();
		            KdTree.Build(this);
                    break;
                default:
                    return true; //done initializing
            }

            _state++;
            return false; //still initializing
        }

        private void AddBox(Vector3D position, Vector3D size) {
            Vertex[] v = new Vertex[8];
            v[0] = new Vertex(new Vector3D(position.X, position.Y, position.Z), 0, 0);
            v[1] = new Vertex(new Vector3D(position.X + size.X, position.Y, position.Y), 0, 0);
            v[2] = new Vertex(new Vector3D(position.X + size.X, position.Y + size.Y, position.Z), 0, 0);
            v[3] = new Vertex(new Vector3D(position.X, position.Y + size.Y, position.Z), 0, 0);
            v[4] = new Vertex(new Vector3D(position.X, position.Y, position.Z + size.Z), 0, 0);
            v[5] = new Vertex(new Vector3D(position.X + size.X, position.Y, position.Z + size.Z), 0, 0);
            v[6] = new Vertex(new Vector3D(position.X + size.X, position.Y + size.Y, position.Z + size.Z), 0, 0);
            v[7] = new Vertex(new Vector3D(position.X, position.Y + size.Y, position.Z + size.Z), 0, 0);
            // front plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[0], v[1], v[3]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[1], v[2], v[3]));
            // back plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[5], v[4], v[7]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[5], v[7], v[6]));
            // left plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[4], v[0], v[3]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[4], v[3], v[7]));
            // right plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[1], v[5], v[2]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[5], v[6], v[2]));
            // top plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[4], v[5], v[1]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[4], v[1], v[0]));
            // bottom plane
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[6], v[7], v[2]));
            Primitives.Add(new Primitive(PrimitiveType.Vertex, v[7], v[3], v[2]));
        }

        private void AddPlane(Vector3D p1, Vector3D p2, Vector3D p3, Vector3D p4, Material mat) {
            Vertex[] v = new Vertex[4];
            v[0] = new Vertex(p1, 0, 0);
            v[1] = new Vertex(p2, 0, 0);
            v[2] = new Vertex(p3, 0, 0);
            v[3] = new Vertex(p4, 0, 0);
            Primitive p = new Primitive(PrimitiveType.Vertex, v[0], v[1], v[3]);
            p.Material = mat;
            Primitives.Add(p);
            p = new Primitive(PrimitiveType.Vertex, v[1], v[2], v[3]);
            p.Material = mat;
            Primitives.Add(p);
        }
        /*
        class ThreeDsLoader {
            private double[] _vertices;
            private double[] _tCoords;
            private int _vertexCount;
            private int _faceCount;
            private ushort[] _faces;

            public void Load3DS(string file, Material mat, Vector3D pos, double size) {
                byte[] buffer;
                int bytesRead;

                // load 3ds file to memory
                using (Stream s = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    buffer = new byte[800 * 1024];
                    bytesRead = s.Read(buffer, 0, 800 * 1024);
                }

                if (bytesRead <= 0)
                    return;

                //// initialize chunk parser
                //_vertices = null;
                //_tCoords = null;
                //_faces = null;

                // process chunks
                EatChunk(buffer);

                //determine extends
                Vector3D min = new Vector3D(_vertices[0], _vertices[1], _vertices[2]);
                Vector3D max = new Vector3D(_vertices[0], _vertices[1], _vertices[2]);
                for (int i = 1; i < _vertexCount; i++) {
                    if (_vertices[i * 3] < min.X) min.X = _vertices[i * 3];
                    if (_vertices[i * 3] > max.X) max.X = _vertices[i * 3];
                    if (_vertices[i * 3 + 1] < min.Y) min.Y = _vertices[i * 3 + 1];
                    if (_vertices[i * 3 + 1] > max.Y) max.Y = _vertices[i * 3 + 1];
                    if (_vertices[i * 3 + 2] < min.Z) min.Z = _vertices[i * 3 + 2];
                    if (_vertices[i * 3 + 2] > max.Z) max.Z = _vertices[i * 3 + 2];
                }

                Vector3D vSize = max - min;
                // determine scale based on largest extend
	            double scale;
	            if ((vSize.X > vSize.Y) && (vSize.X > vSize.Z))
                    scale = size / vSize.X;
	            else if (vSize.Y > vSize.Z)
                    scale = size/vSize.Y;
	            else
                    scale = size / vSize.Z;

	            // determine offset
	            Vector3D center = (min + max) * 0.5f;
	            Vector3D offset = (center * scale) - pos;
	            // create vertices
	            Vertex[] vert = new Vertex[_vertexCount];
	            Primitive[][] vertface = new Primitive[_vertexCount][];
	            int[] vertfaces = new int[_vertexCount];

	            for (int i = 0; i < _vertexCount; i++ ) {
		            double x = _vertices[i * 3], z = _vertices[i * 3 + 1], y = _vertices[i * 3 + 2];
	                vert[i] = new Vertex(new Vector3D((x*scale) - offset.X, (y*scale) - offset.Y, (z*scale) - offset.Z), 0, 0);
	                vert[i].SetUV(vert[i].Position[0]/size, vert[i].Position[1]/size);
                    vertface[i] = new Primitive[10];
		            vertfaces[i] = 0;
	            }

	            // convert to ray tracer primitives
	            for ( int i = 0; i < _faceCount; i++ ) {
		            int[] idx = new int[3];
		            for (int v = 0; v < 3; v++)
                        idx[v] = _faces[i * 3 + v];

	                Primitive p = new Primitive(PrimitiveType.Vertex, vert[idx[0]], vert[idx[1]], vert[idx[2]]);
	                p.Material = mat;
                    for (int v = 0; v < 1; v++)
                        if (vertfaces[idx[v]] < 10)
                            vertface[idx[v]][vertfaces[idx[v]]++] = p;
	            }

	            // calculate vertex normals
                for (int i = 0; i < _vertexCount; i++) {
                    Vector3D vN = new Vector3D(0, 0, 0);
                    for (int v = 0; v < vertfaces[i]; v++)
                        vN += vertface[i][v].GetNormal(pos);

                    vN *= 1.0f/vertfaces[i];
                    vert[i].Normal = vN;
                }
            }
        }

        */
    }
}
