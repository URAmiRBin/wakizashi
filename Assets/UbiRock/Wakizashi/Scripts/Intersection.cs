namespace UbiRock.Wakizashi.Toolkit {
    public class Intersection {
        private Tri[] topTriangles, bottomTriangles;
        private Vertex[] newVertices;
        private int topTrianglesIndex, bottomTrianglesIndex;
        private int newVerticesIndex;

        public Intersection() {
            topTriangles = new Tri[2];
            bottomTriangles = new Tri[2];
            topTrianglesIndex = bottomTrianglesIndex = 0;

            newVertices = new Vertex[2];
            newVerticesIndex = 0;
        }

        public void AddTopTri(Tri tri) => topTriangles[topTrianglesIndex++] = tri;
        public void AddBottomTri(Tri tri) => bottomTriangles[bottomTrianglesIndex++] = tri;
        public void AddNewVertex(Vertex v) => newVertices[newVerticesIndex++] = v;

        public int TopTrisCount => topTrianglesIndex;
        public int BottomTrisCount => bottomTrianglesIndex;
        public int NewVerticesCount => newVerticesIndex;

        public Tri[] TopTriangles => topTriangles;
        public Tri[] BottomTriangles => bottomTriangles;
        public Vertex[] NewVertices => newVertices;
    }
}