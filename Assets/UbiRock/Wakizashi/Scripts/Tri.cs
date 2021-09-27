using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Tri {
        private Vertex _vertA, _vertB, _vertC;
        public Tri(Vertex a, Vertex b, Vertex c) {
            _vertA = a;
            _vertB = b;
            _vertC = c;
        }

        public Tri(Vector3 a, Vector3 b, Vector3 c) {
            _vertA = new Vertex(a);
            _vertB = new Vertex(b);
            _vertC = new Vertex(c);
        }

        public (Vertex, Vertex, Vertex) GetVertecies() => (_vertA, _vertB, _vertC);
        public (Vector3, Vector3, Vector3) GetPositions() => (_vertA.Position, _vertB.Position, _vertC.Position);
        public (Vector3, Vector3, Vector3) GetNormals() => (_vertA.Normal, _vertB.Normal, _vertC.Normal);

        public void SetNormals(Vector3 normal) {
            _vertA.Normal = normal;
            _vertB.Normal = normal;
            _vertC.Normal = normal;
        }

        public void FlipNormals() {
            _vertA.Normal = -_vertA.Normal;
            _vertB.Normal = -_vertB.Normal;
            _vertC.Normal = -_vertC.Normal;
        }

        public Intersection Split(Plane plane) {
            return Intersector.Intersect(plane, this);
        }
    }
}
