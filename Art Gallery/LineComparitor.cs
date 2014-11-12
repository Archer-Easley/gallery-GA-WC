using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FormsPolygonGenerator
{
    class LineComparitor
    {
        bool IsOnSegment(float xi, float yi, float xj, float yj,
                        float xk, float yk)
        {
            return (xi <= xk || xj <= xk) && (xk <= xi || xk <= xj) &&
                   (yi <= yk || yj <= yk) && (yk <= yi || yk <= yj);
        }

        int ComputeDirection(float xi, float yi, float xj, float yj,
                                     float xk, float yk)
        {
            float a = (xk - xi) * (yj - yi);
            float b = (xj - xi) * (yk - yi);
            return a < b ? -1 : a > b ? 1 : 0;
        }

        /** Do line segments (x1, y1)--(x2, y2) and (x3, y3)--(x4, y4) intersect? */
        public bool DoLineSegmentsIntersect(float x1, float y1, float x2, float y2,
                                     float x3, float y3, float x4, float y4)
        {
            int d1 = ComputeDirection(x3, y3, x4, y4, x1, y1);
            int d2 = ComputeDirection(x3, y3, x4, y4, x2, y2);
            int d3 = ComputeDirection(x1, y1, x2, y2, x3, y3);
            int d4 = ComputeDirection(x1, y1, x2, y2, x4, y4);
            return (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) &&
                    ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0))) ||
                    (d1 == 0 && IsOnSegment(x3, y3, x4, y4, x1, y1)) ||
                    (d2 == 0 && IsOnSegment(x3, y3, x4, y4, x2, y2)) ||
                    (d3 == 0 && IsOnSegment(x1, y1, x2, y2, x3, y3)) ||
                    (d4 == 0 && IsOnSegment(x1, y1, x2, y2, x4, y4));
            
        }

        /* intersection point of two lines where line 1 is defined by (x1,y1) and (x2,y2) and the other line is defined by (x3,y3) and (x4,y4) */
        public PointF IntersectionPoint(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            PointF point = new PointF();
            float denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (denominator == 0)
            {
                // return empty PointF if lines are parrallel
                return new PointF();
            }
            point.X = ((x1*y2 - y1*x2)*(x3-x4) - (x1-x2)*(x3*y4 - y3*x4))/(denominator);
            point.Y = ((x1*y2 - y1*x2)*(y3-y4) - (y1-y2)*(x3*y4 - y3*x4))/(denominator);
            return point;
        }

        public float distance(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float angleBetween(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            float P12 = distance(x1, y2, x2, y2);
            float P13 = distance(x1, y1, x3, y3);
            float P23 = distance(x2, y2, x3, y3);
            return (float)Math.Acos((Math.Pow(P12, 2) + Math.Pow(P13, 2) - Math.Pow(P23, 2) / (2 * P12 * P13)));
        }
    }
}
