using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MeshGenerator {
        public static Mesh CreateMeshFromTriangles(List<Tri> triangles, List<Tri> fillTriangles) {
            Mesh result = new Mesh();

            Vector3[] vertecies = new Vector3[triangles.Count * 3 + fillTriangles.Count * 3];
            Vector3[] normals = new Vector3[triangles.Count * 3 + fillTriangles.Count * 3];
            int[] indices = new int[triangles.Count * 3];
            int[] indicesFill = new int[fillTriangles.Count * 3];

            for(int i = 0; i < triangles.Count * 3; i += 3) {
                (vertecies[i], vertecies[i + 1], vertecies[i + 2]) = triangles[i / 3].GetPositions();
                (normals[i], normals[i + 1], normals[i + 2]) = triangles[i / 3].GetNormals();


                indices[i] = i;
                indices[i + 1] = i + 1;
                indices[i + 2] = i + 2;
            }

            for(int i = 0; i < fillTriangles.Count * 3; i += 3) {
                (vertecies[i + triangles.Count * 3], vertecies[i + 1 + triangles.Count * 3], vertecies[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetPositions();
                (normals[i + triangles.Count * 3], normals[i + 1 + triangles.Count * 3], normals[i + 2 + triangles.Count * 3]) = fillTriangles[i / 3].GetNormals();

                indicesFill[i] = i + triangles.Count * 3;
                indicesFill[i + 1] = i + triangles.Count * 3 + 1;
                indicesFill[i + 2] = i + triangles.Count * 3 + 2;
            }

            result.vertices = vertecies;
            result.normals = normals;
            result.subMeshCount = 2;
            result.SetTriangles(indices, 0, false);
            result.SetTriangles(indicesFill, 1, false);

            return result;
        }
    }
}