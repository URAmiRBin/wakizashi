using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class MathHelper {
        public static Vector2 Project(Vector3 point, Vector3 normal) {
            Vector3 u, v;
            (u, v) = CalculateUV(normal);
            return new Vector2(Vector3.Dot(point, u), Vector3.Dot(point, v));
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

        public static (Vector3, Vector3) CalculateUV(Vector3 normal) {
            Vector3 u = Vector3.Normalize(Vector3.Cross(normal, Vector3.forward));
            if (u == Vector3.zero) u = Vector3.Normalize(Vector3.Cross(normal, Vector3.up));
            Vector3 v = Vector3.Cross(u, normal);
            return (u, v);
        }
    }
}