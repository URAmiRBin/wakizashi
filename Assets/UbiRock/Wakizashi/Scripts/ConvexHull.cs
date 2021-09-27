using System;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class ConvexHull {
        private static (int[], int[]) SeperateHulls(Vertex[] vertices) {
            Array.Sort(vertices);

            int[] upperHull, lowerHull;
            upperHull = new int[vertices.Length];
            lowerHull = new int[vertices.Length];
            int upperHullIndex, lowerHullIndex;

            upperHull[upperHullIndex++] = 0;

            for(int i = 1; i < vertices.Length; i++) {
                if (vertices[i].Position.y >= vertices[0]) upperHull[upperHullIndex++] = i;
                else lowerHull[lowerHullIndex++] = i;
            }

            return (upperHull, lowerHull);
        }

        private static int[] CalculateHull(int[] upperIndices, int[] lowerIndices) {
            int[] hullIndices = upperIndices.Length + lowerIndices.Length;
            
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