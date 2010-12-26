using Raytracing.Objects;

namespace Raytracing {
    public class KdTreeNode {
        public KdTreeNode() {
            Left = null;
            Right = null;
            SplitPosition = double.MinValue;
            Axis = 2; //Default to the Z-axis
        }

        public double SplitPosition { get; set; }
        public int Axis { get; set; }

        public KdTreeNode Left { get; set; }
        public KdTreeNode Right { get; set; }

        public bool IsLeaf {
            get {
                return Left != null && Right != null;
            }
        }

        public ObjectList List { get; set; }

        public void Add(Primitive p) {
            if (List == null || List.Primitive == null) {
                List = new ObjectList(p, null);
                return;
            }

            ObjectList tmp = List;
            while (tmp.Next != null)
                tmp = tmp.Next;

            tmp.Next = new ObjectList(p, null);
        }
    }
}
