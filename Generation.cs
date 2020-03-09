using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApp
{
    public class Generation
    {
        public List<Road> Roads { get; set; }

        public Generation(List<Road> roads)
        {
            this.Roads = roads;
        }

        public List<Road> GetBestRoad(int nbRoads)
        {
            List<Road> shuffledList = Roads.Distinct(new Comparator()).ToList();
            shuffledList.Sort();
            return shuffledList.GetRange(0, nbRoads);
        }

        public double GetBestScore()
        {
            return this.GetBestRoad(1)[0].Score;
        }

        public double GetAverageScore()
        {
            var scores = from road in Roads select road.Score;
            return scores.Average();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("The best score is " + GetBestScore() + " ; the average score is " + GetAverageScore() + "\n");
            sb.Append("{\n");
            foreach (Road r in Roads)
            {
                sb.Append("\t" + r);
                sb.Append("\n");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }

    class Comparator : IEqualityComparer<Road>
    {
        public bool Equals(Road r1, Road r2)
        {
            return r1.Equals(r2);
        }

        public int GetHashCode(Road r)
        {
            return r.GetHashCode();
        }
    }
}
