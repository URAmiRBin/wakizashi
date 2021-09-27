using System;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Map {
        public Vector2 position;
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

            float xMin = 0, xMax = 0;

            Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            Vector3 v = Vector3.Cross(u, normal);

            for(int i = 0; i < clone.Length; i++) clone[i] = new Map(new Vector2(Vector3.Dot(vertices[i].Position, u), Vector3.Dot(vertices[i].Position, v)), i);

            for(int i = 0; i < clone.Length; i++) {
                if (clone[i].position.x < xMin) xMin = clone[i].position.x;
                else if (clone[i].position.x > xMax) xMax = clone[i].position.x;
            }

            Array.Sort(clone, (x, y) => (x.position.x < y.position.x) || (x.position.x == y.position.x && x.position.x == xMin && x.position.y < y.position.y) || (x.position.x == y.position.x && x.position.x == xMax && x.position.y > y.position.y)  ? -1 : 1);

            for(int i = 0; i < clone.Length; i++) {
                Debug.Log(clone[i].position);
            }

            Debug.Log("=====");

            int[] upperHull, lowerHull;
            upperHull = new int[vertices.Length];
            lowerHull = new int[vertices.Length];
            upperHullIndex = 0;
            lowerHullIndex = 0;

            upperHull[upperHullIndex++] = 0;

            for(int i = 1; i < clone.Length; i++) {
                if (clone[i].position.y > clone[0].position.y) upperHull[upperHullIndex++] = i;
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

            for(int i = 0; i < clone.Length; i++) {
                Debug.Log(clone[hullIndices[i]].position);
            }

            return hullIndices;
        }

        public static int[] GetConvexHull(Vertex[] vertices, Vector3 normal) {
            var (hull1, hull2) = SeperateHulls(vertices, normal);
            int[] indices = CalculateHull(hull1, hull2);
            int[] result = new int[indices.Length];
            for (int i = 0; i < indices.Length; i++) {
                result[i] = clone[indices[i]].index;
            }
            return result;
        }
    }
}