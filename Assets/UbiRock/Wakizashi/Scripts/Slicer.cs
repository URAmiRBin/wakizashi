using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
            Vector2[] uvs = mesh.uv;


            int[] indices = mesh.GetTriangles(0);
            int indicesCount = indices.Length;
            Vertex[] newVerticesArray = new Vertex[indicesCount * 2];
            int newVerticesCount = 0;

            List<Tri> topTriangles, bottomTriangles, fillTriangles;
            topTriangles = new List<Tri>();
            bottomTriangles = new List<Tri>();
            fillTriangles = new List<Tri>();

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
                    int d = 0;
                    for (int i = 0; i < intersection.NewVerticesCount; i++) {
                        Vector3 xxx = intersection.NewVertices[i].Position;
                        Vector3 nu = Vector3.Normalize(Vector3.Cross(plane.Normal, Vector3.forward));
                        Vector3 nv = Vector3.Cross(nu, plane.Normal);
                        Vector2 newUV = new Vector2(Vector3.Dot(xxx, nu) + .5f, Vector3.Dot(xxx, nv) + .5f);
                        newVerticesArray[i + newVerticesCount] = new Vertex(intersection.NewVertices[i].Position, newUV, intersection.NewVertices[i].Normal);
                        d++;
                    }
                    newVerticesCount += d;
                } else {
                    if (plane.GetPointToPlaneRelation(vertices[a]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[b]) == PointToPlaneRelation.TOP ||
                        plane.GetPointToPlaneRelation(vertices[c]) == PointToPlaneRelation.TOP ) {
                            topTriangles.Add(tri);
                        } else bottomTriangles.Add(tri);
                }
            }

            Vertex[] newVertices = new Vertex[newVerticesCount];
            for (int i = 0; i < newVerticesCount; i++) newVertices[i] = newVerticesArray[i]; 
            int cc = newVerticesCount;

            for(int i = 0; i < newVerticesCount; i++) {
                if (newVertices[i] != null)  {
                    for(int x = i + 1; x < newVerticesCount; x++) {
                        if (newVertices[x] != null) {
                            if (Vector3.Equals(newVertices[i].Position, newVertices[x].Position)) {
                                newVertices[x] = null;
                                cc--;
                            }
                        }
                    }
                }
            }

            Vertex[] veryNewVertices = new Vertex[cc];
            int j = 0;
            for (int i = 0; i < newVerticesCount; i++) {
                if (newVertices[i] != null)
                    veryNewVertices[j++] = newVertices[i];
            }
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < cc; i++) {
                sum += veryNewVertices[i].Position;
            }

            sum /= cc;

            Vector3 u = Vector3.Normalize(Vector3.Cross(plane.Normal, Vector3.forward));
            Vector3 v = Vector3.Cross(u, plane.Normal);
            Vector2 xx = new Vector2(Vector3.Dot(sum, u) + .5f, Vector3.Dot(sum, v) + .5f);


            // TODO: Fill should be an option

            int[] hullIndices = ConvexHull.SeperateHulls(veryNewVertices, plane.Normal);
            // FIXME: Handle if the count does not create a triangle

            // TODO: Add this to triangulator
            // TODO: Account for concave
            for (int i = 0; i < hullIndices.Length - 1; i++) {
                fillTriangles.Add(new Tri(
                    new Vertex(sum, xx, plane.Normal),
                    veryNewVertices[hullIndices[i]],
                    veryNewVertices[hullIndices[i + 1]]
                    ));
            }

            fillTriangles.Add(new Tri(
                    new Vertex(sum, xx, plane.Normal),
                    veryNewVertices[hullIndices[hullIndices.Length - 1]],
                    veryNewVertices[hullIndices[0]]
                    ));

            Mesh bottomMesh = MeshGenerator.CreateMeshFromTriangles(bottomTriangles, fillTriangles, false);
            Mesh topMesh = MeshGenerator.CreateMeshFromTriangles(topTriangles, fillTriangles, true);
            
            return new SlicedHull(topMesh, bottomMesh);
        }
    }
}