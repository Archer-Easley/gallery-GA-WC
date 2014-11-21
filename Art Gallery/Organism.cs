using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    class Organism
    {
        public List<Vertex> vertexList;
        public int fitness = 0;
        public CPolygon unionedPolygon;
        public double polygonArea;

        public Organism(List<Vertex> list)
        {
            vertexList = new List<Vertex>(list);
        }

        public Organism(Organism temp)
        {
            vertexList = new List<Vertex>(temp.vertexList);
            fitness = temp.fitness;
        }

        public Organism()
        {
            vertexList = new List<Vertex>();
        }

        public void determineFitness()
        {
            fitness = 0;
            foreach(Vertex v in vertexList)
            {
                if (v.hasGuard == true)
                    fitness++;
            }
        }

        public void getUnionedPolygon(CPolygon map)
        {
            bool firstGuard = true;
            CPolygon temp = new CPolygon();
            foreach(Vertex v in vertexList)
            {
                if(v.hasGuard && firstGuard)
                {
                    temp = new CPolygon(v.LOS.m_aVertices);
                }
                else if(v.hasGuard)
                {
                    temp = new CPolygon(map.JoinPolygon(temp, v.LOS).ToArray());
                }
            }
            unionedPolygon = temp;
            polygonArea = temp.PolygonArea();
        }
    }
}
