using System.Linq;
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

            TrianglePlaneRelation triangleStatus = plane.TrianglePlaneRelation(tri);

            if (triangleStatus == TrianglePlaneRelation.NO_INTERSECTION) return;
            if (triangleStatus == TrianglePlaneRelation.TWO_TRI) {
                if (a == PointToPlaneRelation.SURFACE) {
                    // TODO: Add this to the final return
                    Intersect(plane, b, c);
                    return;
                } else if (b == PointToPlaneRelation.SURFACE) {
                    // TODO: Add this to the final return
                    Intersect(place, a, c);
                    return;
                } else {
                    // TODO: Add this to the final return
                    Intersect(plane, a, b);
                    return;
                }
            }
            if (triangleStatus == TrianglePlaneRelation.THREE_TRI) {
                if (pointRelations[0] != pointRelations[1]) {
                    // TODO: Add this to the final return
                    Intersect(plane, a, b);

                    if (pointRelations[2] == pointRelations[0]) {
                        // TODO: Add this to the final return
                        Intersect(plane, b, c);
                        return;
                    }
                    else {
                        // TODO: Add this to the final return
                        Intersect(plane, a, c);
                        return;
                    }
                }
                else {
                    // TODO: Add these to the final return
                    Intersect(plane, a, c);
                    Intersect(plane, b, c);
                    return;
                }
            }
        }
    }
}