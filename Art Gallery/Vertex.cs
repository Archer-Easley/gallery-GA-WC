using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    public class Vertex : IComparable<Vertex>
    {
        PointF location = new PointF();
        public CPolygon LOS;
        public double polygonArea;
        public int ID;
        public bool hasGuard = false;
        public float x
        {
            get
            {
                return location.X;
            }
            set
            {
                location.X = value;
            }
        }
        public float y
        {
            get
            {
                return location.Y;
            }
            set
            {
                location.Y = value;
            }
        }

        public Vertex()
        {

        }

        public Vertex(float x, float y)
        {
            location.X = x;
            location.Y = y;
        }

        public Vertex(Vertex v)
        {
            location = v.location;
            x = v.x;
            y = v.y;
            ID = v.ID;
            hasGuard = v.hasGuard;
        }

        public void getLOS(CPolygon map)
        {
            CPoint2D tempPoint = new CPoint2D();
            tempPoint.X = this.x;
            tempPoint.Y = this.y;
            CPolygon ret = new CPolygon(map.VisibilitySet(tempPoint).ToArray());
            this.LOS = ret;
            this.polygonArea = ret.PolygonArea();
        }

        // way to allow vertices to be compared
        public int CompareTo(Vertex v)
        {
            return this.ID.CompareTo(v.ID);
        }

        public override bool Equals(object v)
        {
            Vertex ver = v as Vertex;

            return ver.ID == this.ID;

            //if (this.ID == v.ID)
            //    return true;
            //else
            //    return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
