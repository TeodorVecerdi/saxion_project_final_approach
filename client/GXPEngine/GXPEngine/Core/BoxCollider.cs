namespace GXPEngine.Core {
    public class BoxCollider : Collider {
        private readonly Sprite _owner;

        //------------------------------------------------------------------------------------------------------------------------
        //														BoxCollider()
        //------------------------------------------------------------------------------------------------------------------------		
        public BoxCollider(Sprite owner) {
            _owner = owner;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        public override bool HitTest(Collider other) {
            if (other is BoxCollider) {
                var c = _owner.GetExtents();
                if (c == null) return false;
                var d = ((BoxCollider) other)._owner.GetExtents();
                if (d == null) return false;
                if (!areaOverlap(c, d)) return false;
                return areaOverlap(d, c);
            }

            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        public override bool HitTestPoint(float x, float y) {
            var c = _owner.GetExtents();
            if (c == null) return false;
            var p = new Vector2(x, y);
            return pointOverlapsArea(p, c);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														areaOverlap()
        //------------------------------------------------------------------------------------------------------------------------
        private bool areaOverlap(Vector2[] c, Vector2[] d) {
            // normal 1:
            var ny = c[1].x - c[0].x;
            var nx = c[0].y - c[1].y;

            // own 'depth' in direction of this normal:
            var dx = c[3].x - c[0].x;
            var dy = c[3].y - c[0].y;
            var dot = dy * ny + dx * nx;

            if (dot == 0.0f) dot = 1.0f;

            float t, minT, maxT;

            t = ((d[0].x - c[0].x) * nx + (d[0].y - c[0].y) * ny) / dot;
            maxT = t;
            minT = t;

            t = ((d[1].x - c[0].x) * nx + (d[1].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[2].x - c[0].x) * nx + (d[2].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[3].x - c[0].x) * nx + (d[3].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            if (minT >= 1 || maxT <= 0) return false;

            // second normal:
            ny = dx;
            nx = -dy;
            dx = c[1].x - c[0].x;
            dy = c[1].y - c[0].y;
            dot = dy * ny + dx * nx;

            if (dot == 0.0f) dot = 1.0f;

            t = ((d[0].x - c[0].x) * nx + (d[0].y - c[0].y) * ny) / dot;
            maxT = t;
            minT = t;

            t = ((d[1].x - c[0].x) * nx + (d[1].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[2].x - c[0].x) * nx + (d[2].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[3].x - c[0].x) * nx + (d[3].y - c[0].y) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            if (minT >= 1 || maxT <= 0) return false;

            return true;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														pointOverlapsArea()
        //------------------------------------------------------------------------------------------------------------------------
        //ie. for hittestpoint and mousedown/up/out/over
        private bool pointOverlapsArea(Vector2 p, Vector2[] c) {
            var dx1 = c[1].x - c[0].x;
            var dy1 = c[1].y - c[0].y;
            var dx2 = c[3].x - c[0].x;
            var dy2 = c[3].y - c[0].y;

            // first: take delta1 as normal:
            var dot = dy2 * dx1 - dx2 * dy1;

            float t;

            t = ((p.y - c[0].y) * dx1 - (p.x - c[0].x) * dy1) / dot;
            if (t > 1 || t < 0) return false;

            // next: take delta2 as normal:
            dot = -dot;

            t = ((p.y - c[0].y) * dx2 - (p.x - c[0].x) * dy2) / dot;

            if (t > 1 || t < 0) return false;

            return true;
        }

        public override float TimeOfImpact(Collider other, float vx, float vy, out Vector2 normal) {
            normal = new Vector2();
            if (other is BoxCollider) {
                var c = _owner.GetExtents();
                if (c == null) return float.MaxValue;
                var d = ((BoxCollider) other)._owner.GetExtents();
                if (d == null) return float.MaxValue;

                var maxTOI = float.MinValue;
                var minTOE = float.MaxValue;

                // normals of this vs points of other:
                var nx = -c[0].y + c[1].y;
                var ny = -c[1].x + c[0].x;
                if (updateImpactExitTime(
                    c[0].x, c[0].y, nx, ny,
                    c[3].x - c[0].x, c[3].y - c[0].y, d, -vx, -vy, ref maxTOI, ref minTOE)) {
                    normal.x = nx;
                    normal.y = ny;
                }

                if (minTOE <= maxTOI || minTOE <= 0)
                    return float.MaxValue;
                nx = c[0].y - c[3].y;
                ny = c[3].x - c[0].x;
                if (updateImpactExitTime(
                    c[0].x, c[0].y, nx, ny,
                    c[1].x - c[0].x, c[1].y - c[0].y, d, -vx, -vy, ref maxTOI, ref minTOE)) {
                    normal.x = nx;
                    normal.y = ny;
                }

                if (minTOE <= maxTOI || minTOE <= 0)
                    return float.MaxValue;

                // normals of other vs points of this:
                nx = -d[0].y + d[1].y;
                ny = -d[1].x + d[0].x;
                if (updateImpactExitTime(
                    d[0].x, d[0].y, nx, ny,
                    d[3].x - d[0].x, d[3].y - d[0].y, c, vx, vy, ref maxTOI, ref minTOE)) {
                    normal.x = nx;
                    normal.y = ny;
                }

                if (minTOE <= maxTOI || minTOE <= 0)
                    return float.MaxValue;

                nx = d[0].y - d[3].y;
                ny = d[3].x - d[0].x;
                if (updateImpactExitTime(
                    d[0].x, d[0].y, nx, ny,
                    d[1].x - d[0].x, d[1].y - d[0].y, c, vx, vy, ref maxTOI, ref minTOE)) {
                    normal.x = nx;
                    normal.y = ny;
                }

                if (minTOE <= maxTOI || minTOE <= 0)
                    return float.MaxValue;

                // normalize the normal when there's an actual collision:
                var nLen = Mathf.Sqrt(normal.x * normal.x + normal.y * normal.y);
                normal.x /= nLen;
                normal.y /= nLen;
                if (normal.x * vx + normal.y * vy > 0) {
                    normal.x *= -1;
                    normal.y *= -1;
                }

                if (maxTOI >= 0)
                    return maxTOI;

                // remaining case: maxTOI is negative, minTOE is positive. => currently overlapping
                if (Mathf.Abs(maxTOI) < Mathf.Abs(minTOE)) // only return collision if going towards deeper overlap!
                    return 0;
                return float.MaxValue;
            }

            return float.MaxValue;
        }

        // cx,cy: corner point of body 1
        // nx,ny: current normal (not necessarily normalized)
        // dx,dy: body vector of body 1 that gives the max depth along this normal
        // d: points of body 2
        // vx,vy: relative velocity of body 2 w. resp. to body 1
        // TOI/TOE: time of impact/exit. Updated when we find better values along this normal.
        //
        // Returns true if the TOI is updated.
        private bool updateImpactExitTime(float cx, float cy, float nx, float ny, float dx, float dy, Vector2[] d, float vx, float vy,
            ref float maxTOI, ref float minTOE) {
            var dot = dy * ny + dx * nx;

            if (dot == 0.0f) dot = 1.0f; // hm

            float t, minT, maxT;

            t = ((d[0].x - cx) * nx + (d[0].y - cy) * ny) / dot;
            maxT = t;
            minT = t;

            t = ((d[1].x - cx) * nx + (d[1].y - cy) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[2].x - cx) * nx + (d[2].y - cy) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            t = ((d[3].x - cx) * nx + (d[3].y - cy) * ny) / dot;
            minT = Mathf.Min(minT, t);
            maxT = Mathf.Max(maxT, t);

            // relative velocity:
            var vp = (vx * nx + vy * ny) / dot;

            if (Mathf.Abs(vp) < 0.0001f) {
                if (minT >= 1 || maxT < 0) {
                    // no overlap in this direction, ever.
                    minTOE = float.MinValue;
                    maxTOI = float.MaxValue;
                    return true;
                }
            } else {
                float TOI, TOE;
                if (vp > 0) {
                    TOI = -maxT / vp;
                    TOE = (1 - minT) / vp;
                } else {
                    TOE = -maxT / vp;
                    TOI = (1 - minT) / vp;
                }

                if (TOE < minTOE) minTOE = TOE;
                if (TOI > maxTOI) {
                    maxTOI = TOI;
                    return true;
                }
            }

            return false;
        }

        public override Collision GetCollisionInfo(Collider other) {
            var penetrationDepth = float.MaxValue;
            var normal = new Vector2();
            var point = new Vector2();
            if (other is BoxCollider) {
                //Console.WriteLine ("\n\n===== Computing collision data:\n");
                var c = _owner.GetExtents();
                if (c == null) return null;
                var d = ((BoxCollider) other)._owner.GetExtents();
                if (d == null) return null;

                //Console.WriteLine ("\nSide vectors of this:\n {0},{1} and {2},{3}",
                //	c[1].x-c[0].x,c[1].y-c[0].y,c[3].x-c[0].x,c[3].y-c[0].y
                //);

                // normals of this vs points of other:
                var nx = -c[0].y + c[1].y;
                var ny = -c[1].x + c[0].x;
                if (!updateCollisionPoint(
                    c[0].x, c[0].y, nx, ny,
                    c[3].x - c[0].x, c[3].y - c[0].y, d,
                    true, ref penetrationDepth, ref normal, ref point))
                    return null;

                nx = c[0].y - c[3].y;
                ny = c[3].x - c[0].x;
                if (!updateCollisionPoint(
                    c[0].x, c[0].y, nx, ny,
                    c[1].x - c[0].x, c[1].y - c[0].y, d,
                    true, ref penetrationDepth, ref normal, ref point))
                    return null;

                //Console.WriteLine ("\nSide vectors of other:\n {0},{1} and {2},{3}",
                //	d[1].x-d[0].x,d[1].y-d[0].y,d[3].x-d[0].x,d[3].y-d[0].y
                //);
                // normals of other vs points of this:
                nx = -d[0].y + d[1].y;
                ny = -d[1].x + d[0].x;
                if (!updateCollisionPoint(
                    d[0].x, d[0].y, nx, ny,
                    d[3].x - d[0].x, d[3].y - d[0].y, c,
                    false, ref penetrationDepth, ref normal, ref point))
                    return null;

                nx = d[0].y - d[3].y;
                ny = d[3].x - d[0].x;
                if (!updateCollisionPoint(
                    d[0].x, d[0].y, nx, ny,
                    d[1].x - d[0].x, d[1].y - d[0].y, c,
                    false, ref penetrationDepth, ref normal, ref point))
                    return null;
                /*
                if (convertToParentSpace && _owner.parent!=null) {
                    normal = _owner.parent.InverseTransformPoint (normal.x, normal.y);
                    float nLen = Mathf.Sqrt (normal.x * normal.x + normal.y * normal.y);
                    normal.x /= nLen;
                    normal.y /= nLen;

                    point = _owner.parent.InverseTransformPoint (point.x, point.y);
                }
                */
                return new Collision(_owner, ((BoxCollider) other)._owner, normal, point, penetrationDepth);
            }

            return null;
        }

        private bool updateCollisionPoint(
            float cx, float cy, float nx, float ny, float dx, float dy, Vector2[] d, bool invertNormal,
            ref float minPenetrationDepth, ref Vector2 normal, ref Vector2 point
        ) {
            var dot = dy * ny + dx * nx;

            if (dot == 0.0f) dot = 1.0f; // hm

            var argMin = new Vector2();
            var argMax = new Vector2();
            var minT = float.MaxValue;
            var maxT = float.MinValue;
            for (var i = 0; i < d.Length; i++) {
                var t = ((d[i].x - cx) * nx + (d[i].y - cy) * ny) / dot;
                if (t < minT) {
                    minT = t;
                    argMin = d[i];
                }

                if (t > maxT) {
                    maxT = t;
                    argMax = d[i];
                }
            }

            // Two cases where no collision:
            if (maxT < 0)
                return false;
            if (minT > 1)
                return false;
            var updateNormal = false;
            var lenD = Mathf.Sqrt(dx * dx + dy * dy);

            //Console.WriteLine ("\n  considering normal: {0},{1}\n  minT, maxT: {2},{3}\n  intersection candidates: {4},{5}",
            //	nx,ny,minT,maxT,(1-minT)*lenD,maxT*lenD
            //);
            if (lenD == 0)
                lenD = 1; // hm
            if (maxT * lenD < minPenetrationDepth) {
                minPenetrationDepth = maxT * lenD;
                updateNormal = true;
                point = argMax;
            }

            if ((1 - minT) * lenD < minPenetrationDepth) {
                minPenetrationDepth = (1 - minT) * lenD;
                updateNormal = true;
                point = argMin;
                invertNormal = !invertNormal;
            }

            if (updateNormal) {
                var len = invertNormal ? -Mathf.Sqrt(nx * nx + ny * ny) : Mathf.Sqrt(nx * nx + ny * ny);
                normal.x = nx / len;
                normal.y = ny / len;

                //Console.WriteLine ("NEW BEST");
            }

            //Console.WriteLine (" (check:) best normal: "+normal);
            return true;
        }
    }
}