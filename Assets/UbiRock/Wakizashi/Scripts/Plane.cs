using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Plane {
        private Vector3 _position;
        private Vector3 _normal;

        public Plane(Vector3 position, Vector3 normal) {
            _position = position;
            _normal = normal;
        }

        public Plane(Vector3 a, Vector3 b, Vector3 c) {
            Normal = Vector3.Cross(b - a, c - a);
            _position = a;
        }

        public Vector3 Normal { get => _normal; private set => _normal = Vector3.Normalize(value); }

        public float Distance { get => Vector3.Dot(_position, _normal); }
        public Vector3 Position { get => _position; }

        public PointToPlaneRelation GetPointToPlaneRelation(Vector3 p) {
            float planeDistance = Vector3.Dot(_position, _normal);
            float pointDistance = Vector3.Dot(p, _normal);

            float difference = pointDistance - planeDistance;

            if (difference > Constants.EPSILON) return PointToPlaneRelation.TOP;
            else if (difference < -Constants.EPSILON) return PointToPlaneRelation.BOTTOM;
            else return PointToPlaneRelation.SURFACE;
        }

        public (PointToPlaneRelation, PointToPlaneRelation, PointToPlaneRelation)  GetPointToPlaneRelations(Tri tri) {
            var (a, b, c) = tri.GetPositions();
            return (GetPointToPlaneRelation(a), GetPointToPlaneRelation(b), GetPointToPlaneRelation(c));
        }

        public TrianglePlaneRelation GetTriangleToPlaneRelation(Tri tri) {
            PointToPlaneRelation[] relations = new PointToPlaneRelation[3];

            var (a, b, c) = tri.GetPositions();
            var (ra, rb, rc) = (GetPointToPlaneRelation(a), GetPointToPlaneRelation(b), GetPointToPlaneRelation(c));
            
            switch (Mathf.Abs((int)ra + (int)rb + (int)rc)) {
                case 3: return TrianglePlaneRelation.NO_INTERSECTION;
                case 2: return TrianglePlaneRelation.NO_INTERSECTION;
                case 0: return TrianglePlaneRelation.TWO_TRI;
                case 1:
                    if (ra == PointToPlaneRelation.SURFACE || rb == PointToPlaneRelation.SURFACE || rc == PointToPlaneRelation.SURFACE)
                        return TrianglePlaneRelation.NO_INTERSECTION;
                    return TrianglePlaneRelation.THREE_TRI;
                default:
                    throw new System.NotSupportedException("UbiRock.Wakizashi:: This triangle relation to the plane can not be calculated.");
            }
        }
    }
}
