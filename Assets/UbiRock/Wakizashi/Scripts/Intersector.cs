using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Intersector {
        public static (Vector3, bool) Intersect(Plane plane, Line line) => Intersect(p, l.PointA, l.PointB);

        public static (Vector3, bool) Intersect(Plane plane, Vector3 a, Vector3 b) {
            Vector3 pNormal = p.Normal;
            float pDistance = p.Distance;
            Vector3 line = b - a;

            float t = (pDistance - Vector3.Dot(pNormal, a)) / Vector3.Dot(pNormal, line);
        }

        public static void Intersect(Plane plane, Tri tri) {
            var (a, b, c) = tri.GetPositions();
            bool[] pointRelations = plane.GetTriangleToPlaneRelation(tri);

            // 1 : All the points are in one side
            // 1.1 : One is on surface and two on same side
            // 1.2 : Two on surface

            // 2 : One is one surface and two on different sides

            // 3 : two points are in different sides
        }
    }
}