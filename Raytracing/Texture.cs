using System;
using System.IO;

namespace Raytracing {
    public class Texture {
        public Texture(string file) {
            byte[] buffer;
            using (Stream s = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                buffer = new byte[2];
                s.Seek(12, SeekOrigin.Begin);
                int read = s.Read(buffer, 0, 2);
                if (read == 2)
                    Width = buffer[0] + 256*buffer[1];

                read = s.Read(buffer, 0, 2);
                if (read == 2)
                    Height = buffer[0] + 256 * buffer[1];

                buffer = new byte[Width * Height * 3 + 1024];
                s.Seek(0, SeekOrigin.Begin);
                read = s.Read(buffer, 0, Width*Height*3 + 1024);
            }

            Bitmap = new Color3D[Width*Height];
            double rec = 1f/256;
            int size = Width*Height;
            for (int i = 0; i < size; i++) {
                Bitmap[i] = new Color3D(buffer[i * 3 + 20] * rec, buffer[i * 3 + 19] * rec, buffer[i * 3 + 18] * rec);
            }
        }

        public Texture(Color3D[] bitmap, int width, int height) {
            Bitmap = bitmap;
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Color3D[] Bitmap { get; private set; }

        public Color3D GetTexel(double u, double v) {
            // fetch a bilinearly filtered texel
            double fu = (u + 1000.5f) * Width;
            double fv = (v + 1000.0f) * Width;
            int u1 = ((int)fu) % Width;
            int v1 = ((int)fv) % Height;
            int u2 = (u1 + 1) % Width;
            int v2 = (v1 + 1) % Height;
            // calculate fractional parts of u and v
            double fracu = fu - (double)Math.Floor(fu);
            double fracv = fv - (double)Math.Floor(fv);
            // calculate weight factors
            double w1 = (1 - fracu) * (1 - fracv);
            double w2 = fracu * (1 - fracv);
            double w3 = (1 - fracu) * fracv;
            double w4 = fracu * fracv;
            // fetch four texels
            Color3D c1 = Bitmap[u1 + v1 * Width];
            Color3D c2 = Bitmap[u2 + v1 * Width];
            Color3D c3 = Bitmap[u1 + v2 * Width];
            Color3D c4 = Bitmap[u2 + v2 * Width];
            // scale and sum the four colors
            return c1 * w1 + c2 * w2 + c3 * w3 + c4 * w4;
        }
    }
}
