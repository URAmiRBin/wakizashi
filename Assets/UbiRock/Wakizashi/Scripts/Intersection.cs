using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Intersection {
        private Tri[] topTriangles, bottomTriangles;
        private int topTrianglesIndex, bottomTrianglesIndex;

        public Intersection() {
            topTriangles = new Tri[2];
            bottomTriangles = new Tri[2];
            topTrianglesIndex = bottomTriangles = 0;
        }

        public void AddTopTri(Tri tri) => topTriangles[topTrianglesIndex++] = tri;
        public void AddBottomTri(Tri tri) => bottomTriangles[bottomTrianglesIndex++] = tri;

        public int TopTrisCount => topTrianglesIndex;
        public int BottomTrisCount => bottomTrianglesIndex;

        public Tri[] TopTriangles => topTriangles;
        public Tri[] BottomTriangles => bottomTriangles;
    }
}