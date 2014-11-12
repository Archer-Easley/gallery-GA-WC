using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FormsPolygonGenerator
{
    public class Vertex : IComparable<Vertex>
    {
        PointF location = new PointF();
        public List<Vertex> LOS = new List<Vertex>();
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
            LOS = new List<Vertex>(v.LOS);
            x = v.x;
            y = v.y;
            ID = v.ID;
            hasGuard = v.hasGuard;
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
