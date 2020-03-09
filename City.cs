using SQLite;
using System;
using System.Text;

namespace WpfApp
{
    [Table("City")]
    public class City
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("X")]
        public double X { get; set; }

        [Column("Y")]
        public double Y { get; set; }

        public City(string name, double x, double y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }
        public City()
        {
           
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                City city = (City) obj;

                return (Id.Equals(city.Id) && Name.Equals(city.Name) &&
                    X == city.X && Y == city.Y);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.Append(" Id : " + Id + ", ");
            sb.Append(" Name : " + Name + ", ");
            sb.Append(" X : " + X + ", ");
            sb.Append(" Y : " + Y);
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
