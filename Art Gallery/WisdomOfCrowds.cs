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

        public WisdomOfCrowds(List<List<Vertex>> pop)
        {
            population = new List<List<Vertex>>(pop);
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
                foreach(Vertex v in lv)
                {
                    
                }
            }
        }

        public void createWOCSolution()
        {

        }
    }
}
