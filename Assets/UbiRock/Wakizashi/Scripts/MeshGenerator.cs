using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MeshGenerator {
        public static Mesh CreateMeshFromTriangles(List<Tri> triangles, List<Tri> fillTriangles, bool isTop) {
            Mesh result = new Mesh();

            Vector3[] vertecies = new Vector3[triangles.Count * 3 + fillTriangles.Count * 3];
            Vector3[] normals = new Vector3[triangles.Count * 3 + fillTriangles.Count * 3];
            int[] indices = new int[triangles.Count * 3 + fillTriangles.Count * 3];

            for(int i = 0; i < triangles.Count * 3; i += 3) {
                (vertecies[i], vertecies[i + 1], vertecies[i + 2]) = triangles[i / 3].GetPositions();
                (normals[i], normals[i + 1], normals[i + 2]) = triangles[i / 3].GetNormals();


                indices[i] = i;
                indices[i + 1] = i + 1;
                indices[i + 2] = i + 2;
            }


            for(int i = 0; i < fillTriangles.Count * 3; i += 3) {
                (vertecies[i + triangles.Count * 3], vertecies[i + 1 + triangles.Count * 3], vertecies[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetPositions();
                (normals[i + triangles.Count * 3], normals[i + 1 + triangles.Count * 3], normals[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetNormals(isTop);

                int start = isTop ? 2 : 0;
                int inc = isTop ? -1 : 1;
                indices[i + triangles.Count * 3] = i + triangles.Count * 3 + start;
                indices[i + 1 + triangles.Count * 3] = i + triangles.Count * 3 + start + inc;
                indices[i + 2 + triangles.Count * 3] = i + triangles.Count * 3 + start + inc + inc;
            }

            result.vertices = vertecies;
            result.normals = normals;
            result.SetTriangles(indices, 0, false);

            // WRITE ABOOT THIS
            result.Optimize();

            return result;
        }
    }
}