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
        static int upperHullIndex, lowerHullIndex;
        static Map[] clone;
        public static int[] SeperateHulls(Vertex[] vertices, Vector3 normal) {
            clone = new Map[vertices.Length];
            Stack<Map> stack = new Stack<Map>();

            Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            Vector3 v = Vector3.Cross(u, normal);

            for(int i = 0; i < clone.Length; i++) clone[i] = new Map(new Vector2(Vector3.Dot(vertices[i].Position, u), Vector3.Dot(vertices[i].Position, v)), i);

            int lowestYIndex = -1;
            float lowestYValue = clone[0].position.y;

            for(int i = 0; i < clone.Length; i++) {
                if (clone[i].position.y < lowestYValue) {
                    lowestYIndex = i;
                    lowestYValue = clone[i].position.y;
                }
            }


            for (int i = 0; i < clone.Length; i++) {
                if (i == lowestYIndex) clone[i].degree = -1;
                else {
                    clone[i].degree = Vector2.Angle(clone[i].position - clone[lowestYIndex].position, Vector2.right);
                }
            }

            Array.Sort(clone, (x, y) => (x.degree < y.degree) || (x.degree == y.degree && y.degree == 180 && x.position.x < y.position.x) || (x.degree == y.degree && y.degree > 90 && x.position.y > y.position.y) || (x.degree == y.degree && y.degree <= 90 && x.position.y < y.position.y) ? -1 : 1);


            for(int i = 0; i < clone.Length; i++) Debug.Log(clone[i].position + " = " + clone[i].degree);

            // stack.Push(clone[0]);
            // stack.Push(clone[1]);

            for(int i = 0; i < clone.Length; i++) stack.Push(clone[i]);

            // for (int i = 2; i < clone.Length; i++) {
            //     var p = stack.Pop();
            //     // var p1 = stack.Peek();
            //     while (stack.Count != 0 && CCW(stack.Peek().position, p.position, clone[i].position) < 0) {
            //         p = stack.Pop();
            //     }
            //     stack.Push(p);
            //     stack.Push(clone[i]);
            // }


            int[] hull = new int[clone.Length];
            for(int i = 0; i < hull.Length; i++) {
                hull[i] = stack.Pop().index;
            }


            return hull;
        }
    }
}