using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

using Raytracing.Objects;
using Raytracing.Primitives;

namespace Raytracing {
    public class KdTree {
        class SplitListItem {
            public double SplitPos { get; set; }
            public int N1Count { get; set; }
            public int N2Count { get; set; }
        }

        class SplitList : SplitListItem {
            public SplitList Next { get; set; }
        }

        private SortedDictionary<double, SplitListItem> _sortedList;
        private SplitList _list;

        public KdTree() {
            Root = new KdTreeNode();
        }

        public KdTreeNode Root { get; set; }

        public void Build(Scene scene) {
            foreach (Primitive p in scene.Primitives) {
                Root.Add(p);
            }

            int prims = scene.Primitives.Count;
            AxisAlignedBox sBox = scene.Extends;
            _list = null;
            _sortedList = null;
            Subdivide(Root, sBox, 0, prims);
        }

        //public void InsertSplitPos(double splitPos) {
        //    // insert a split position candidate in the list if it is unique
        //    SplitList entry = new SplitList();
        //    entry.Next = null;
        //    entry.SplitPos = splitPos;
        //    entry.N1Count = 0;
        //    entry.N2Count = 0;

        //    if (_list == null)
        //        _list = entry;
        //    else {
        //        if (splitPos < _list.SplitPos) {
        //            entry.Next = _list;
        //            _list = entry;
        //        }
        //        else if (splitPos == _list.SplitPos) {
        //            //Do nothing, this item will not be added.
        //        }
        //        else {
        //            SplitList list = _list;
        //            while ((list.Next != null) && (splitPos >= list.Next.SplitPos)) {
        //                if (splitPos == list.Next.SplitPos) {
        //                    return; //Do not add this item if the split position is the same
        //                }
        //                list = list.Next;
        //            }
        //            entry.Next = list.Next;
        //            list.Next = entry;
        //        }
        //    }
        //}

        public void InsertSplitPos(double splitPos) {
            // insert a split position candidate in the list if it is unique
            _sortedList = new SortedDictionary<double, SplitListItem>();
            SplitListItem entry = new SplitListItem();
            entry.SplitPos = splitPos;
            entry.N1Count = 0;
            entry.N2Count = 0;

            if (_sortedList.Count == 0) {
                _sortedList.Add(splitPos, entry);
                return;
            }

            if (_sortedList.ContainsKey(splitPos))
                return; //Item already in list, return
            
            _sortedList.Add(splitPos, entry); //adds the item in the correct position
        }

        //public void Subdivide(KdTreeNode node, AxisAlignedBox box, int depth, int prims) {
        //    // recycle used split list nodes
        //    _list = null;

        //    // determine split axis
        //    Vector3D s = box.Size;
        //    if ((s.X >= s.Y) && (s.X >= s.Z))
        //        node.Axis = 0;
        //    else if ((s.Y >= s.X) && (s.Y >= s.Z))
        //        node.Axis = 1;

        //    int axis = node.Axis;
        //    // make a list of the split position candidates
        //    ObjectList l = node.List;
        //    double p1, p2;
        //    double pos1 = box.Position[axis];
        //    double pos2 = box.Position[axis] + box.Size[axis];
            
        //    bool[] pright = new bool[prims];
        //    double[] eleft = new double[prims];
        //    double[] eright = new double[prims];
        //    Primitive[] parray = new Primitive[prims];

        //    int aidx = 0;
        //    while (l != null) {
        //        parray[aidx] = l.Primitive;
        //        Primitive p = parray[aidx];
        //        pright[aidx] = true;
        //        p.CalculateRange(ref eleft[aidx], ref eright[aidx], axis);
        //        aidx++;
		        
        //        if (p.Type == PrimitiveType.Sphere) {
        //            p1 = p.Center[axis] - p.Radius;
        //            p2 = p.Center[axis] + p.Radius;
        //            if ((p1 >= pos1) && (p1 <= pos2)) InsertSplitPos( p1 );
        //            if ((p2 >= pos1) && (p2 <= pos2)) InsertSplitPos( p2 );
        //        }
        //        else {
        //            for (int i = 0; i < 3; i++ ) {
        //                p1 = p.GetVertex(i).Position[axis];
        //                if ((p1 >= pos1) && (p1 <= pos2)) InsertSplitPos( p1 );
        //            }
        //        }
        //        l = l.Next;
        //    }
        //    // determine n1count / n2count for each split position
        //    AxisAlignedBox b1 = null, b2 = null, b3, b4;
        //    b3 = box;
        //    b4 = box;
        //    SplitList splist = _list;
        //    double b3p1 = b3.Position[axis];
        //    double b4p2 = b4.Position[axis] + b4.Size[axis];
        //    while (splist != null) {
        //        b4.Position[axis] = splist.SplitPos;
        //        b4.Size[axis] = pos2 - splist.SplitPos;
        //        b3.Size[axis] = splist.SplitPos - pos1;
        //        double b3p2 = b3.Position[axis] + b3.Size[axis];
        //        double b4p1 = b4.Position[axis];
        //        for (int i = 0; i < prims; i++) {
        //            if (pright[i]) {
        //                Primitive p = parray[i];
        //                if ((eleft[i] <= b3p2) && (eright[i] >= b3p1))
        //                    if (p.IntersectBox(b3))
        //                        splist.N1Count++;
	                    
