using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FormsPolygonGenerator
{
    class Vertex
    {
        PointF location;
        public List<Vertex> LOS;
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
        }

    }
}
