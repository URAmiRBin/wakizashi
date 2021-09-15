using UnityEngine;

namespace UbiRock.Toolkit {
    public class Line {
        private Vector3 _pointA, _pointB;

        public Line(Vector3 a, Vector3 b) {
            _pointA = a;
            _pointB = b;
        }

        public Vector3 PointA { get => _pointA; }
        public Vector3 PointB { get => _pointB; }
        public Vector3 NormalizedDirection { get => Vector3.Normalize(_pointB - PointA); }
    }
}
