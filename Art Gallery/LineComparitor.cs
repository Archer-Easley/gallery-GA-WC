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
