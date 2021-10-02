using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public class Map {
        public Vector2 position;
        public int index;
        public float degree;

        public Map(Vector3 p, int i) {
            position = p;
            index = i;
        }
    }
    public static class ConvexHull {
        public static int CCW(Vector2 a, Vector2 b, Vector2 c) {
            float area = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);

            if (area < 0) return -1;
            if (area > 0) return 1;
            return 0;
        }
        public static bool IsAbout(float x, float y) {
            if (x <= y + float.Epsilon && x >= y - float.Epsilon) return true;
            return false;
        }

        public static int Compare(Vector2 p1, Vector2 p2) {
            int ccw = CCW(minP, p1, p2);
            if (ccw == 0) {
                if (Vector2.Distance(minP, p2) > Vector2.Distance(minP, p1)) return -1;
                else return 1;
            } else {
                if (ccw == 1) return -1;
                else return 1;
            }
        }

        static Vector2 minP;

        static int upperHullIndex, lowerHullIndex;
        static Map[] clone;
        public static int[] SeperateHulls(Vertex[] vertices, Vector3 normal) {
            clone = new Map[vertices.Length];
            Stack<Map> stack = new Stack<Map>();

            Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            Vector3 v = Vector3.Cross(u, normal);

            for(int i = 0; i < clone.Length; i++) clone[i] = new Map(new Vector2(Vector3.Dot(vertices[i].Position, u), Vector3.Dot(vertices[i].Position, v)), i);

            int lowestXYIndex = 0;
            float lowestYValue = clone[0].position.y;
            float lowestXValue = clone[0].position.x;

            for(int i = 0; i < clone.Length; i++) {
                if (clone[i].position.y <= lowestYValue) {
                    if (clone[i].position.y == lowestYValue && clone[i].position.x >= lowestXValue) continue;
                    if (clone[i].position.y == lowestYValue && clone[i].position.x < lowestXValue) {
                        lowestXYIndex = i;
                        lowestYValue = clone[i].position.y;
                        lowestXValue = clone[i].position.x;
                    } else {
                        lowestXYIndex = i;
                        lowestYValue = clone[i].position.y;
                        lowestXValue = clone[i].position.x;
                    }
                }
            }

            minP = clone[lowestXYIndex].position;


            for (int i = 0; i < clone.Length; i++) {
                if (i == lowestXYIndex) clone[i].degree = 181;
                else if (clone[i].position == clone[lowestXYIndex].position) clone[i].degree = 180.5f;
                else {
                    clone[i].degree = Vector2.Angle(clone[i].position - new Vector2(lowestXValue, lowestYValue), Vector2.right);
                }
            }

            // Array.Sort(clone, (x, y) => (x.degree > y.degree + 0.1f) || ( IsAbout(x.degree, y.degree) && Vector2.Distance(x.position, new Vector2(lowestXValue, lowestYValue)) <= Vector2.Distance(y.position, new Vector2(lowestXValue, lowestYValue)) + 0.001f && Vector2.Distance(x.position, new Vector2(lowestXValue, lowestYValue)) >= float.Epsilon )  ? -1 : 1);
            Array.Sort(clone, (x, y) => Compare(x.position, y.position));

            for(int i = 0; i < clone.Length; i++) Debug.Log(clone[i].position + " = " + clone[i].degree + " = " + Vector2.Distance(clone[i].position, new Vector2(lowestXValue, lowestYValue)));
            Debug.Log("=========");


            stack.Push(clone[0]);
            stack.Push(clone[1]);

            // stack.Push(clone[lowestXYIndex]);

            // for(int i = 0; i < clone.Length; i++) {
                // if (i == lowestXYIndex) continue;
                // stack.Push(clone[i]);
            // }

            for (int i = 2; i < clone.Length; i++) {
                var p = stack.Pop();
                // var p1 = stack.Peek();
                while (stack.Count != 0 && CCW(stack.Peek().position, p.position, clone[i].position) <= 0) {
                    p = stack.Pop();
                }
                stack.Push(p);
                stack.Push(clone[i]);
            }


            int[] hull = new int[stack.Count];
            // for(int i = hull.Length - 1; i >= 0; i--) {
            //     Debug.Log(stack.Peek().position);
            //     hull[i] = stack.Pop().index;
            // }
            int x = 0;
            while(stack.Count != 0) {
                Debug.Log(stack.Peek().position);
                hull[x++] = stack.Pop().index;
            }
            return hull;
        }
    }
}