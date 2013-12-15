using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using Theatre.Storage.Movies;

namespace Theatre.Storage
{
    public class AppSettings
    {
        // Singletone //
        private static AppSettings instance;
        private AppSettings() { }
        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSettings();
                    instance.UserCity = "kiev";
                    instance.Storage = new KinoafishaDataSource();
                }
                return instance;
            }
        }


        public StorageDataSource Storage;
        public string UserCity;
    }
}
