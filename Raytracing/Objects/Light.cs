using System;

namespace Raytracing.Objects {
    public class Light {
        public enum LightType {
            Point = 1,
            Area = 2
        }

        private readonly Vector3D[] _grid;

        public Light(LightType type, Vector3D position, Color3D color) {
            Type = type;
            Position = position;
            Color = color;
        }

        public Light(LightType type, Vector3D p1, Vector3D p2, Vector3D p3, Color3D color) {
            Type = type;
            Color = color;

            _grid = new Vector3D[16];
            _grid[0] = new Vector3D(1, 2, 0);
            _grid[1] = new Vector3D(3, 3, 0);
            _grid[2] = new Vector3D(2, 0, 0);
            _grid[3] = new Vector3D(0, 1, 0);
            _grid[4] = new Vector3D(2, 3, 0);
            _grid[5] = new Vector3D(0, 3, 0);
            _grid[6] = new Vector3D(0, 0, 0);
            _grid[7] = new Vector3D(2, 2, 0);
            _grid[8] = new Vector3D(3, 1, 0);
            _grid[9] = new Vector3D(1, 3, 0);
            _grid[10] = new Vector3D(1, 0, 0);
            _grid[11] = new Vector3D(3, 2, 0);
            _grid[12] = new Vector3D(2, 1, 0);
            _grid[13] = new Vector3D(3, 0, 0);
            _grid[14] = new Vector3D(1, 1, 0);
            _grid[15] = new Vector3D(0, 2, 0);

            CellX = (p2 - p1) * .25f;
            CellY = (p3 - p1) * .25f;

            for (int i = 0; i < 16; i++)
                _grid[i] = _grid[i][0] * CellX + _grid[i][1] * CellY + p1;

            Position = p1 + 2 * CellX + 2 * CellY;
        }
        
        public LightType Type { get; private set; }
        public Vector3D Position { get; private set; }
        public Vector3D CellX { get; private set; }
        public Vector3D CellY { get; private set; }
        public Color3D Color { get; private set; }

        public Vector3D GetGrid(int index) {
            if (Type == LightType.Point)
                throw new NotSupportedException();

            if (index < 0 || index > _grid.Length)
                throw new ArgumentOutOfRangeException();

            return _grid[index];
        }
    }
}
