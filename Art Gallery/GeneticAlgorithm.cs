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

        }

        public void performGA()
        {
            //TODO: ADD IN CALLS TO MUTATE AND CROSSOVER FUNCTIONS DETERMINED BY RANDOM CHANCES
        }
    }
}
