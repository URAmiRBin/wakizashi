using System;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Map {
        public Vector3 position;
        public int index;

        public Map(Vector3 p, int i) {
            position = p;
            index = i;
        }
    }
    public static class ConvexHull {
        static int upperHullIndex, lowerHullIndex;
        static Map[] clone;
        private static (int[], int[]) SeperateHulls(Vertex[] vertices, Vector3 normal) {
            clone = new Map[vertices.Length];

            for(int i = 0; i < clone.Length; i++) clone[i] = new Map(Vector3.ProjectOnPlane(vertices[i].Position, normal), i);
            Array.Sort(clone, (x, y) => (x.position.x < y.position.x) || (x.position.x == y.position.x && x.position.x == 0.5f && x.position.z > y.position.z) || (x.position.x == y.position.x && x.position.x == -0.5f && x.position.z < y.position.z) || (x.position.x == y.position.x && x.position.z > y.position.z) ? -1 : 1);


            int[] upperHull, lowerHull;
            upperHull = new int[vertices.Length];
            lowerHull = new int[vertices.Length];
            upperHullIndex = 0;
            lowerHullIndex = 0;

            upperHull[upperHullIndex++] = 0;

            for(int i = 1; i < clone.Length; i++) {
                if (clone[i].position.z >= clone[0].position.z) upperHull[upperHullIndex++] = i;
                else lowerHull[lowerHullIndex++] = i;
            }


            return (upperHull, lowerHull);
        }

        private static int[] CalculateHull(int[] upperIndices, int[] lowerIndices) {
            int[] hullIndices = new int[upperHullIndex + lowerHullIndex];
            
            for (int i = 0; i < upperHullIndex; i++)
                hullIndices[i] = upperIndices[i];
            for (int i = lowerHullIndex - 1, j = upperHullIndex; i >= 0; i--) {
                hullIndices[j++] = lowerIndices[i];
            }

            return hullIndices;
        }

        public static int[] GetConvexHull(Vertex[] vertices, Vector3 normal) {
            for(int i = 0; i < vertices.Length; i++) Debug.Log(vertices[i].Position);
            var (hull1, hull2) = SeperateHulls(vertices, normal);
            int[] indices = CalculateHull(hull1, hull2);
            int[] result = new int[indices.Length];
            for (int i = 0; i < indices.Length; i++) {
                result[i] = clone[indices[i]].index;
            }
            for(int i = 0; i < vertices.Length; i++) Debug.Log(result[i]);
            return result;
        }
    }
}