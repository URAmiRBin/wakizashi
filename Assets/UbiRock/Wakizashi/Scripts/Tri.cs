using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Tri {
        private Vertex _vertA, _vertB, _vertC;
        public Tri(Vertex a, Vertex b, Vertex c) {
            _vertA = a;
            _vertB = b;
            _vertC = c;
        }

        public (Vertex, Vertex, Vertex) GetVertecies() => (_vertA, _vertB, _vertC);
        public (Vector3, Vector3, Vector3) GetPositions() => (_vertA.Position, _vertB.Position, _vertC.Position);
    }
}
