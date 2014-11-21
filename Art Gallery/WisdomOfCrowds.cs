using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    class WisdomOfCrowds
    {
        public List<List<Vertex>> population;
        private List<Agreement> agreementList;
        private List<Vertex> guardVertex = new List<Vertex>();
        private CPolygon map;
        private int totalVerticesCount;

        public WisdomOfCrowds(List<List<Vertex>> pop, int vertCount, CPolygon tempMap)
        {
            population = new List<List<Vertex>>(pop);
            totalVerticesCount = vertCount;
            map = tempMap;
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
            agreementList.OrderByDescending(x => x.vert.polygonArea).ThenByDescending(x => x.freq);

            //add first vertex and create unionable polygon
            guardVertex.Add(agreementList[0].vert);
            CPolygon unionSet = new CPolygon(agreementList[0].vert.LOS.m_aVertices.ToArray());

            CPolygon temp, biggest = new CPolygon();
            double biggestArea = double.MinValue;
            double totalArea = map.PolygonArea();
            double tempArea = double.MinValue;
            double blahArea = 0;
            int index = 0;

            //add guards until whole area is complete
            while((blahArea = Math.Abs(unionSet.PolygonArea() - totalArea)) > ConstantValue.SmallValue) //allows no fp precision issues
            {
                biggestArea = double.MinValue;

                //get point that will yield the most area if added
                for(int i = 0; i < agreementList.Count; i++)
                {
                    temp = new CPolygon(map.JoinPolygon(agreementList[i].vert.LOS, unionSet).ToArray());
                    if((tempArea = temp.PolygonArea()) > biggestArea)
                    {
                        biggest = new CPolygon(temp.m_aVertices.ToArray());
                        biggestArea = tempArea;
                        index = i;
                    }
                }
                //add that point's LOS to unionSet polygon
                unionSet = new CPolygon(map.JoinPolygon(unionSet, biggest).ToArray());

                //add point to the guard vertex list
                guardVertex.Add(agreementList[index].vert);
            }
        }
    }
}
