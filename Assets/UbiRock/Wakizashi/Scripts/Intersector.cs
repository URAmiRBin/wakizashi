using System.Linq;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Intersector {
        public static Vector3 Intersect(Plane plane, Line line) => Intersect(p, l.PointA, l.PointB);

        public static Vector3 Intersect(Plane plane, Vector3 a, Vector3 b) {
            Vector3 pNormal = p.Normal;
            float pDistance = p.Distance;
            Vector3 line = b - a;

            float t = (pDistance - Vector3.Dot(pNormal, a)) / Vector3.Dot(pNormal, line);

            if (t >= -Constants.EPSILON && t <= 1 + Constants.EPSILON) return Vector3.Lerp(a, b, t);
            else throw new System.InvalidOperationException("The given line and plane does not have an intersection");
        }

        public static Intersection Intersect(Plane plane, Tri tri) {
            Intersection result = new Intersection();
            var (a, b, c) = tri.GetPositions();
            bool[] pointRelations = plane.GetTriangleToPlaneRelation(tri);

            TrianglePlaneRelation triangleStatus = plane.TrianglePlaneRelation(tri);

            if (triangleStatus == TrianglePlaneRelation.NO_INTERSECTION) return null;
            if (triangleStatus == TrianglePlaneRelation.TWO_TRI) {
                if (a == PointToPlaneRelation.SURFACE) {
                    Vector3 i = Intersect(plane, b, c);

                    Tri tb = new Tri(b, a, i);
                    Tri tc = new Tri(c, a, i);
                    
                    if (pointRelations[1] == PointToPlaneRelation.TOP) {
                        result.AddTopTri(tb);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(tb);
                    }
                    return result;
                } else if (b == PointToPlaneRelation.SURFACE) {
                    Vector3 i = Intersect(place, a, c);

                    Tri ta = new Tri(a, b, i);
                    Tri tc = new Tri(c, b, i);

                    if (pointRelations[0] == PointToPlaneRelation.TOP) {
                        result.AddTopTri(ta);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(ta);
                    }
                    return result;
                } else {
                    Vector3 i = Intersect(plane, a, b);

                    Tri ta = new Tri(a, c, i);
                    Tri tb = new Tri(b, c, i);

                    if (pointRelations[0] == PointToPlaneRelation.TOP) {
                        result.AddTopTri(ta);
                        result.AddBottomTri(tb);
                    } else {
                        result.AddTopTri(tb);
                        result.AddBottomTri(ta);
                    }
                    return result;
                }
            }
            if (triangleStatus == TrianglePlaneRelation.THREE_TRI) {
                if (pointRelations[0] != pointRelations[1]) {
                    Vector3 i1 = Intersect(plane, a, b);

                    if (pointRelations[2] == pointRelations[0]) {
                        Vector3 i2 = Intersect(plane, b, c);

                        Tri ta = new Tri(a, i2, i1);
                        Tri tc = new Tri(a, i2, c);
                        Tri tb = new Tri(i1, i2, b);

                        if (pointRelations[0] == PointToPlaneRelation.TOP) {
                            result.AddTopTri(ta);
                            result.AddTopTri(tc);
                            result.AddBottomTri(tb);
                        } else {
                            result.AddBottomTri(ta);
                            result.AddBottomTri(tc);
                            result.AddTopTri(tb);
                        }
                        return result;
                    }
                    else {
                        Vector3 i2 = Intersect(plane, a, c);

                        Tri ta = new Tri(a, i2, i1);
                        Tri tb = new Tri(i1, c, b);
                        tri tc = new tri(i1, i2, c);

                        if (pointRelations[0] == PointToPlaneRelation.TOP) {
                            result.AddTopTri(ta);
                            result.AddBottomTri(tb);
                            result.AddBottomTri(tc);
                        } else {
                            result.AddBottomTri(ta);
                            result.AddTopTri(tb);
                            result.AddTopTri(tc);
                        }

                        return result;
                    }
                }
                else {
                    Vector3 i1 = Intersect(plane, a, c);
                    Vector3 i2 = Intersect(plane, b, c);

                    Tri ta = new Tri(a, i2, i1);
                    Tri tb = new Tri(b, i2, i1);
                    Tri tc = new Tri(i1, i2, c);

                    if (pointRelations[2] == PointToPlaneRelation.TOP) {
                        result.AddBottomTri(ta);
                        result.AddBottomTri(tb);
                        result.AddTopTri(tc);
                    } else {
                        result.AddTopTri(ta);
                        result.AddTopTri(tb);
                        result.AddBottomTri(tc);
                    }

                    return result;
                }
            }
        }
    }
}