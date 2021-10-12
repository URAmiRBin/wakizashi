using UnityEngine;

namespace UbiRock.Wakizashi.Toolkit {
    public static class Intersector {
        // public static Vector3 Intersect(Plane plane, Line line) => Intersect(plane, line.PointA, line.PointB);

        public static Vertex Intersect(Plane plane, Vertex a, Vertex b) {
            Vector3 pNormal = plane.Normal;
            float pDistance = plane.Distance;
            Vector3 line = b.Position - a.Position;

            float t = (pDistance - Vector3.Dot(pNormal, a.Position)) / Vector3.Dot(pNormal, line);
            Vector3 normal = Vector3.Normalize(a.Normal * (1 - t) + b.Normal * t);
            Vector2 uv = a.UV * (1 - t) + b.UV * t;

            if (t >= -Constants.EPSILON && t <= 1 + Constants.EPSILON) return new Vertex(Vector3.Lerp(a.Position, b.Position, t), uv, normal);
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
                    Vertex i = Intersect(plane, b, c);
                    result.AddNewVertex(new Vertex(i.Position, MathHelper.Project(i.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

                    Tri tb = rb == PointToPlaneRelation.TOP ? new Tri(a, b, i) : new Tri(a, i, b);
                    Tri tc = rc == PointToPlaneRelation.TOP ? new Tri(a, c, i) : new Tri(a, i, c);
                    
                    if (rb == PointToPlaneRelation.TOP) {
                        result.AddTopTri(tb);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(tb);
                    }
                    return result;
                } else if (rb == PointToPlaneRelation.SURFACE) {
                    Vertex i = Intersect(plane, a, c);
                    result.AddNewVertex(new Vertex(i.Position, MathHelper.Project(i.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

                    Tri ta = ra == PointToPlaneRelation.TOP ? new Tri(b, a, i) : new Tri(b, i, a);
                    Tri tc = rc == PointToPlaneRelation.TOP ? new Tri(b, c, i) : new Tri(b, i, c);

                    if (ra == PointToPlaneRelation.TOP) {
                        result.AddTopTri(ta);
                        result.AddBottomTri(tc);
                    } else {
                        result.AddTopTri(tc);
                        result.AddBottomTri(ta);
                    }
                    return result;
                } else {
                    Vertex i = Intersect(plane, a, b);
                    result.AddNewVertex(new Vertex(i.Position, MathHelper.Project(i.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

                    Tri ta = ra == PointToPlaneRelation.TOP ? new Tri(c, a, i) : new Tri(c, i, a);
                    Tri tb = rb == PointToPlaneRelation.TOP ?  new Tri(c, b, i) : new Tri(c, i, b);

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
                    Vertex i1 = Intersect(plane, a, b);
                    result.AddNewVertex(new Vertex(i1.Position, MathHelper.Project(i1.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

                    if (rc == ra) {
                        Vertex i2 = Intersect(plane, b, c);
                        result.AddNewVertex(new Vertex(i2.Position, MathHelper.Project(i2.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

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
                        Vertex i2 = Intersect(plane, a, c);
                        result.AddNewVertex(new Vertex(i2.Position, MathHelper.Project(i2.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

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
                    Vertex i1 = Intersect(plane, a, c);
                    result.AddNewVertex(new Vertex(i1.Position, MathHelper.Project(i1.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));
                    Vertex i2 = Intersect(plane, b, c);
                    result.AddNewVertex(new Vertex(i2.Position, MathHelper.Project(i2.Position, plane.Normal, Vector2.one * .5f) ,plane.Normal));

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