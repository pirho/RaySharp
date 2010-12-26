
namespace Raytracing {
    public sealed class Material {
        public Material() {
            Color = new Color3D(0.2f, 0.2f, 0.2f);
            Reflection = 0f;
            Diffuse = 0.2f;
            DiffuseReflection = 0f;
            Specular = 0.8f;
            RefractionIndex = 1.5f;

            Texture = null;
            SetUVScale(1, 1);
        }

        public Color3D Color { get; set; }
        public double Diffuse { get; set; }
        public double DiffuseReflection { get; set; }
        public double Reflection { get; set; }
        public double Refraction { get; set; }
        public double RefractionIndex { get; set; }
        public double Specular { get; set; }
        public Texture Texture { get; set; }
        
        public double UScale { get; private set; }
        public double VScale { get; private set; }
        public double UScaleReci { get; private set; }
        public double VScaleReci { get; private set; }

        public void SetUVScale(double uScale, double vScale) {
            UScale = uScale;
            VScale = vScale;
            UScaleReci = 1f / uScale;
            VScaleReci = 1f / vScale;
        }

        public void SetParameters(double refl, double refr, Color3D col, double diff, double spec) {
            Reflection = refl;
            Refraction = refr;
            Color = col;
            Diffuse = diff;
            Specular = spec;
        }
    }       
}
