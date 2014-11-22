using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GeometryUtility;

namespace FormsPolygonGenerator
{
    class GeneticAlgorithm
    {
        public List<Organism> popOrg = new List<Organism>();
        public List<List<Vertex>> population = new List<List<Vertex>>();
        Random r;
        int generationCount = 100;
        private CPolygon map;
        private double mapSize;
        public string timeElapsed;
        public double averageFitness;
        private double populationCount;

        public GeneticAlgorithm(Random rand, List<List<Vertex>> pop, int tempGenCount, CPolygon masterMap, int populationCount)
        {
            r = rand; //avoids having multiple instances of the Random class
            //Organism temp;
            //foreach(List<Vertex> lv in pop)
            //{
            //    temp = new Organism(lv);
            //    popOrg.Add(temp);
            //}
            this.generationCount = tempGenCount;
            this.map = masterMap;
            this.populationCount = populationCount;
        }

        private void mutate(Organism points)
        {
            int rand1, rand2;
            do
            {
                rand1 = r.Next(points.vertexList.Count); //loop until a vertex is found that is guarded
            } while (points.vertexList[rand1].hasGuard == false);

            points.vertexList[rand1].hasGuard = false; //remove the guard

            do
            {
                rand2 = r.Next(points.vertexList.Count); //loop until a vertex is found that is not guarded
            } while (points.vertexList[rand2].hasGuard == true && rand1 != rand2);

            points.vertexList[rand2].hasGuard = true; //add a guard to the vertex

            //no need to evaluate fitness; will only ever add a guard, then subtract a guard, giving the same fitness
            points.getUnionedPolygon(map);
        }

        private void crossover(Organism points1, Organism points2)
        {
            //create temporary copies
            Organism temp1 = new Organism(points1);
            Organism temp2 = new Organism(points2);
            Organism temp1Orig = new Organism(points1);

            int randPoint = r.Next(temp1.vertexList.Count);

            //swap items in first copy
            for (int i = 0; i < randPoint; i++)
            {
                if (temp2.vertexList[i].hasGuard)
                {
                    temp1.vertexList[i].hasGuard = true;
                }
                else
                {
                    temp1.vertexList[i].hasGuard = false;
                }
            }

            //swap items in second array
            for (int i = 0; i < randPoint; i++)
            {
                if (temp1Orig.vertexList[i].hasGuard)
                {
                    temp2.vertexList[i].hasGuard = true;
                }
                else
                {
                    temp2.vertexList[i].hasGuard = false;
                }
            }

            temp1.determineFitness();
            temp2.determineFitness();

            temp1.getUnionedPolygon(map);
            temp2.getUnionedPolygon(map);

            popOrg.Add(temp1);
            popOrg.Add(temp2);
        }

        private void generatePopulation()
        {
            //populate list of vertices for organism
            List<Vertex> tempList;
            Vertex temp;
            mapSize = map.PolygonArea();
            int randIndex;
            for (int i = 0; i < populationCount; i++)
            {
                tempList = new List<Vertex>();
                for (int j = 0; j < map.m_aVertices.Length; j++)//(CPoint2D p in polygon.m_aVertices)
                {
                    if (map.PolygonVertexType(map.m_aVertices[j]).Equals(VertexType.ConcavePoint))
                    {
                        temp = new Vertex();
                        temp.x = (float)map.m_aVertices[j].X;
                        temp.y = (float)map.m_aVertices[j].Y;
                        temp.ID = j;
                        tempList.Add(temp);
                    }
                }
                popOrg.Add(new Organism(tempList));
            }

            //populate LOS for a vertex only once, then copy to other vertices
            foreach(Vertex v in popOrg[0].vertexList)
            {
                v.getLOS(map);
            }

            for (int i = 1; i < popOrg.Count; i++)
            {
                for(int j = 0; j < popOrg[0].vertexList.Count; j++)
                {
                    popOrg[i].vertexList[j].LOS = popOrg[0].vertexList[j].LOS;
                }
            }

            //for each member of the population, add guards until solution is valid
            double blahArea;
            for (int i = 0; i < popOrg.Count; i++)
            {
                while ((blahArea = Math.Round(popOrg[i].polygonArea)) < Math.Round(mapSize))
                {
                    do
                    {
                        randIndex = r.Next(popOrg[i].vertexList.Count);
                    } while (popOrg[i].vertexList[randIndex].hasGuard == true);
                    popOrg[i].vertexList[randIndex].hasGuard = true;
                    popOrg[i].getUnionedPolygon(map);
                }
            }
            //for (int i = 0; i < popOrg.Count; i++)
            //{
            //    //get first guard to prevent unionedPolygon from being null
            //    randIndex = r.Next(popOrg[i].vertexList.Count);
            //    popOrg[i].vertexList[randIndex].hasGuard = true;
            //    popOrg[i].getUnionedPolygon(map);
            //    double maxArea = popOrg[i].polygonArea;
            //    int maxIndex = -1;
            //    double blahArea;

            //    //for each Organism, set random guards until solution is valid
            //    while ((blahArea = Math.Round(popOrg[i].unionedPolygon.PolygonArea())) < Math.Round(mapSize))
            //    {
            //        Organism tempMax = new Organism(popOrg[i]);
            //        //find vertex that when given a guard, adds the most area to the shape
            //        for (int j = 0; j < tempMax.vertexList.Count; j++)
            //        {
            //            if (j == randIndex)
            //                j++; //make sure to not count initial guard
            //            if (j == tempMax.vertexList.Count)
            //                break; //if initial guard was last item in the list, break from the loop
            //            tempMax.vertexList[j].hasGuard = true;
            //            tempMax.getUnionedPolygon(map);
            //            tempMax.vertexList[j].hasGuard = false; //reset to not throw off values from 
            //            if (tempMax.polygonArea > maxArea)
            //            {
            //                maxArea = tempMax.unionedPolygon.PolygonArea();
            //                maxIndex = j;
            //            }
            //        }
            //        //puts biggest area coverage guard at index
            //        popOrg[i].vertexList[maxIndex].hasGuard = true;
            //        popOrg[i].getUnionedPolygon(map);
            //    }
            //}
        }

