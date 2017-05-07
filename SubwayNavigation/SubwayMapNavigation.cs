using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SubwayNavigation
{
    class SubwayMapNavigation
    {
        Button[] activeStationButtons = new Button[2];
        public SubwayMapNavigation()
        {
            ClickCommand = new Command(ClickMethod);
        }
        public ICommand ClickCommand { get; set; }
        public IAnimationOperator RouteBuilder { get; set; }
        public List<SubwayStation> StationList { get; set; }

        private void GetRoutePoints(SubwayStation fromStation, SubwayStation toStation, ref Point[] routepoints)
        {
            Int32 firstStNum, secondStNum, nextIndex, tempInt;
            List<SubwayStation> stationsOnRoute;

            routepoints = null;
            firstStNum = fromStation.Number;
            secondStNum = toStation.Number;
            if (fromStation.Number > toStation.Number)
            {
                tempInt = firstStNum;
                firstStNum = secondStNum;
                secondStNum = tempInt;
            }
            stationsOnRoute = StationList.FindAll(t => t.Number >= firstStNum && t.Number <= secondStNum);
            if (stationsOnRoute != null)
            {
                if (fromStation.Number > toStation.Number)
                {
                    stationsOnRoute.Reverse();
                }
                routepoints = new Point[stationsOnRoute.Count];
                nextIndex = 0;
                foreach (var station in stationsOnRoute)
                {
                    routepoints[nextIndex++] = new Point(station.X, station.Y);
                }
            }
        }

        private void DrawRoute()
        {
            
            Point[] routepoints, routePointsPart = null;
            SubwayStation startStation, endStation, transferStation;

            if (RouteBuilder != null)
            {
                RouteBuilder.StopAnimation();
                if (activeStationButtons[0] != null && activeStationButtons[1] != null)
                {
                    startStation = StationList.Find(t => t.Name == activeStationButtons[0].Name);
                    endStation = StationList.Find(t => t.Name == activeStationButtons[1].Name);
                    if (startStation != null && endStation != null)
                    {
                        routepoints = new Point[0];
                        if (startStation.BrachLine == endStation.BrachLine)
                        {
                            GetRoutePoints(startStation, endStation, ref routepoints);
                            if (routepoints != null)
                            {
                                RouteBuilder.BeginAnimation(routepoints);
                            }
                        }
                        else
                        {
                            transferStation = StationList.Find(t => t.BrachLine == startStation.BrachLine && t.SwitchToStationBrachLine == endStation.BrachLine);
                            if (transferStation != null)
                            {
                                GetRoutePoints(startStation, transferStation, ref routePointsPart);
                                if (routePointsPart != null)
                                {
                                    Array.Resize<Point>(ref routepoints, routepoints.Length + routePointsPart.Length);
                                    Array.Copy(routePointsPart, 0, routepoints, routepoints.Length - routePointsPart.Length, routePointsPart.Length);
                                }
                            }
                            transferStation = StationList.Find(t => t.BrachLine == endStation.BrachLine && t.SwitchToStationBrachLine == startStation.BrachLine);
                            if (transferStation != null)
                            {
                                GetRoutePoints(transferStation, endStation, ref routePointsPart);
                                if (routePointsPart != null)
                                {
                                    Array.Resize<Point>(ref routepoints, routepoints.Length + routePointsPart.Length);
                                    Array.Copy(routePointsPart, 0, routepoints, routepoints.Length - routePointsPart.Length, routePointsPart.Length);
                                }
                            }
                            if (routepoints != null)
                            {
                                RouteBuilder.BeginAnimation(routepoints);
                            }
                        }
                    }
                }
            }
        }

        private void TransformStationButton(Button clickedButton, int delta)
        {
            clickedButton.Margin = new Thickness(clickedButton.Margin.Left + delta, clickedButton.Margin.Top + delta, 0, 0);
            clickedButton.Height = clickedButton.Height - delta*2;
            clickedButton.Width = clickedButton.Width - delta*2;
        }

        private void ClickMethod(object sender)
        {
            Button clickedButton, activeButton;

            if (sender != null && sender is Button)
            {
                clickedButton = (sender as Button);
                if (activeStationButtons.Length == 2)
                {
                    if (activeStationButtons[0] != null && activeStationButtons[0].Name == clickedButton.Name)
                    {
                        activeStationButtons[0] = null;
                        TransformStationButton(clickedButton,4);
                    }
                    else if (activeStationButtons[1] != null && activeStationButtons[1].Name == clickedButton.Name)
                    {
                        activeStationButtons[1] = null;
                        TransformStationButton(clickedButton, 4);
                    }
                    else if (activeStationButtons[0] == null)
                    {
                        activeStationButtons[0] = clickedButton;
                        TransformStationButton(clickedButton, -4);
                    }
                    else if (activeStationButtons[1] == null)
                    {
                        activeStationButtons[1] = clickedButton;
                        TransformStationButton(clickedButton, -4);
                    }
                    else
                    {
                        activeButton = activeStationButtons[1];
                        if (activeButton != null)
                        {
                            activeStationButtons[1] = null;
                            TransformStationButton(activeButton, 4);
                        }
                        activeStationButtons[1] = clickedButton;
                        TransformStationButton(clickedButton, -4);
                    }
                    DrawRoute();
                }
            }
            else
                MessageBox.Show("Unknown station");
        }
    }
}
