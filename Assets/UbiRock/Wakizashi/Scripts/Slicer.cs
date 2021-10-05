using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Slicer {
        public static SlicedHull Slice(Sliceable sliceable, Plane plane) => Slice(sliceable.Mesh, plane, sliceable.Fill);

        public static SlicedHull Slice(Mesh mesh, Plane plane, bool fill) {
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = mesh.uv;


            int[] indices = mesh.GetTriangles(0);
            int indicesCount = indices.Length;
            Vertex[] newVertices = new Vertex[indicesCount * 2];
            int newVerticesCount = 0;

            List<Tri> topTriangles, bottomTriangles;
            topTriangles = new List<Tri>();
            bottomTriangles = new List<Tri>();

            for(int index = 0; index < indicesCount; index += 3) {
                int a = indices[index + 0];
                int b = indices[index + 1];
                int c = indices[index + 2];

                Tri tri = new Tri(new Vertex(vertices[a], uvs[a], normals[a]), new Vertex(vertices[b], uvs[b], normals[b]), new Vertex(vertices[c], uvs[c], normals[c]));

                Intersection intersection = tri.Split(plane);
                
                if (intersection != null) {
                    for (int i = 0; i < intersection.TopTrisCount; i++) {
                        topTriangles.Add(intersection.TopTriangles[i]);
                    }
                    for (int i = 0; i < intersection.BottomTrisCount; i++) {
                        bottomTriangles.Add(intersection.BottomTriangles[i]);
                    }
                    for (int i = 0; i < intersection.NewVerticesCount; i++) {
                        newVertices[i + newVerticesCount] = intersection.NewVertices[i];
                    }
                    newVerticesCount += intersection.NewVerticesCount;
                } else {
                    if (plane.GetPointToPlaneRelation(vertices[a]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[b]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[c]) == PointToPlaneRelation.TOP ) {
                            topTriangles.Add(tri);
                        } else bottomTriangles.Add(tri);
                }
            }

            List<Tri> fillTriangles = fill ? Triangulate(ArrayHelper.SliceTo<Vertex>(newVertices, newVerticesCount), plane.Normal) : null;
            
            Mesh bottomMesh = MeshGenerator.CreateMeshFromTriangles(bottomTriangles, fillTriangles, false);
            Mesh topMesh = MeshGenerator.CreateMeshFromTriangles(topTriangles, fillTriangles, true);
            
            return new SlicedHull(topMesh, bottomMesh);
        }

        static List<Tri> Triangulate(Vertex[] newVertices, Vector3 normal) {
            List<Tri> fillTriangles = new List<Tri>();
            
            Vertex fanMiddleVertex = MathHelper.Average(newVertices);
            fanMiddleVertex.Normal = normal;
            fanMiddleVertex.UV = MathHelper.Project(fanMiddleVertex.Position, normal, Vector2.one * .5f);


            int[] hullIndices = ConvexHull.CalculateConvexHull(newVertices, normal);
            // FIXME: Handle if the count does not create a triangle

            for (int i = 0; i < hullIndices.Length - 1; i++) {
                fillTriangles.Add(new Tri(
                    fanMiddleVertex,
                    newVertices[hullIndices[i]],
                    newVertices[hullIndices[i + 1]]
                    ));
            }

            fillTriangles.Add(new Tri(
                    fanMiddleVertex,
                    newVertices[hullIndices[hullIndices.Length - 1]],
                    newVertices[hullIndices[0]]
                    ));

            return fillTriangles;

        }
    }
}