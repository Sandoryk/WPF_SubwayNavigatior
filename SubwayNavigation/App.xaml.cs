using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace SubwayNavigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StringReader stringReader;
            XmlReader xmlReader;
            Int16 xOffset = 0;
            Int16 yOffset = 0;
            Style bluestyle = null, greenstyle = null;
            Dictionary<string, PointCollection> polylineRoute;
            Button newStationButton;
            Polyline route;
            Label stationLabel;

            var mainWin = new MainWindow();
            var routeNav = new SubwayMapNavigation();
            var redBranch = new BranchLine {Name="RedBranch",Color = "Red" };
            var blueBranch = new BranchLine { Name = "BlueBranch", Color = "Blue" };
            var greenBranch = new BranchLine { Name = "GreenBranch", Color = "Green" };
            polylineRoute = new Dictionary<string, PointCollection> { { redBranch.Color, new PointCollection() }, { blueBranch.Color, new PointCollection() }, { greenBranch.Color, new PointCollection() } };
            try
            {
                bluestyle = mainWin.FindResource("BlueRoundCorner") as Style;
                greenstyle = mainWin.FindResource("GreenRoundCorner") as Style;
            }
            catch (ResourceReferenceKeyNotFoundException resEx)
            {
                
            }
            var stationList = new List<SubwayStation> { 
                new SubwayStation{Name = "Academmistechko", Number=1, BrachLine = redBranch, X = xOffset+=6, Y = yOffset+=12},
                new SubwayStation{Name = "Zhytomyrska", Number=2, BrachLine = redBranch, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Sviatoshyn", Number=3, BrachLine = redBranch, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Nyvky", Number=4, BrachLine = redBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset+=4},
                new SubwayStation{Name = "Beresteiska", Number=5, BrachLine = redBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset+=3},
                new SubwayStation{Name = "Shuliavska", Number=6, BrachLine = redBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset+=3},
                new SubwayStation{Name = "Politekhnichnyi_instytut", Number=7, BrachLine = redBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset+=3},
                new SubwayStation{Name = "Vokzalna", Number=8, BrachLine = redBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset+=7, Y = yOffset+=1},
                new SubwayStation{Name = "Universytet", Number=9, BrachLine = redBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset+=7, Y = yOffset},
                new SubwayStation{Name = "Teatralna", Number=10, BrachLine = redBranch, NameAlignment = StationNameAlignment.Bottom, SwitchToStationNumber = 40, SwitchToStationBrachLine = greenBranch, X = xOffset+=7, Y = yOffset},
                new SubwayStation{Name = "Khreshchatyk", Number=11, BrachLine = redBranch, SwitchToStationNumber = 26, SwitchToStationBrachLine = blueBranch, X = xOffset+=7, Y = yOffset},
                new SubwayStation{Name = "Arsenalna", Number=12, BrachLine = redBranch, X = xOffset+=7, Y = yOffset+=4},
                new SubwayStation{Name = "Dnipro", Number=13, BrachLine = redBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset+=4, Y = yOffset+=3},
                new SubwayStation{Name = "Hidropark", Number=14, BrachLine = redBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset+=5, Y = yOffset+=2},
                new SubwayStation{Name = "Livoberezhna", Number=15, BrachLine = redBranch, X = xOffset+=6, Y = yOffset},
                new SubwayStation{Name = "Darnytsia", Number=16, BrachLine = redBranch, X = xOffset+=6, Y = yOffset-=4},
                new SubwayStation{Name = "Chernihivksa", Number=17, BrachLine = redBranch, X = xOffset+=6, Y = yOffset-=4},
                new SubwayStation{Name = "Lisova", Number=18, BrachLine = redBranch, X = xOffset+=6, Y = yOffset-=4},

                new SubwayStation{Name = "Heroiv_Dnipra", Number=19, BrachLine = blueBranch, X = xOffset=48, Y = yOffset=7},
                new SubwayStation{Name = "Minska", Number=20, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Obolon", Number=21, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Petrivka", Number=22, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Tarasa_Shevchenka", Number=23, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Kontractova_Ploshcha", Number=24, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Poshtova_Ploshcha", Number=25, BrachLine = blueBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Maidan_Nezalezhnosti", Number=26, BrachLine = blueBranch, SwitchToStationNumber = 11, SwitchToStationBrachLine = redBranch, X = xOffset, Y = yOffset+=4},
                new SubwayStation{Name = "Ploshcha_Lva_Tolstoho", Number=27, BrachLine = blueBranch, SwitchToStationNumber = 41, SwitchToStationBrachLine = greenBranch, NameAlignment = StationNameAlignment.Left, X = xOffset-=5, Y = yOffset+=10},
                new SubwayStation{Name = "Olimpiiska", Number=28, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset-=4, Y = yOffset+=7},
                new SubwayStation{Name = "Palats_Ukraina", Number=29, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Lybidska", Number=30, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Demiivska", Number=31, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset-=4, Y = yOffset+=6},
                new SubwayStation{Name = "Holosiivska", Number=32, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset-=4, Y = yOffset+=5},
                new SubwayStation{Name = "Vasylkivska", Number=33, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Left, X = xOffset-=4, Y = yOffset+=5},
                new SubwayStation{Name = "Vystavkovyi_Tsentr", Number=34, BrachLine = blueBranch, X = xOffset-=4, Y = yOffset+=5},
                new SubwayStation{Name = "Ipodrom", Number=35, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset-=6, Y = yOffset+=3},
                new SubwayStation{Name = "Teremky", Number=36, BrachLine = blueBranch, NameAlignment = StationNameAlignment.Top, X = xOffset-=6, Y = yOffset},

                new SubwayStation{Name = "Syrets", Number=37, BrachLine = greenBranch, X = xOffset=29, Y = yOffset=20},
                new SubwayStation{Name = "Dorohozhychi", Number=38, BrachLine = greenBranch, X = xOffset+=4, Y = yOffset+=4},
                new SubwayStation{Name = "Lukianivska", Number=39, BrachLine = greenBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset+=4},
                new SubwayStation{Name = "Zoloti_Vorota", Number=40, BrachLine = greenBranch, NameAlignment = StationNameAlignment.Left, SwitchToStationNumber = 10, SwitchToStationBrachLine = redBranch, X = xOffset+=3, Y = yOffset+=7},
                new SubwayStation{Name = "Palats_Sportu", Number=41, BrachLine = greenBranch, SwitchToStationNumber = 27, SwitchToStationBrachLine = blueBranch, X = xOffset+=6, Y = yOffset+=10},
                new SubwayStation{Name = "Klovska", Number=42, BrachLine = greenBranch, X = xOffset+=4, Y = yOffset+=7},
                new SubwayStation{Name = "Pecherksa", Number=43, BrachLine = greenBranch, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Druzhby_Narodiv", Number=44, BrachLine = greenBranch, X = xOffset, Y = yOffset+=6},
                new SubwayStation{Name = "Vydubychi", Number=45, BrachLine = greenBranch, X = xOffset+=4, Y = yOffset+=6},
                new SubwayStation{Name = "Slavutych", Number=46, BrachLine = greenBranch, X = xOffset+=6, Y = yOffset+=10},
                new SubwayStation{Name = "Osokorky", Number=47, BrachLine = greenBranch, X = xOffset+=4, Y = yOffset+=4},
                new SubwayStation{Name = "Pozniaky", Number=48, BrachLine = greenBranch, NameAlignment = StationNameAlignment.Bottom, X = xOffset+=6, Y = yOffset+=4},
                new SubwayStation{Name = "Kharkivska", Number=49, BrachLine = greenBranch, X = xOffset+=6, Y = yOffset},
                new SubwayStation{Name = "Vyrlytsia", Number=50, BrachLine = greenBranch, X = xOffset+=6, Y = yOffset-=4},
                new SubwayStation{Name = "Boryspilska", Number=51, BrachLine = greenBranch, X = xOffset+=4, Y = yOffset-=4},
                new SubwayStation{Name = "Chervonyi_Khutir", Number=52, BrachLine = greenBranch, NameAlignment = StationNameAlignment.Left, X = xOffset+=4, Y = yOffset-=4},
            };
            string sampleButtonXaml = XamlWriter.Save(mainWin.SampleButton);

            foreach (var station in stationList)
            {
                using (stringReader = new StringReader(sampleButtonXaml))
                {
                    xmlReader = XmlReader.Create(stringReader);
                    newStationButton = (Button)XamlReader.Load(xmlReader);
                    station.X = (int)(mainWin.MainGrid.Width * station.X / 100);
                    station.Y = (int)(mainWin.MainGrid.Height * station.Y / 100);
                    newStationButton.Margin = new Thickness(station.X, station.Y, 0, 0);
                    if (String.IsNullOrEmpty(station.BrachLine.Color) == false && polylineRoute.ContainsKey(station.BrachLine.Color))
                    {
                        polylineRoute[station.BrachLine.Color].Add(new Point((station.X + station.Width / 2),(station.Y + station.Width / 2)));
                    }
                    newStationButton.Name = station.Name;
                    newStationButton.Command = routeNav.ClickCommand;
                    newStationButton.CommandParameter = newStationButton;
                    switch (station.BrachLine.Color)
                    {
                        case "Blue":
                            if (bluestyle != null) 
                                newStationButton.Style = bluestyle;
                            break;
                        case "Green":
                            if (greenstyle != null) 
                                newStationButton.Style = greenstyle;
                            break;
                    }

                    stationLabel = new Label();
                    stationLabel.Content = station.Name.Replace("_", " ");
                    stationLabel.Height = 20;
                    stationLabel.Width = (stationLabel.Content as string).Length * stationLabel.FontSize/1.6;
                    stationLabel.FontWeight = FontWeights.Bold;
                    stationLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    stationLabel.VerticalAlignment = VerticalAlignment.Top;
                    stationLabel.Padding = new Thickness(1,0,1,0);
                    //stationLabel.BorderBrush = new SolidColorBrush(Colors.Black);
                    //stationLabel.BorderThickness = new Thickness(2);
                    switch (station.NameAlignment)
                    {
                        case StationNameAlignment.Left:
                            stationLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
                            stationLabel.Margin = new Thickness(newStationButton.Margin.Left - stationLabel.Width, newStationButton.Margin.Top - (stationLabel.Height / 2 - newStationButton.Height / 2), 0, 0);
                            break;
                        case StationNameAlignment.Top:
                            stationLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                            stationLabel.Margin = new Thickness(newStationButton.Margin.Left + newStationButton.Width / 2 - stationLabel.Width / 2, newStationButton.Margin.Top - stationLabel.Height, 0, 0);
                            break;
                        case StationNameAlignment.Right:
                            stationLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
                            stationLabel.Margin = new Thickness(newStationButton.Margin.Left + newStationButton.Width, newStationButton.Margin.Top - (stationLabel.Height/2 - newStationButton.Height / 2), 0, 0);
                            break;
                        case StationNameAlignment.Bottom:
                            stationLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                            stationLabel.Margin = new Thickness(newStationButton.Margin.Left + newStationButton.Width/2 - stationLabel.Width/2, newStationButton.Margin.Top + newStationButton.Height, 0, 0);
                            break;
                    }

                    mainWin.MainGrid.Children.Add(stationLabel);
                    mainWin.MainGrid.Children.Add(newStationButton);
                }
            }
            foreach (var key in polylineRoute.Keys)
            {
                route = new Polyline();
                route.Points = polylineRoute[key];
                route.StrokeThickness = 4;
                route.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(key));
                mainWin.MainGrid.Children.Insert(0,route);
            }
            mainWin.MainGrid.Children.Remove(mainWin.SampleButton);
            routeNav.StationList = stationList;
            routeNav.RouteBuilder = new RouteBuilder(mainWin.MainGrid);
            mainWin.DataContext = routeNav;
            mainWin.Show();
        }
    }
}
