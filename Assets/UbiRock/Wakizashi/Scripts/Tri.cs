using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Tri {
        private Vertex _vertA, _vertB, _vertC;
        public Tri(Vertex a, Vertex b, Vertex c) {
            _vertA = a;
            _vertB = b;
            _vertC = c;
        }
    }
}
