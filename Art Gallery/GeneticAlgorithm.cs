using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPolygonGenerator
{
    class GeneticAlgorithm
    {
        public List<List<Vertex>> population = new List<List<Vertex>>();
        Random r;

        public GeneticAlgorithm(Random rand)
        {
            r = rand; //avoids having multiple instances of the Random class
        }

        private void mutate(List<Vertex> points)
        {
            int rand;
            do
            {
                rand = r.Next(points.Count); //loop until a vertex is found that is guarded
            } while (points[rand].hasGuard == false);

            points[rand].hasGuard = false; //remove the guard

            do
            {
                rand = r.Next(points.Count); //loop until a vertex is found that is not guarded
            } while (points[rand].hasGuard == true);

            points[rand].hasGuard = true; //add a guard to the vertex
        }

        private void crossover(List<Vertex> points1, List<Vertex> points2)
        {
            //create temporary copies
            List<Vertex> temp1 = new List<Vertex>(points1);
            List<Vertex> temp2 = new List<Vertex>(points2);
            List<Vertex> temp1Orig = new List<Vertex>(points1);

            int randPoint = r.Next(temp1.Count);

            //swap items in first copy
            for (int i = 0; i < randPoint; i++)
            {
                if (temp2[i].hasGuard)
                {
                    temp1[i].hasGuard = true;
                }
                else
                {
                    temp1[i].hasGuard = false;
                }
            }

            //swap items in second array
            for (int i = 0; i < randPoint; i++)
            {
                if (temp1Orig[i].hasGuard)
                {
                    temp2[i].hasGuard = true;
                }
                else
                {
                    temp2[i].hasGuard = false;
                }
            }

            population.Add(temp1);
            population.Add(temp2);
        }

        public void performGA()
        {
            //TODO: ADD IN CALLS TO MUTATE AND CROSSOVER FUNCTIONS DETERMINED BY RANDOM CHANCES
        }
    }
}
