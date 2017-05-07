using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubwayNavigation
{
    enum StationNameAlignment
    {
        Left = 1,
        Top,
        Right,
        Bottom
    }

    class SubwayStation
    {
        public SubwayStation()
        {
            Width = 20;
            Height = 20;
            SwitchToStationNumber = -1;
            NameAlignment = StationNameAlignment.Right;
        }
        public string Name { get; set; }
        public StationNameAlignment NameAlignment { get; set; }
        public int Number { get; set; }
        public BranchLine BrachLine { get; set; }
        public int SwitchToStationNumber { get; set; }
        public BranchLine SwitchToStationBrachLine { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
