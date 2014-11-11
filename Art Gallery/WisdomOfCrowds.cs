using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPolygonGenerator
{
    class WisdomOfCrowds
    {
        public List<List<Vertex>> population;
        public List<Agreement> agreementList;
        public List<Vertex> guardVertex = new List<Vertex>();
        public List<Vertex> coveredVertices = new List<Vertex>();
        private int totalVerticesCount;

        public WisdomOfCrowds(List<List<Vertex>> pop, int vertCount)
        {
            population = new List<List<Vertex>>(pop);
            totalVerticesCount = vertCount;
        }

        public void initializeAgreementList()
        {
            agreementList = new List<Agreement>();
            Agreement temp;
            foreach(Vertex v in population[0])
            {
                temp = new Agreement();
                temp.vert = new Vertex(v);
                temp.freq = 0;
                agreementList.Add(temp);
            }
        }

        public void populateAgreementList()
        {
            foreach(List<Vertex> lv in population)
            {
                for(int i = 0; i < lv.Count; i++)
                {
                    if (lv[i].hasGuard)
                        agreementList[i].freq++;
                }
            }
        }

        public void createWOCSolution()
        {
            //sort population by descending LOS count, frequency of vertex in the population
            agreementList.OrderByDescending(x => x.vert.LOS.Count).ThenByDescending(x => x.freq);

            //add first vertex
            guardVertex.Add(agreementList[0].vert);

            //add all vertices to a list of viewed vertices
            foreach(Vertex v in guardVertex[0].LOS)
            {
                coveredVertices.Add(v);
            }

            int maxAdded = 0;
            List<Vertex> unionSet = new List<Vertex>();
            //add successive vertices
            while(coveredVertices.Count != totalVerticesCount)
            {
                Vertex temp = new Vertex();
                //find vertex that will add the most vertices to the coveredVertices list
                for(int i = 0; i < agreementList.Count; i++)
                {
                    unionSet = coveredVertices.Union(agreementList[i].vert.LOS).ToList();
                    if(unionSet.Count > maxAdded)
                    {
                        maxAdded = unionSet.Count;
                        temp = new Vertex(agreementList[i].vert);
                    }
                    unionSet.Clear();
                }
                coveredVertices = coveredVertices.Union(temp.LOS).ToList();
                guardVertex.Add(temp);
            }
        }
    }
}
