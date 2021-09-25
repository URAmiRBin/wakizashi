using System.Linq;
using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Intersector {
        public static Vector3 Intersect(Plane plane, Line line) => Intersect(plane, line.PointA, line.PointB);

        public static Vector3 Intersect(Plane plane, Vector3 a, Vector3 b) {
            Vector3 pNormal = plane.Normal;
            float pDistance = plane.Distance;
            Vector3 line = b - a;

            float t = (pDistance - Vector3.Dot(pNormal, a)) / Vector3.Dot(pNormal, line);

            if (t >= -Constants.EPSILON && t <= 1 + Constants.EPSILON) return Vector3.Lerp(a, b, t);
            else throw new System.InvalidOperationException("The given line and plane does not have an intersection");
        }

        public static Intersection Intersect(Plane plane, Tri tri) {
            Intersection result = new Intersection();
            var (a, b, c) = tri.GetVertecies();
            var (ra, rb, rc) = plane.GetPointToPlaneRelations(tri);

            TrianglePlaneRelation triangleStatus = plane.GetTriangleToPlaneRelation(tri);

            if (triangleStatus == TrianglePlaneRelation.NO_INTERSECTION) return null;
            if (triangleStatus == TrianglePlaneRelation.TWO_TRI) {
                if (ra == PointToPlaneRelation.SURFACE) {
                    // TODO: Calculate real normals
                    Vertex i = new Vertex(Intersect(plane, b.Position, c.Position), a.Normal);

                    Tri tb = new Tri(b, a, i);
                    Tri tc = new Tri(c, a, i);
                    
                    if (ra == PointToPlaneRelation.TOP) {
                        result.AddTopTri(tb);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(tb);
                    }
                    return result;
                } else if (rb == PointToPlaneRelation.SURFACE) {
                    // TODO: Calculate real normals
                    Vertex i = new Vertex(Intersect(plane, a.Position, c.Position), a.Normal);

                    Tri ta = new Tri(a, b, i);
                    Tri tc = new Tri(c, b, i);

                    if (ra == PointToPlaneRelation.TOP) {
                        result.AddTopTri(ta);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(ta);
                    }
                    return result;
                } else {
                    // TODO: Calculate real normals
                    Vertex i = new Vertex(Intersect(plane, a.Position, b.Position), a.Normal);

                    Tri ta = new Tri(a, c, i);
                    Tri tb = new Tri(b, c, i);

                    if (ra == PointToPlaneRelation.TOP) {
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
                if (ra != rb) {
                    // TODO: Calculate real normals
                    Vertex i1 = new Vertex(Intersect(plane, a.Position, b.Position), a.Normal);

                    if (rc == ra) {
                        // TODO: Calculate real normals
                        Vertex i2 = new Vertex(Intersect(plane, b.Position, c.Position), b.Normal);

                        Tri ta = new Tri(a, i1, i2);
                        Tri tc = new Tri(c, a, i2);
                        Tri tb = new Tri(b, i2, i1);

                        if (ra == PointToPlaneRelation.TOP) {
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
                        // TODO: Calculate real normals
                        Vertex i2 = new Vertex(Intersect(plane, a.Position, c.Position), a.Normal);

                        Tri ta = new Tri(a, i1, i2);
                        Tri tb = new Tri(i1, b, c);
                        Tri tc = new Tri(i2, i1, c);

                        if (ra == PointToPlaneRelation.TOP) {
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
                    // TODO: Calculate real normals
                    Vertex i1 = new Vertex(Intersect(plane, a.Position, c.Position), a.Normal);
                    // TODO: Calculate real normals
                    Vertex i2 = new Vertex(Intersect(plane, b.Position, c.Position), b.Normal);

                    Tri ta = new Tri(a, b, i1);
                    Tri tb = new Tri(b, i2, i1);
                    Tri tc = new Tri(c, i1, i2);

                    if (rc == PointToPlaneRelation.TOP) {
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
            return null;
        }
    }
}