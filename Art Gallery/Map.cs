using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
        CPolygon polygon;

        public Map()
        {
            vertices = new List<Vertex>();
        }

        public void Solve(List<Point> GUI, int generationCount, int populationCount)
        {
            convertGUIPointsToInternalPoints(GUI);
            minimalGuardSet();
            createPopulation(populationCount);
            performGA(generationCount);
            performWOC();
        }

        private void convertGUIPointsToInternalPoints(List<Point> GUI)
        {
            var temp = new CPoint2D[GUI.Count - 1];
            for (var i = 0; i < GUI.Count -1; i++)
            {
                temp[i] = new CPoint2D(GUI[i].X, GUI[i].Y);
            }
            polygon = new CPolygon(temp);
            //Vertex temp;
            //for (int i = 0; i < GUI.Count; i++)
            //{
            //    temp = new Vertex();
            //    temp.x = GUI[i].X;
            //    temp.y = GUI[i].Y;
            //    temp.ID = i;
            //    vertices.Add(temp);
            //}
            //vertices.ToString();
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
                foreach (CPoint2D p in polygon.m_aVertices)
                {
                    if (polygon.PolygonVertexType(p).Equals(VertexType.ConcavePoint))
                    {
                        temp = new Vertex();
                        temp.x = (float)p.X;
                        temp.y = (float)p.Y;
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

        //private void generateLineOfSightForReflexVertices()
        //{
        //    for (var i = 0; i < polygon.numberOfVertices; i++)
        //    {
        //        if (polygon.PolygonVertexType(polygon[i]).Equals(VertexType.ConcavePoint))
        //        {
        //            reflexVertices.Add(new Vertex((float) polygon[i].X, (float)polygon[i].Y));
        //            var temp = polygon.VisibilitySet(polygon[i]);
        //            for (var j = 0; j < temp.Count - 1; j++)
        //            {
        //                reflexVertices[reflexVertices.Count-1].LOS.Add(new Vertex((float)temp[j].X, (float)temp[j].Y));
        //            }
        //        }
        //    }
        //}

        //private void identifyReflexVertices()
        //{
        //    LineComparitor comp = new LineComparitor();
        //    for (var i = 0; i < vertices.Count; i++)
        //    {
        //        float theta = 0;
        //        for (var j = 0; j < vertices[i].LOS.Count; j++)
        //        {
        //            theta += comp.angleBetween(vertices[i].x, vertices[i].y, vertices[i].LOS[j].x, vertices[i].LOS[j].y, vertices[i].LOS[(j + 1) * vertices[i].LOS.Count].x, vertices[i].LOS[(j + 1) * vertices[i].LOS.Count].y);
        //        }
        //        if (theta >= Math.PI)
        //        {
        //            reflexVertices.Add(vertices[i]);
        //        }
        //    }
        //}

        //private void populateLOSforReflexVertices()
        //{
        //    LineComparitor comp = new LineComparitor();
        //    for (var i = 0; i < vertices.Count; i++)
        //    {
        //        vertices[i].LOS.Add(vertices[i]);
        //        vertices[i].LOS.Add(vertices[(i + 1) % vertices.Count]);
        //        vertices[i].LOS.Add(vertices[Math.Abs((i - 1) % vertices.Count)]);
        //        for (var j = 0; j < i; j++)
        //        {
        //            if (!vertices[i].LOS.Contains(vertices[j]))
        //            {
        //                for (var k = 0; k < vertices.Count; k++)
        //                {
        //                    var blocked = false;
        //                    if ((i != k && j != k) && !blocked)
        //                    {
        //                        if (comp.DoLineSegmentsIntersect(vertices[i].x, vertices[i].y, vertices[j].x, vertices[j].y, vertices[k].x, vertices[k].y, vertices[(k + 1) % vertices.Count].x, vertices[(k + 1) % vertices.Count].y))
        //                        {
        //                            blocked = true;
        //                        }
        //                    }
        //                    if (!blocked)
        //                    {
        //                        vertices[i].LOS.Add(vertices[j]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void calculateMinimumVisibilityIndependentSet()
        //{
        //    List<Vertex> convexVertices = new List<Vertex>(vertices.Except(reflexVertices));
        //    List<Vertex> midpointVerticesOfConsecutiveReflexEdges = new List<Vertex>();
        //    for (var i = 0; i < vertices.Count;i++)
        //    {
        //        if (reflexVertices.Contains(vertices[i]) && reflexVertices.Contains(vertices[(i+1)%vertices.Count]))
        //        {
        //            var x = (vertices[i].x + vertices[(i+1)%vertices.Count].x)/2;
        //            var y = (vertices[i].y + vertices[(i+1)%vertices.Count].y)/2;
        //            midpointVerticesOfConsecutiveReflexEdges.Add(new Vertex(x,y));
        //        }
        //    }
        //    List<Vertex> C = new List<Vertex>(convexVertices.Union(midpointVerticesOfConsecutiveReflexEdges));
        //    List<Vertex> miniumGuardSet = new List<Vertex>();
        //    for (var i = 0; i < C.Count; i++)
        //    {
        //        // determine coverage area
        //    }
        //    while (C.Count > 0)
        //    {
        //        // add smallest area vertex to minimumGuardSet
        //        // remove that vertex from C
        //        // remove any other vertices that cover any of the same area that is already in the minimum set
        //    }
        //}

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

        private void performGA(int generationCount)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm(r, population, generationCount, polygon);
            ga.performGA();
            this.population = new List<List<Vertex>>(ga.population);
        }

        private void performWOC()
        {
            WisdomOfCrowds woc = new WisdomOfCrowds(population, vertices.Count, polygon);
            woc.initializeAgreementList();
            woc.populateAgreementList();
            woc.createWOCSolution();
            this.population = woc.population;
            r.Next();
        }
    }
}
