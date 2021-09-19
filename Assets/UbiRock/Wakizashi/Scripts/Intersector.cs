using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Intersector {
        public static (Vector3, bool) Intersect(Plane p, Line l) => Intersect(p, l.PointA, l.PointB);

        public static (Vector3, bool) Intersect(Plane p, Vector3 a, Vector3 b) {
            Vector3 pNormal = p.Normal;
            float pDistance = p.Distance;
            Vector3 line = b - a;

            float t = (pDistance - Vector3.Dot(pNormal, a)) / Vector3.Dot(pNormal, line);
        }
    }
}