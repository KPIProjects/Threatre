using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;

namespace Theatre.Storage.Session
{
    public class SimpleSession
    {
        public string Time { get; set; }
        public string URL { get; set; }
        public string In3DText { get; set; }
        public string Status { get; set; }
    }

    public class Hall
    {
        public string Name { get; set; }
        public bool In3D { get; set; }
        public List<SimpleSession> Sessions { get; set; }
    }

    public class SessionInCinema
    {
        public string CinemaName { get; set; }
        public string CinemaURL { get; set; }
        public string CinemaPhone { get; set; }
        public string CinemaAdress { get; set; }
        public List<Hall> Halls { get; set; }
        public string Timesheet { get; set; }  
    }
}
