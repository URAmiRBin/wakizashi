using UnityEngine;

namespace UbiRock.Toolkit {
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
    }
}
