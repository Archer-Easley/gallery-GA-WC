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
        List<List<Vertex>> population;
        public Random r = new Random();
        List<Vertex> reflexVertices;

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
            LineComparitor comp = new LineComparitor();
            for (var i = 0; i < vertices.Count; i++)
            {
                float theta = 0;
                vertices[i].LOS.OrderBy(x => x.ID);
                for (var j = 0; j < vertices[i].LOS.Count; j++)
                {
                    theta += comp.angleBetween(vertices[i].x, vertices[i].y, vertices[i].LOS[j].x, vertices[i].LOS[j].y, vertices[i].LOS[(j + 1) % vertices[i].LOS.Count].x, vertices[i].LOS[(j + 1) % vertices[i].LOS.Count].y);
                }
                if (theta >= Math.PI)
                {
                    reflexVertices.Add(vertices[i]);
                }
            }
        }

        private void populateLOSforReflexVertices()
        {
            LineComparitor comp = new LineComparitor();
            for (var i = 0; i < vertices.Count; i++)
            {
                vertices[i].LOS.Add(vertices[i]);
                for (var j = 0; j < i; j++)
                {
                    for (var k = 0; k < vertices.Count; k++)
                    {
                        if (!vertices[i].LOS.Contains(vertices[j])){
                            var blocked = false;
                            if ((i != k) && (j != k) && (!blocked))
                            {
                                if(comp.DoLineSegmentsIntersect(vertices[i].x,vertices[i].y,vertices[j].x,vertices[j].y,vertices[k].x,vertices[k].y,vertices[(k+1)%vertices.Count].x,vertices[(k+1)%vertices.Count].y))
                                {
                                    blocked = true;
                                }
                            }
                            if (!blocked)
                            {
                                vertices[i].LOS.Add(vertices[j]);
                            }
                        }
                    }
                }
            }
        }

        private void performGA()
        {
            GeneticAlgorithm ga = new GeneticAlgorithm(r);
            ga.performGA();
            this.population = new List<List<Vertex>>(ga.population);
        }

        private void performWOC()
        {
            WisdomOfCrowds woc = new WisdomOfCrowds(population, vertices.Count);
            woc.initializeAgreementList();
            woc.populateAgreementList();
            woc.createWOCSolution();
        }
    }
}