        //                if ((eleft[i] <= b4p2) && (eright[i] >= b4p1))
        //                    if (p.IntersectBox(b4))
        //                        splist.N2Count++;
        //                    else
        //                        pright[i] = false;
        //            }
        //            else
        //                splist.N1Count++;
        //        }
                
        //        splist = splist.Next;
        //    }

        //    // calculate surface area for current node
        //    double SAV = 0.5f / (box.W * box.D + box.W * box.H + box.D * box.H);
        //    // calculate cost for not splitting
        //    double Cleaf = prims * 1.0f;
        //    // determine optimal split plane position
        //    splist = _list;
        //    double lowcost = 10000;
        //    double bestpos = 0;
        //    while (splist != null) {
        //        // calculate child node extends
        //        b4.Position[axis] = splist.SplitPos;
        //        b4.Size[axis] = pos2 - splist.SplitPos;
        //        b3.Size[axis] = splist.SplitPos - pos1;
        //        // calculate child node cost
        //        double SA1 = 2 * (b3.W * b3.D + b3.W * b3.H + b3.D * b3.H);
        //        double SA2 = 2 * (b4.W * b4.D + b4.W * b4.H + b4.D * b4.H);
        //        double splitcost = 0.3f + 1.0f * (SA1 * SAV * splist.N1Count + SA2 * SAV * splist.N2Count);
        //        // update best cost tracking variables
        //        if (splitcost < lowcost) {
        //            lowcost = splitcost;
        //            bestpos = splist.SplitPos;
        //            b1 = b3;
        //            b2 = b4;
        //        }
        //        splist = splist.Next;
        //    }
        //    if (lowcost > Cleaf)
        //        return;

        //    node.SplitPosition = bestpos;
        //    // construct child nodes
        //    KdTreeNode tmpLeft = new KdTreeNode();
        //    KdTreeNode tmpRight = new KdTreeNode();
        //    int n1count = 0, n2count = 0, total = 0;
        //    // assign primitives to both sides
        //    double b1p1 = b1.Position[axis];
        //    double b2p2 = b2.Position[axis] + b2.Size[axis];
        //    double b1p2 = b1.Position[axis] + b1.Size[axis];
        //    double b2p1 = b2.Position[axis];
        //    for ( int i = 0; i < prims; i++ ) {
        //        Primitive p = parray[i];
        //        total++;
        //        if (eleft[i] <= b1p2 && eright[i] >= b1p1) {
        //            if (p.IntersectBox(b1)) {
        //                tmpLeft.Add(p);
        //                n1count++;
        //            }
        //        }
        //        if (eleft[i] <= b2p2 && eright[i] >= b2p1) {
        //            if (p.IntersectBox(b2)) {
        //                tmpRight.Add(p);
        //                n2count++;
        //            }
        //        }
        //    }

        //    node.List = null;
        //    if (n1count > 0) node.Left = tmpLeft;
        //    if (n2count > 0) node.Right = tmpRight;

        //    if (depth < Constants.MaxTreeDepth) {
        //        if (n1count > 2) Subdivide(tmpLeft, b1, depth + 1, n1count);
        //        if (n2count > 2) Subdivide(tmpRight, b2, depth + 1, n2count);
        //    }
        //}

