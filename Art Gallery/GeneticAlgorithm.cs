using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public GeneticAlgorithm(Random rand, List<List<Vertex>> pop, int tempGenCount, CPolygon masterMap)
        {
            r = rand; //avoids having multiple instances of the Random class
            Organism temp;
            foreach(List<Vertex> lv in pop)
            {
                temp = new Organism(lv);
                popOrg.Add(temp);
            }
            this.generationCount = tempGenCount;
            this.map = masterMap;
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

            popOrg.Add(temp1);
            popOrg.Add(temp2);
        }

        public void performGA()
        {
            int randMutationIndex;
            int randCrossoverIndex1;
            int randCrossoverIndex2;
            int randNumCrossovers = r.Next(popOrg.Count);
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

                //join polygons
                foreach(Organism o in popOrg)
                {
                    o.getUnionedPolygon(map);
                }

                //sort by fitness
                popOrg.OrderByDescending(x => x.polygonArea).ThenBy(x => x.fitness);

                //remove least fit
                popOrg.RemoveRange(origPopCount, popOrg.Count - origPopCount);
            }

            //populate population in form of List<List<Vertex>> to pass to presenter
            foreach(Organism o in popOrg)
            {
                population.Add(o.vertexList);
            }
        }
    }
}
