using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Slicer {
        public static SlicedHull Slice(GameObject gameObject, Plane plane) {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;

            return Slice(mesh, plane);
        }

        public static SlicedHull Slice(Mesh mesh, Plane plane) {
            Vector3[] vertecies = mesh.vertecies;

            int[] indices = mesh.GetTriangles(0);
            int indicesCount = indices.Length;

            List<Tri> topTriangles, bottomTriangles;
            topTriangles = new List<Tri>();
            bottomTriangles = new List<Tri>();

            for(int inndex = 0; index < indicesCount; index += 3) {
                int a = indices[index + 0];
                int b = indices[index + 1];
                int c = indices[index + 2];

                Tri tri = new Tri(vertecies[a], vertecies[b], vertecies[c]);

                Intersection intersection = tri.Split(plane);
                
                if (intersection != null) {
                    topTriangles.AddRange(intersection.TopTriangles);
                    bottomTriangles.AddRange(intersection.BottomTriangles);
                } else {
                    if (plane.GetPointToPlaneRelation(vertecies[a]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertecies[b]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertecies[c]) == PointToPlaneRelation.TOP ) {
                            topTriangles.Add(tri);
                        } else bottomTriangles.Add(tri);
                }
            }

            // TODO: Convert top and bottom triangles to meshes
            // TODO: Pass the generated meshes to sliced hull and return
        }
    }
}