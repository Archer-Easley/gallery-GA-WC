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

        public void Solve(List<Point> GUI)
        {
            convertGUIPointsToInternalPoints(GUI);
            generateLineOfSightForReflexVertices();
            minimalGuardSet();
            //performGA();
            //performWOC();
        }

        private void convertGUIPointsToInternalPoints(List<Point> GUI)
        {
            var temp = new CPoint2D[GUI.Count];
            for (var i = 0; i < GUI.Count; i++)
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

        private void generateLineOfSightForReflexVertices()
        {
            for (var i = 0; i < polygon.numberOfVertices; i++)
            {
                if (polygon.PolygonVertexType(polygon[i]).Equals(VertexType.ConvexPoint))
                {
                    reflexVertices.Add(new Vertex((float) polygon[i].X, (float)polygon[i].Y));
                    var temp = polygon.VisibilitySet(polygon[i]);
                    for (var j = 0; j < temp.Count - 1; j++)
                    {
                        reflexVertices[reflexVertices.Count-1].LOS.Add(new Vertex((float)temp[j].X, (float)temp[j].Y));
                    }
                }
            }
        }

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
            for (var i = 0; i < polygon.numberOfVertices-1; i++)
            {
                switch (polygon.PolygonVertexType(polygon[i]))
                {
                    case VertexType.ConvexPoint:
                        if (polygon.PolygonVertexType(polygon.NextPoint(polygon[i])).Equals(VertexType.ConvexPoint))
                        {
                            C.Add(new CPoint2D((polygon[i].X + polygon.NextPoint(polygon[i]).X) / 2, (polygon[i].Y + polygon.NextPoint(polygon[i]).Y) / 2));
                        }
                        break;
                    case VertexType.ConcavePoint:
                        C.Add(polygon[i]);
                        break;
                    case VertexType.ErrorPoint:
                        C.Add(polygon[i]);
                        break;
                }
            }

            while (C.Count > 0)
            {
                var smallestArea = double.MaxValue;
                var smallest = new CPoint2D();
                for (var i = 0; i < C.Count; i++)
                {
                    var temp = new CPolygon(polygon.VisibilitySet(C[i]).ToArray());
                    var tempArea = temp.PolygonArea();
                    if (tempArea < smallestArea)
                    {
                        smallest = C[i];
                        smallestArea = tempArea;
                    }
                }
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
