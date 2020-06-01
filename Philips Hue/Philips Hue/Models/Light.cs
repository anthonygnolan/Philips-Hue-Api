﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipsHue.Models
{
    public class Light
    {
        public State state { get; set; }
        public Swupdate1 swupdate { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modelid { get; set; }
        public string manufacturername { get; set; }
        public string productname { get; set; }
        public Capabilities capabilities { get; set; }
        public Config config { get; set; }
        public string uniqueid { get; set; }
        public string swversion { get; set; }
    }



    public class State
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public int hue { get; set; }
        public int sat { get; set; }
        public string effect { get; set; }
        public List<double> xy { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string colormode { get; set; }
        public string mode { get; set; }
        public bool reachable { get; set; }
    }

    public class Swupdate1
    {
        public string state { get; set; }
        public DateTime lastinstall { get; set; }
    }

    public class Capabilities
    {
        public bool certified { get; set; }
        public Control control { get; set; }
        public Streaming streaming { get; set; }
    }

    public class Control
    {
        public int mindimlevel { get; set; }
        public int maxlumen { get; set; }
        public string colorgamuttype { get; set; }
        public List<List<double>> colorgamut { get; set; }
        public Ct ct { get; set; }
    }

    public class Ct
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class Streaming
    {
        public bool renderer { get; set; }
        public bool proxy { get; set; }
    }
}
