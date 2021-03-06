using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MathHelper {
        public static Vector2 Project(Vector3 point, Vector3 normal) {
            Vector3 u, v;
            (u, v) = CalculateUV(normal);
            return new Vector2(Vector3.Dot(point, u), Vector3.Dot(point, v));
        }

        public static Vector2 Project(Vector3 point, Vector3 normal, Vector2 offset) {
            Vector2 p = Project(point, normal);
            return new Vector2(p.x + offset.x, p.y + offset.y);
        }

        public static Vector2[] Project(Vector3[] points, Vector3 normal) {
            Vector3 u, v;
            (u, v) = CalculateUV(normal);

            Vector2[] mappedPoints = new Vector2[points.Length];

            int index = 0;
            foreach (Vector3 point in points) {
                mappedPoints[index++] = new Vector2(Vector3.Dot(point, u), Vector3.Dot(point, v));
            }

            return mappedPoints;
        }

        public static Map2D[] Project(Vertex[] vertices, Vector3 normal) {
            Vector3 u, v;
            (u, v) = CalculateUV(normal);

            Map2D[] mappedPoints = new Map2D[vertices.Length];

            for(int i = 0; i < vertices.Length; i++) {
                Vector3 point = vertices[i].Position;
                mappedPoints[i] = new Map2D(new Vector2(Vector3.Dot(point, u), Vector3.Dot(point, v)), i);
            }

            return mappedPoints;
        }

        public static (Vector3, Vector3) CalculateUV(Vector3 normal) {
            Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            if (u == Vector3.zero) u = Vector3.Normalize(Vector3.Cross(normal, Vector3.up));
            Vector3 v = Vector3.Cross(u, normal);
            return (u, v);
        }

        public static Vertex Average(Vertex[] vertices) {
            Vector3 sum = Vector3.zero;
            foreach(Vertex v in vertices) sum += v.Position;
            return new Vertex(sum / vertices.Length); 
        }
    }
}