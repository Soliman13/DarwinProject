using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private const string XOVERS_PARAMS_KEY = "NbXovers";
        private const string ELITES_PARAMS_KEY = "NbElites";
        private const string MUTATIONS_PARAMS_KEY = "NbMutations";
        private const string GENERATION_PARAMS_KEY = "NbRoadsPerGeneration";
        public event PropertyChangedEventHandler PropertyChanged;


        private ObservableCollection<City> cities = new ObservableCollection<City>();
        private Dictionary<City, Ellipse> pointsMap = new Dictionary<City, Ellipse>();
        private List<Line> roadLines = new List<Line>();
        private int indexCities;
        private int nbRoadsPerGeneration = 10;
        private int nbMutations = 30;
        private int nbXOver = 30;
        private int nbElites = 2;

        public MainWindow()
        {
            InitializeComponent();

            DB.GetDataBase().CreateTableCity();
            DB.GetDataBase().CreateTableParams();
            var queryParams = DB.GetDataBase().SelectParams("SELECT * FROM Params");
            UpdateParams(queryParams);
            var queryCities = DB.GetDataBase().SelectCity("SELECT * FROM City");
            listCities.ItemsSource = this.cities;
            this.DataContext = this;
            InitCanvas(queryCities);
        }

        private void UpdateParams(List<Params> parameters)
        {
            foreach (Params p in parameters)
            {
                if (p.Key == ELITES_PARAMS_KEY)
                {
                    this.NbElites = p.Value;
                }
                if (p.Key == XOVERS_PARAMS_KEY)
                {
                    this.NbXovers = p.Value;
                }
                if (p.Key == MUTATIONS_PARAMS_KEY)
                {
                    this.NbMutations = p.Value;
                }
                if (p.Key == GENERATION_PARAMS_KEY)
                {
                    this.NbRoadsPerGeneration = p.Value;
                }
                if (p.Key == "indexCities")
                {
                    this.indexCities = p.Value;
                }
            }
        }

        public void AddCity(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvasMap);
            City newCity = new City("City n°" + this.indexCities++, p.X, p.Y);
            DB.GetDataBase().InsertCity(newCity);
            this.AddCity(newCity);
        }

        private void RemoveCity(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            City city = listCities.SelectedItem as City;
            if (city != null)
            {
                this.RemoveCity(city);
                DB.GetDataBase().DeleteCity(city);
            }
            this.cities.Remove(city);
        }

        private void InitCanvas(List<City> citiesDB)
        {
            foreach (City c in citiesDB)
            {
                AddCity(c);
            }
        }
        private void AddCity(City c)
        {
            this.cities.Add(c);
            Ellipse ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.AliceBlue
            };
            Thickness t = new Thickness((c.X - 2.5), (c.Y - 2.5), 0, 0);
            ellipse.Margin = t;
            this.pointsMap.Add(c, ellipse);
            canvasMap.Children.Add(ellipse);
        }

        private void RemoveCity(City c)
        {
            canvasMap.Children.Remove(this.pointsMap[c]);
            this.pointsMap.Remove(c);
        }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void SaveParams()
        {
            Params NbXovers = new Params(XOVERS_PARAMS_KEY, this.NbXovers);
            DB.GetDataBase().InsertParams(NbXovers);
            Params NbMutations = new Params(MUTATIONS_PARAMS_KEY, this.NbMutations);
            DB.GetDataBase().InsertParams(NbMutations);
            Params NbElites = new Params(ELITES_PARAMS_KEY, this.NbElites);
            DB.GetDataBase().UpdateParams(NbElites);
            Params NbRoadsPerGeneration = new Params(GENERATION_PARAMS_KEY, this.NbRoadsPerGeneration);
            DB.GetDataBase().InsertParams(NbRoadsPerGeneration);
            Params indexCities = new Params("indexCities", this.indexCities);
            DB.GetDataBase().InsertParams(indexCities);
        }

        private void ExecAlgo()
        {
            // Remove precedent generation
            Dispatcher.Invoke(() =>
            {
                foreach (Line l in roadLines)
                {
                    canvasMap.Children.Remove(l);
                }
                roadLines.RemoveRange(0, roadLines.Count);
            });

            this.SaveParams();
            var generations = Algo.Launch(new List<City>(this.cities),
                this.NbRoadsPerGeneration, this.NbMutations, this.NbXovers, this.NbElites);

            int indexGen = 0;
            StringBuilder sb = new StringBuilder();
            foreach (Generation g in generations)
            {
                sb.Append("Generation " + indexGen++ + " : ---------------------------------------------");
                sb.Append("\n");
                sb.Append(g);
                sb.Append("\n");
            }
            Dispatcher.Invoke(() =>
            {
                UpdateConsole(sb);
                //display best road
                PrintRoad(generations[generations.Count - 1].Roads[0]);
            });
        }

        private void PrintRoad(Road road)
        {
            for (int i = 0; i < road.ListCities.Count - 1; i++)
            {
                Line line = new Line
                {
                    Stroke = SystemColors.WindowFrameBrush,
                    X1 = road.ListCities[i].X,
                    Y1 = road.ListCities[i].Y,
                    X2 = road.ListCities[i + 1].X,
                    Y2 = road.ListCities[i + 1].Y
                };
                roadLines.Add(line);
                canvasMap.Children.Add(line);
            }
        }

        private void UpdateConsole(StringBuilder sb)
        {
            console.Text = sb.ToString();
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            Thread algoThread = new Thread(this.ExecAlgo);
            algoThread.Start();
        }

        public int NbRoadsPerGeneration
        {
            get { return this.nbRoadsPerGeneration; }
            set
            {
                if (this.nbRoadsPerGeneration != value)
                {
                    this.nbRoadsPerGeneration = value;
                    this.NotifyPropertyChanged("NbRoadsPerGeneration");
                }
            }
        }

        public int NbMutations
        {
            get { return this.nbMutations; }
            set
            {
                if (this.nbMutations != value)
                {
                    this.nbMutations = value;
                    this.NotifyPropertyChanged("NbMutations");
                }
            }
        }

        public int NbXovers
        {
            get { return this.nbXOver; }
            set
            {
                if (this.nbXOver != value)
                {
                    this.nbXOver = value;
                    this.NotifyPropertyChanged("NbXovers");
                }

            }
        }

        public int NbElites
        {
            get { return this.nbElites; }
            set
            {
                if (this.nbElites != value)
                {
                    this.nbElites = value;
                    this.NotifyPropertyChanged("NbElites");
                }
            }
        }
    }
}
