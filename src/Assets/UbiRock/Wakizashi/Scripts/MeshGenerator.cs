using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MeshGenerator {
        public static Mesh CreateMeshFromTriangles(List<Tri> triangles, List<Tri> fillTriangles, bool isTop) {
            Mesh result = new Mesh();

            bool isFill = fillTriangles != null;

            int vertexCount = triangles.Count * 3 + (isFill ? fillTriangles.Count * 3 : 0);

            Vector3[] vertecies = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            int[] indices = new int[vertexCount];

            for(int i = 0; i < triangles.Count * 3; i += 3) {
                (vertecies[i], vertecies[i + 1], vertecies[i + 2]) = triangles[i / 3].GetPositions();
                (normals[i], normals[i + 1], normals[i + 2]) = triangles[i / 3].GetNormals();
                (uvs[i], uvs[i + 1], uvs[i + 2]) = triangles[i / 3].GetUVs();


                indices[i] = i;
                indices[i + 1] = i + 1;
                indices[i + 2] = i + 2;
            }

            if (isFill) {
                for(int i = 0; i < fillTriangles.Count * 3; i += 3) {
                    (vertecies[i + triangles.Count * 3], vertecies[i + 1 + triangles.Count * 3], vertecies[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetPositions();
                    (normals[i + triangles.Count * 3], normals[i + 1 + triangles.Count * 3], normals[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetNormals(isTop);
                    (uvs[i + triangles.Count * 3], uvs[i + triangles.Count * 3 + 1], uvs[i + triangles.Count * 3 + 2]) = fillTriangles[i / 3].GetUVs();

                    int start = isTop ? 2 : 0;
                    int inc = isTop ? -1 : 1;
                    indices[i + triangles.Count * 3] = i + triangles.Count * 3 + start;
                    indices[i + 1 + triangles.Count * 3] = i + triangles.Count * 3 + start + inc;
                    indices[i + 2 + triangles.Count * 3] = i + triangles.Count * 3 + start + inc + inc;
                }
            }
            

            result.vertices = vertecies;
            result.normals = normals;
            result.uv = uvs;
            result.SetTriangles(indices, 0, false);

            // WRITE ABOOT THIS
            result.Optimize();

            return result;
        }
    }
}