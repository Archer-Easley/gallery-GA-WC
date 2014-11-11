using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FormsPolygonGenerator
{
    class Map
    {
        List<Vertex> vertices;

        public Map()
        {
            vertices = new List<Vertex>();
        }

        public void Solve(List<Point> GUI)
        {
            convertGUIPointsToInternalPoints(GUI);
            identifyReflexVertices();
            populateLOSforReflexVertices();
            performGA();
            performWOC();
        }

        private void convertGUIPointsToInternalPoints(List<Point> GUI)
        {
            Vertex temp;
            for (int i = 0; i < GUI.Count; i++)
            {
                temp = new Vertex();
                temp.x = GUI[i].X;
                temp.y = GUI[i].Y;
                temp.ID = i;
                vertices.Add(temp);
            }
            vertices.ToString();
        }

        private void identifyReflexVertices()
        {

        }

        private void populateLOSforReflexVertices()
        {

        }

        private void performGA()
        {

        }

        private void performWOC()
        {
            WisdomOfCrowds woc = new WisdomOfCrowds();
            woc.initializeAgreementList();
            woc.populateAgreementList();
            woc.createWOCSolution();
        }

    }
}
