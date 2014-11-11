using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public float distance(float xi, float yi, float xj, float yj)
        {
            var dx = xj - xi;
            var dy = yj - yi;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float angleBetween(float xi, float yi, float xj, float yj, float xk, float yk)
        {
            var P12 = distance(xi, yi, xj, yj);
            var P13 = distance(xi,yi,xk,yk);
            var P23 = distance(xj,yj,xk,yk);
            return (float)Math.Acos((Math.Pow(P12, 2)+Math.Pow(P13,2)-Math.Pow(P23,2))/(2*P12*P13));
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
    }
}
