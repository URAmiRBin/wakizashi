using System.Collections.Generic;
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
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            int[] indices = mesh.GetTriangles(0);
            int indicesCount = indices.Length;

            List<Tri> topTriangles, bottomTriangles;
            topTriangles = new List<Tri>();
            bottomTriangles = new List<Tri>();

            for(int index = 0; index < indicesCount; index += 3) {
                int a = indices[index + 0];
                int b = indices[index + 1];
                int c = indices[index + 2];

                Tri tri = new Tri(new Vertex(vertices[a], normals[a]), new Vertex(vertices[b], normals[b]), new Vertex(vertices[c], normals[c]));

                Intersection intersection = tri.Split(plane);
                
                if (intersection != null) {
                    for (int i = 0; i < intersection.TopTrisCount; i++) {
                        topTriangles.Add(intersection.TopTriangles[i]);
                    }
                    for (int i = 0; i < intersection.BottomTrisCount; i++) {
                        bottomTriangles.Add(intersection.BottomTriangles[i]);
                    }
                } else {
                    if (plane.GetPointToPlaneRelation(vertices[a]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[b]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[c]) == PointToPlaneRelation.TOP ) {
                            topTriangles.Add(tri);
                        } else bottomTriangles.Add(tri);
                }
            }

            Mesh topMesh = MeshGenerator.CreateMeshFromTriangles(topTriangles);
            Mesh bottomMesh = MeshGenerator.CreateMeshFromTriangles(bottomTriangles);
            
            return new SlicedHull(topMesh, bottomMesh);
        }
    }
}