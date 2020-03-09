using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp
{
    public static class Algo
    {
        public static Random random = new Random();
        public static List<Generation> Launch(List<City> LIST_CITIES, int populationNumber,
            int mutationsPercentage, int xoverPercentage, int elitesPercentage)
        {
            List<Generation> generations = new List<Generation>();
            var firstGen = new Generation(GenerateRandomRoads(LIST_CITIES, populationNumber));
            generations.Add(firstGen);

            while (!CheckStopCondition(new List<Generation>(generations)))
            {
                generations.Add(GetNextGeneration(generations.Last(), mutationsPercentage, xoverPercentage, elitesPercentage));
            }
            return generations;
        }

        private static bool CheckStopCondition(List<Generation> generations)
        {
            Boolean ret = false;
            if(generations.Count > generations[0].Roads.Count)
            {
                for(int i = generations.Count - 1; i > generations.Count - generations[0].Roads.Count; i--)
                {
                    if(generations[i].GetBestScore() != generations.Last().GetBestScore())
                    {
                        ret = true;
                        break;
                    }
                }
                ret = true;
            }
            return ret;
        }

        private static Generation GetNextGeneration(Generation genN,
            int mutationsPercentage, int xoverPercentage, int elitesPercentage)
        {
            var xovers = Algo.GenerateXOver(genN, xoverPercentage);
            var mutations = Algo.GenerateMutations(genN, mutationsPercentage);
            var elites = genN.GetBestRoad(elitesPercentage);

            var genPrimeRoad = new List<Road>(mutations);
            genPrimeRoad.AddRange(elites);
            genPrimeRoad.AddRange(xovers);

            var genPrime = new Generation(genPrimeRoad).GetBestRoad(genN.Roads.Count);
            return new Generation(genPrime);
        }

        private static int PositionPivot(Road r1, Road r2)
        {
            int pivot = 1;
            for(int i = 0; i < r1.ListCities.Count - 1; i++)
            {
                if(r1.ListCities[i] != r2.ListCities[i])
                {
                    pivot = i;
                    break;
                }
            }
            return pivot;
        }

        private static List<Road> GenerateRandomRoads(List<City> cities, int roadNumber)
        {
            List<Road> roads = new List<Road>();
            while (roads.Count < roadNumber)
            {
                Road newRoad = new Road(cities.OrderBy(a => Guid.NewGuid()).ToList());
                if(!roads.Contains(newRoad))
                {
                    roads.Add(newRoad);
                }
            }
            return roads;
        }

        private static List<Road> GenerateXOver(Generation gen, int xOverNumber)
        {
            List<Road> roads = new List<Road>();
            while(roads.Count < xOverNumber)
            {
                int random1 = random.Next(gen.Roads.Count);
                int random2 = random.Next(gen.Roads.Count);
                
                while (random2 == random1)
                {
                    random2 = random.Next(gen.Roads.Count);
                }

                int pivotStart = PositionPivot(gen.Roads[random1], gen.Roads[random2]);
                int pivotPosition = random.Next(pivotStart, gen.Roads[0].ListCities.Count);

                List<City> newListCities = new List<City>();
                for(int i = 0; i <= pivotPosition; i++)
                {
                    newListCities.Add(gen.Roads[random1].ListCities[i]);
                }
                for (int i = pivotPosition+1; i < gen.Roads[0].ListCities.Count; i++)
                {
                    newListCities.Add(gen.Roads[random2].ListCities[i]);
                }

                var duplicates = new Road(newListCities).GetDuplicates();
                var remainingCities = new Road(newListCities).GetRemaining(gen.Roads[0].ListCities);

                for(int i = 0; i < duplicates.Count; i++)
                {
                    newListCities[newListCities.LastIndexOf(duplicates[i])] = remainingCities[i];
                }
                var newRoad = new Road(newListCities);
                
                roads.Add(newRoad);
            }
            return roads;
        }

        private static List<Road> GenerateMutations(Generation gen, int mutationsNumber)
        {
            List<Road> result = new List<Road>();
            while (result.Count < mutationsNumber)
            {
                int rndRoadIndex = random.Next(gen.Roads.Count);

                int random1 = random.Next(gen.Roads[rndRoadIndex].ListCities.Count);
                int random2 = random.Next(gen.Roads[rndRoadIndex].ListCities.Count);
                
                List<City> newListCities = new List<City>(gen.Roads[rndRoadIndex].ListCities);

                var tmpFirstCity = gen.Roads[rndRoadIndex].ListCities[random1];

                newListCities[random1] = newListCities[random2];
                newListCities[random2] = tmpFirstCity;

                var newRoad = new Road(newListCities);
                if (!result.Contains(newRoad))
                {
                    result.Add(newRoad);
                }
            }
            return result;
        }
    }
}
