using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MeshGenerator {
        public static Mesh CreateMeshFromTriangles(List<Tri> triangles) {
            Mesh result = new Mesh();

            Vector3[] vertecies = new Vector3[triangles.Count * 3];
            int[] indices = new int[triangles.Count * 3];

            for(int i = 0; i < triangles.Count; i += 3) {
                (vertecies[i], vertecies[i + 1], vertecies[i + 2]) = triangles[i / 3].GetPositions();

                indices[i] = i;
                indices[i + 1] = i + 1;
                indices[i + 2] = i + 2;
            }

            result.vertices = vertecies;
            result.SetTriangles(indices, 0, false);
            return result;
        }
    }
}