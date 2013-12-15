using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using Theatre.Storage.Movies;
using Theatre.Storage;

namespace Theatre
{
    public class AppSettings
    {
        // Singletone //
        private static AppSettings instance;
        private AppSettings() 
        {
            UserCity = "kiev";
            Storage = new KinoafishaDataSource();
            ImageManager = new ImageManager();
        }

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSettings();
                }
                return instance;
            }
        }

        // Information Expert  //
        public StorageDataSource Storage;
        public ImageManager ImageManager;
        public string UserCity;
    }
}