        public void performGA()
        {
            int randMutationIndex;
            int randCrossoverIndex1;
            int randCrossoverIndex2;
            int randNumCrossovers = r.Next(popOrg.Count);

            Stopwatch t = new Stopwatch();
            t.Reset();
            t.Start();
            generatePopulation();
            int origPopCount = popOrg.Count;

            //initialize fitness for each organism
            foreach(Organism o in popOrg)
            {
                o.determineFitness();
            }

            //iterate through generations
            for (int j = 0; j < generationCount; j++)
            {
                randNumCrossovers = r.Next(popOrg.Count);
                //perform crossovers
                for (int i = 0; i < randNumCrossovers; i++)
                {
                    do //loop insures indexes are different
                    {
                        randCrossoverIndex1 = r.Next(popOrg.Count);
                        randCrossoverIndex2 = r.Next(popOrg.Count);
                    } while (randCrossoverIndex1 == randCrossoverIndex2);

                    crossover(popOrg[randCrossoverIndex1], popOrg[randCrossoverIndex2]);
                }

                //perform mutations
                if (r.NextDouble() < .1)
                {
                    randMutationIndex = r.Next(popOrg.Count);
                    mutate(popOrg[randMutationIndex]);
                }

                //join polygons and determine fitness
                foreach(Organism o in popOrg)
                {
                    o.getUnionedPolygon(map);
                    o.determineFitness();
                }

                //sort by fitness
                //popOrg = new List<Organism>(popOrg.OrderByDescending(x => x.polygonArea).ThenBy(x => x.fitness).ToList());
                popOrg.Sort(delegate(Organism o1, Organism o2)
                {
                    return o1.fitness.CompareTo(o2.fitness);
                });
                popOrg.Reverse();

                //remove least fit
                popOrg.RemoveRange(origPopCount, popOrg.Count - origPopCount);
            }

            //remove invalid answers, then sort by just fitness
            mapSize = Math.Round(map.PolygonArea());
            for (int i = 0; i < popOrg.Count; i++)
            {
                if (mapSize != Math.Round(popOrg[i].polygonArea))
                {
                    popOrg.RemoveAt(i);
                }
            }
            
            //popOrg = new List<Organism>(popOrg.OrderBy(x => x.fitness).ToList());
            popOrg.Sort(delegate(Organism o1, Organism o2)
            {
                return o1.fitness.CompareTo(o2.fitness);
            });
            popOrg.Reverse();

            t.Stop();
            timeElapsed = t.Elapsed.ToString();

            //populate population in form of List<List<Vertex>> to pass to presenter, and calculate average fitness
            for (int i = 0; i < popOrg.Count; i++)
            {
                population.Add(popOrg[i].vertexList);
                averageFitness += popOrg[i].fitness;
            }
            averageFitness = averageFitness / popOrg.Count;
        }
    }
}
