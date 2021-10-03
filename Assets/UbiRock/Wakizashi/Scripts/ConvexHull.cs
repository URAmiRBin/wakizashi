using System;
using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Map2D {
        public Vector2 position;
        public int index;
        public Map2D(Vector2 p, int i) {
            position = p;
            index = i;
        }
    }

    public static class ConvexHull {
        static Vector2 referencePoint;
        static Map2D[] maps;
        
        public static int AreaSign(Vector2 a, Vector2 b, Vector2 c) {
            float area = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);

            if (area < 0) return -1; // Clockwise
            if (area > 0) return 1; // Counter-clockwise
            return 0; // 3 vectors are colinear
        }

        public static int Compare(Vector2 p1, Vector2 p2) {
            int areaSign = AreaSign(referencePoint, p1, p2);

            if (areaSign == 0) {
                if (Vector2.Distance(referencePoint, p2) > Vector2.Distance(referencePoint, p1)) return -1;
                else return 1;
            } else {
                if (areaSign == 1) return -1;
                else return 1;
            }
        }
        
        public static int[] CalculateConvexHull(Vertex[] vertices, Vector3 normal) {
            maps = MathHelper.Project(vertices, normal);
            Stack<Map2D> hulls = new Stack<Map2D>();

            int lowestXYIndex = 0;
            float lowestYValue = maps[0].position.y;

            for(int i = 0; i < maps.Length; i++) {
                if (maps[i].position.y <= lowestYValue) {
                    lowestXYIndex = i;
                    lowestYValue = maps[i].position.y;
                }
            }

            referencePoint = maps[lowestXYIndex].position;


            Array.Sort(maps, (x, y) => Compare(x.position, y.position));

            hulls.Push(maps[0]);
            hulls.Push(maps[1]);

            for (int i = 2; i < maps.Length; i++) {
                var p = hulls.Pop();
                while (hulls.Count != 0 && AreaSign(hulls.Peek().position, p.position, maps[i].position) <= 0) {
                    p = hulls.Pop();
                }
                hulls.Push(p);
                hulls.Push(maps[i]);
            }


            int[] convexHullIndices = new int[hulls.Count];
            
            int index = 0;
            while(hulls.Count != 0) convexHullIndices[index++] = hulls.Pop().index;

            return convexHullIndices;
        }
    }
}