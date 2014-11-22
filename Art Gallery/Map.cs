using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    class Map
    {
        List<Vertex> vertices;
        List<List<Vertex>> population;
        public Random r = new Random();
        List<Vertex> reflexVertices = new List<Vertex>();
        //List<Vertex> minimumGaurdSet;
        List<CPoint2D> minimumGuardSet = new List<CPoint2D>();
        public List<Vertex> WoCSolution;
        public List<Vertex> GASolution;
        public string GAtime;
        public string WOCtime;
        public string GAavgFitness;
        CPolygon polygon;

        public Map()
        {
            vertices = new List<Vertex>();
        }

        public void Solve(List<Point> GUI, int generationCount, int populationCount)
        {
            convertGUIPointsToInternalPoints(GUI);
            //minimalGuardSet();
            //createPopulation(populationCount);
            if(countReflexVertices())
            {
                performGA(generationCount, populationCount);
                performWOC();
            }
        }

        private bool countReflexVertices()
        {
            List<Vertex> reflexVertices = new List<Vertex>(); 

            foreach(CPoint2D p in polygon.m_aVertices)
            {
                if (polygon.PolygonVertexType(p).Equals(VertexType.ConcavePoint))
                    reflexVertices.Add(new Vertex((float)p.X, (float)p.Y));
            }

            if(reflexVertices.Count == 0) //shape is completely convex
            {
                WoCSolution = new List<Vertex>();
                GASolution = new List<Vertex>();
                WoCSolution.Add(new Vertex((float)polygon[0].X, (float)polygon[0].Y));
                GASolution.Add(new Vertex((float)polygon[0].X, (float)polygon[0].Y));
                return false;
            }
            else if(reflexVertices.Count == 1)
            {
                WoCSolution = new List<Vertex>(reflexVertices);
                GASolution = new List<Vertex>(reflexVertices);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void convertGUIPointsToInternalPoints(List<Point> GUI)
        {
            var temp = new CPoint2D[GUI.Count];
            for (var i = 0; i < GUI.Count; i++)
            {
                temp[i] = new CPoint2D(GUI[i].X, GUI[i].Y);
            }
            polygon = new CPolygon(temp);
        }

        private void createPopulation(int populationCount)
        {
            //populate first member of the population 
            population = new List<List<Vertex>>();
            List<Vertex> tempList;
            Vertex temp;
            for (int i = 0; i < populationCount; i++)
            {
                tempList = new List<Vertex>();
                for (int j = 0; j < polygon.m_aVertices.Length; j++)//(CPoint2D p in polygon.m_aVertices)
                {
                    if (polygon.PolygonVertexType(polygon.m_aVertices[j]).Equals(VertexType.ConcavePoint))
                    {
                        temp = new Vertex();
                        temp.x = (float)polygon.m_aVertices[j].X;
                        temp.y = (float)polygon.m_aVertices[j].Y;
                        temp.ID = j;
                        tempList.Add(temp);
                    }
                }
                population.Add(tempList);
            }

            int lowerBound = minimumGuardSet.Count;
            int upperBound = population[0].Count;

            int randGuardCount = r.Next(lowerBound, upperBound);
            int randIndex = 0;

            foreach(List<Vertex> lv in population)
            {
                for(int i = 0; i < randGuardCount; i++)
                {
                    do
                    {
                        randIndex = r.Next(lv.Count);
                    } while (lv[randIndex].hasGuard); //loop until unguarded vertex is found

                    lv[randIndex].hasGuard = true;
                }

                randGuardCount = r.Next(lowerBound, upperBound);
            }

            foreach(Vertex v in population[0])
            {
                v.getLOS(polygon);
            }

            for (int i = 1; i < population.Count; i++)
            {
                for(int j = 0; j < population[0].Count; j++)
                {
                    population[i][j].LOS = population[0][j].LOS;
                }
            }
        }

        private void minimalGuardSet()
        {
            List<CPoint2D> C = new List<CPoint2D>();
            for (var i = 0; i < polygon.numberOfVertices; i++)
            {
                switch (polygon.PolygonVertexType(polygon[i]))
                {
                    case VertexType.ConcavePoint:
                        if (polygon.PolygonVertexType(polygon.NextPoint(polygon[i])).Equals(VertexType.ConcavePoint))
                        {
                            C.Add(new CPoint2D((polygon[i].X + polygon.NextPoint(polygon[i]).X) / 2, (polygon[i].Y + polygon.NextPoint(polygon[i]).Y) / 2));
                        }
                        break;
                    case VertexType.ConvexPoint:
                        C.Add(polygon[i]);
                        break;
                    case VertexType.ErrorPoint:
                        break;
                }
            }

            while (C.Count > 0)
            {
                var smallestArea = double.MaxValue;
                var smallest = new CPoint2D();
                var smallestPolygon = new CPolygon();
                var runningPolygon = new CPolygon();
                for (var i = 0; i < C.Count; i++)
                {
                    var temp = new CPolygon(polygon.VisibilitySet(C[i]).ToArray());
                    var tempArea = temp.PolygonArea();
                    if (tempArea < smallestArea)
                    {
                        smallest = C[i];
                        smallestArea = tempArea;
                        smallestPolygon = temp;
                    }
                }
                //runningPolygon = new CPolygon(polygon.JoinPolygons(runningPolygon, smallestPolygon));
                minimumGuardSet.Add(smallest);
                C.Remove(smallest);
                for (var i = 0; i < C.Count; i++)
                {
                    var temp = new CPolygon(polygon.VisibilitySet(C[i]).ToArray());
                    if (CPolygon.IntersectedWith(new CPolygon(polygon.VisibilitySet(smallest).ToArray()), temp, false))
                    {
                        C.Remove(C[i]);
                        i--;
                    }
                }
            }
        }

        private void performGA(int generationCount, int populationCount)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm(r, population, generationCount, polygon, populationCount);
            ga.performGA();
            this.GASolution = new List<Vertex>(ga.population[0]);
            GAtime = ga.timeElapsed;
            GAavgFitness = ga.averageFitness.ToString("#.###");
            this.population = new List<List<Vertex>>(ga.population);
        }

        private void performWOC()
        {
            WisdomOfCrowds woc = new WisdomOfCrowds(population, vertices.Count, polygon);
            Stopwatch t = new Stopwatch();
            t.Reset();
            t.Start();
            woc.initializeAgreementList();
            woc.populateAgreementList();
            woc.createWOCSolution();
            t.Stop();
            this.WoCSolution = new List<Vertex>(woc.guardVertex);
            WOCtime = t.Elapsed.ToString();
        }
    }
}
