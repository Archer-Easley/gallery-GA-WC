using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPolygonGenerator
{
    class Organism
    {
        public List<Vertex> vertexList;
        public int fitness = 0;

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
    }
}