        public void Subdivide(KdTreeNode node, AxisAlignedBox box, int depth, int prims) {
            // recycle used split list nodes
            _sortedList = null;

            // determine split axis
            Vector3D s = box.Size;
            if ((s.X >= s.Y) && (s.X >= s.Z))
                node.Axis = 0;
            else if ((s.Y >= s.X) && (s.Y >= s.Z))
                node.Axis = 1;

            int axis = node.Axis;
            // make a list of the split position candidates
            ObjectList l = node.List;
            double p1, p2;
            double pos1 = box.Position[axis];
            double pos2 = box.Position[axis] + box.Size[axis];

            bool[] pright = new bool[prims];
            double[] eleft = new double[prims];
            double[] eright = new double[prims];
            Primitive[] parray = new Primitive[prims];

            int aidx = 0;
            while (l != null) {
                parray[aidx] = l.Primitive;
                Primitive p = parray[aidx];
                pright[aidx] = true;
                p.CalculateRange(ref eleft[aidx], ref eright[aidx], axis);
                aidx++;

                if (p.Type == PrimitiveType.Sphere) {
                    p1 = p.Center[axis] - p.Radius;
                    p2 = p.Center[axis] + p.Radius;
                    if ((p1 >= pos1) && (p1 <= pos2)) InsertSplitPos(p1);
                    if ((p2 >= pos1) && (p2 <= pos2)) InsertSplitPos(p2);
                }
                else {
                    for (int i = 0; i < 3; i++) {
                        p1 = p.GetVertex(i).Position[axis];
                        if ((p1 >= pos1) && (p1 <= pos2)) InsertSplitPos(p1);
                    }
                }
                l = l.Next;
            }
            // determine n1count / n2count for each split position
            AxisAlignedBox b1 = null, b2 = null, b3, b4;
            b3 = box;
            b4 = box;
            
            double b3p1 = b3.Position[axis];
            double b4p2 = b4.Position[axis] + b4.Size[axis];
            
            if (_sortedList == null)
                return;
            
            foreach (KeyValuePair<double, SplitListItem> pair in _sortedList) {
                SplitListItem spListItem = pair.Value;

                b4.Position[axis] = spListItem.SplitPos;
                b4.Size[axis] = pos2 - spListItem.SplitPos;
                b3.Size[axis] = spListItem.SplitPos - pos1;
                double b3p2 = b3.Position[axis] + b3.Size[axis];
                double b4p1 = b4.Position[axis];
                for (int i = 0; i < prims; i++) {
                    if (pright[i]) {
                        Primitive p = parray[i];
                        if ((eleft[i] <= b3p2) && (eright[i] >= b3p1))
                            if (p.IntersectBox(b3))
                                spListItem.N1Count++;

                        if ((eleft[i] <= b4p2) && (eright[i] >= b4p1))
                            if (p.IntersectBox(b4))
                                spListItem.N2Count++;
                            else
                                pright[i] = false;
                    }
                    else
                        spListItem.N1Count++;
                }
            }

            // calculate surface area for current node
            double SAV = 0.5f / (box.W * box.D + box.W * box.H + box.D * box.H);
            // calculate cost for not splitting
            double Cleaf = prims * 1.0f;

            // determine optimal split plane position
            double lowcost = 10000;
            double bestpos = 0;

            foreach (KeyValuePair<double, SplitListItem> pair in _sortedList) {
                SplitListItem spListItem = pair.Value;

                // calculate child node extends
                b4.Position[axis] = spListItem.SplitPos;
                b4.Size[axis] = pos2 - spListItem.SplitPos;
                b3.Size[axis] = spListItem.SplitPos - pos1;
                // calculate child node cost
                double SA1 = 2 * (b3.W * b3.D + b3.W * b3.H + b3.D * b3.H);
                double SA2 = 2 * (b4.W * b4.D + b4.W * b4.H + b4.D * b4.H);
                double splitcost = 0.3f + 1.0f * (SA1 * SAV * spListItem.N1Count + SA2 * SAV * spListItem.N2Count);
                // update best cost tracking variables
                if (splitcost < lowcost) {
                    lowcost = splitcost;
                    bestpos = spListItem.SplitPos;
                    b1 = b3;
                    b2 = b4;
                }
            }

            if (lowcost > Cleaf)
                return;

            node.SplitPosition = bestpos;
            // construct child nodes
            KdTreeNode tmpLeft = new KdTreeNode();
            KdTreeNode tmpRight = new KdTreeNode();
            int n1count = 0, n2count = 0, total = 0;
            // assign primitives to both sides
            double b1p1 = b1.Position[axis];
            double b2p2 = b2.Position[axis] + b2.Size[axis];
            double b1p2 = b1.Position[axis] + b1.Size[axis];
            double b2p1 = b2.Position[axis];
            for (int i = 0; i < prims; i++) {
                Primitive p = parray[i];
                total++;
                if (eleft[i] <= b1p2 && eright[i] >= b1p1) {
                    if (p.IntersectBox(b1)) {
                        tmpLeft.Add(p);
                        n1count++;
                    }
                }
                if (eleft[i] <= b2p2 && eright[i] >= b2p1) {
                    if (p.IntersectBox(b2)) {
                        tmpRight.Add(p);
                        n2count++;
                    }
                }
            }

            node.List = null;
            if (n1count > 0) node.Left = tmpLeft;
            if (n2count > 0) node.Right = tmpRight;

            if (depth < Constants.MaxTreeDepth) {
                if (n1count > 2) Subdivide(tmpLeft, b1, depth + 1, n1count);
                if (n2count > 2) Subdivide(tmpRight, b2, depth + 1, n2count);
            }
        }
    }
}
