using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WpfApp
{
    public class Road: IComparable<Road>
    {
        public List<City> ListCities { get; set; }
        public double Score { get { return CalculScore(); } }

        public Road(List<City> listCities)
        {
            this.ListCities = listCities;
        }

        public double CalculScore()
        {
            double result = 0;
            for (int i = 0; i < ListCities.Count - 1; i++)
            {
                result += CalculDistance(ListCities[i], ListCities[i + 1]);
            }
            return result;
        }

        public double CalculDistance(City c1, City c2)
        {
            double xDiff = c1.X - c2.X;
            double yDiff= c1.Y - c2.Y;
            double result = Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
            return result;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Road road = (Road)obj;

                if (ListCities.Count != road.ListCities.Count)
                {
                    return false;
                }

                for (int i = 0; i < this.ListCities.Count; i++)
                {
                    if (ListCities[i] != road.ListCities[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (City c in this.ListCities)
            {
                sb.Append(c.Name);
                if(!c.Equals(ListCities[ListCities.Count - 1]))
                {
                    sb.Append(" ---> ");
                }
            }
            sb.Append(" ] : " + CalculScore());
            return sb.ToString();
        }

        public int CompareTo(Road other)
        {
            if (other == null)
            {
                return 1;
            }
            return CalculScore().CompareTo(other.CalculScore());
        }

        public List<City> GetDuplicates()
        {
            List<City> duplicates = new List<City>();
            foreach (City c in ListCities)
            {
                if (ListCities.IndexOf(c) != ListCities.LastIndexOf(c) && !duplicates.Contains(c))
                {
                    duplicates.Add(c);
                }
            }
            return duplicates;
        }

        public List<City> GetRemaining(List<City> initialList)
        {
            List<City> remainingCities = new List<City>();
            foreach (City c in initialList)
            {
                if (ListCities.IndexOf(c) == -1)
                {
                    remainingCities.Add(c);
                }
            }
            return remainingCities;
        }
    }
}
