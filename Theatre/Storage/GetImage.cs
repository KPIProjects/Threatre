using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;

namespace Theatre
{
    class GetImage
    {
        static public void GetExternalImageBytes(string url, int idx, Action<byte[], int> callback)
        {
            byte[] data;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

            myReq.BeginGetResponse(result =>
            {
                try
                {
                    WebResponse response = myReq.EndGetResponse(result);

                    using (var stream = response.GetResponseStream())
                    {
                        using (BinaryReader br = new BinaryReader(stream))
                        {
                            data = br.ReadBytes(5000000);
                            br.Close();
                        }
                    }
                    response.Close();

                    callback(data, idx);
                }
                catch
                {
                    callback(null, idx);
                }
            }, null);
        }
    }
}
