using System;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class ConvexHull {
        private static (int[], int[]) SeperateHulls(Vertex[] vertices) {
            Vector3[] clone = new Vector3[vertices.Length];

            for(int i = 0; i < clone.Length; i++) clone[i] = Vector3.ProjectOnPlane(vertices[i].Position, Vector3.up);
            Array.Sort(clone, (x, y) => x.x >= y.x ? 1 : -1);

            int[] upperHull, lowerHull;
            upperHull = new int[vertices.Length];
            lowerHull = new int[vertices.Length];
            int upperHullIndex = 0, lowerHullIndex = 0;

            upperHull[upperHullIndex++] = 0;

            for(int i = 1; i < clone.Length; i++) {
                if (clone[i].z <= clone[0].z) upperHull[upperHullIndex++] = i;
                else lowerHull[lowerHullIndex++] = i;
            }

            return (upperHull, lowerHull);
        }

        private static int[] CalculateHull(int[] upperIndices, int[] lowerIndices) {
            int[] hullIndices = new int[upperIndices.Length + lowerIndices.Length];
            
            for (int i = 0; i < upperIndices.Length; i++)
                hullIndices[i] = upperIndices[i];
            for (int i = lowerIndices.Length - 1, j = upperIndices.Length; i >= 0; i--) {
                hullIndices[j++] = lowerIndices[i];
            }

            return hullIndices;
        }

        public static int[] GetConvexHull(Vertex[] vertices) {
            var (hull1, hull2) = SeperateHulls(vertices);
            return CalculateHull(hull1, hull2);
        }
    }
}