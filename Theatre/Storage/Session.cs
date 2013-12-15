using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;

namespace Theatre
{
    [DataContract]
    public class SessionResponse
    {
        [DataMember]
        public string k_id;

        [DataMember]
        public string k_name;

        [DataMember]
        public string k_price;

        [DataMember]
        public string k_url;

        [DataMember]
        public string k_type;

        [DataMember]
        public string k_bron;

        [DataMember]
        public string k_go;

        [DataMember]
        public string k_adv;

        [DataMember]
        public string k_adv_i;

        [DataMember]
        public string k_t0;

        [DataMember]
        public string k_t1;

        [DataMember]
        public string k_t2;

        [DataMember]
        public string k_t3;

        [DataMember]
        public string k_t4;

        [DataMember]
        public string h_name;

        [DataMember]
        public string h_is3d;

        [DataMember]
        public string sessions;
    }

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

    public class Session
    {
        public string CinemaName { get; set; }
        public string CinemaURL { get; set; }
        public string CinemaPhone { get; set; }
        public string CinemaAdress { get; set; }
        public List<Hall> Halls { get; set; }
        public string Timesheet { get; set; }
        public event EventHandler CinemaLogoDidDownload;

        public Session(SessionResponse KinoafishaResponse)
        {
            //CinemaPhone = "+380930164207";
            CinemaName = KinoafishaResponse.k_name;
            CinemaURL = "http://kinoafisha.ua" + KinoafishaResponse.k_url;
            Halls = new List<Hall>();
            Hall hall = new Hall();
            hall.Name = KinoafishaResponse.h_name;
            hall.In3D = (KinoafishaResponse.h_is3d == "1");
            hall.Sessions = ParseSessions(KinoafishaResponse.sessions);
            foreach (SimpleSession session in hall.Sessions)
            {
                session.In3DText = hall.In3D ? "3D" : "";
            }
            Halls.Add(hall);
            Timesheet = "";
            foreach (SimpleSession session in hall.Sessions)
            {
                Timesheet += session.Time + " ";
            }
            Timesheet.Remove(Timesheet.Length - 1, 1);

            var request = WebRequest.CreateHttp(CinemaURL);
            request.Method = "GET";
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                ParseCinemaHTMLPage(responseContent);
            }, null);
        }

        public void AppendResponce(SessionResponse response)
        {
            Hall hall = new Hall();
            hall.Name = response.h_name;
            hall.In3D = (response.h_is3d == "1");
            hall.Sessions = ParseSessions(response.sessions);
            foreach (SimpleSession session in hall.Sessions)
            {
                session.In3DText = hall.In3D ? "3D" : "";
            }
            Halls.Add(hall);
            Timesheet += "; ";
            foreach (SimpleSession session in hall.Sessions)
            {
                Timesheet += session.Time + " ";
            }
            Timesheet.Remove(Timesheet.Length - 1, 1);
        }

        List<SimpleSession> ParseSessions(string ToParse)
        {
            List<SimpleSession> result = new List<SimpleSession>();
            string parseString = ToParse;
            if (ToParse.IndexOf("<a onclick=") != -1)
            {
                string[] sessions = parseString.Replace("<a onclick=\"_gaq.push(['_trackEvent', 'Время сеанса', 'vkino']);\" href=\"", "").
                                                Replace("\" class=\"activeEvent vkino-link\">", "\n").
                                                Replace("</a>", "\0").Split('\0');
                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    string[] sessionData = sessions[i].Split('\n');

                    SimpleSession session = new SimpleSession();
                    session.Time = sessionData[1];
                    session.URL = sessionData[0];
                    session.Status = "Выбрать места";
                    result.Add(session);
                }
            }
            else if (ToParse.IndexOf("<a href=") != -1)
            {
                string[] sessions = parseString.Replace("<a href=\"javascript:void(0)\" title=\"Купить билеты он-лайн\" onclick=\"_gaq.push(['_trackEvent', 'Время сеанса', '2show']); show2_show(28,[['exclusive_show','","").
                                                Replace("<span class=\"event\">", "").
                                                Replace("</span>", "\r").
                                                Replace("'],['provider','","\0").
                                                Replace("'],['event_origin','","\n").
                                                Replace("']]);\" class=\"activeEvent\">","\t").
                                                Replace("</a>","\a").Split('\a');

                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    if (sessions[i].IndexOf("\r") != -1)
                    {
                        string[] spanParts = sessions[i].Split('\r');
                        string leaveTime = spanParts[0];
                        sessions[i] = sessions[i].Substring(leaveTime.Length+1,sessions[i].Length-leaveTime.Length-1);
                        SimpleSession spanSession = new SimpleSession();
                        spanSession.Time = leaveTime;
                        spanSession.Status = "Покупка невозможна";
                        result.Add(spanSession);
                        i--;
                    }
                    else
                    {
                        string[] parts = sessions[i].Split('\0');
                        string movieName = parts[0];
                        parts = parts[1].Split('\n');
                        string provider = parts[0];
                        parts = parts[1].Split('\t');
                        string eventOrigin = parts[0];
                        string time = parts[1];
                        string link = "http://wg.2show.com.ua/picker/widget?partner_site=http://kinoafisha.ua&partner_id=28&exclusive_show=" + movieName + "&provider=" + provider + "&event_origin=" + eventOrigin + "&use_method=infront";
                        SimpleSession session = new SimpleSession();
                        session.Time = time;
                        session.URL = link;
                        session.Status = "Выбрать места";
                        result.Add(session);
                    }
                }
            }
            else
            {
                string[] sessions = parseString.Replace("<span class=\"event\">", "").Replace("</span>", "\0").Split('\0');
                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    SimpleSession session = new SimpleSession();
                    session.Time = sessions[i];
                    session.URL = "Phone";
                    session.Status = "Забронировать по телефону";
                    result.Add(session);
                }
            }
            return result;
        }

        
        void ParseCinemaHTMLPage(string html)
        {
            /*string logoURL = html.Replace("<link rel=\"image_src\" type=\"\" href=\"", "\0").Split('\0')[1];
            logoURL = logoURL.Substring(0,logoURL.IndexOf("\" />"));
            DownloadLogo(logoURL);*/
            if (html.IndexOf("Телефон: <span>") != -1)
            {
                string phone = html.Replace("Телефон: <span>", "\0").Split('\0')[1];
                phone = phone.Substring(0, phone.IndexOf("</span>"));
                int openScopeIdx = -1;
                for (int i = 0; i < 10 && openScopeIdx == - 1; i++)
                {
                    openScopeIdx = phone.IndexOf(i.ToString()[0]);
                }
                if (openScopeIdx == -1)
                {
                    openScopeIdx = phone.IndexOf('(');
                }

                if (openScopeIdx != -1)
                {
                    phone = phone.Substring(openScopeIdx, phone.Length - openScopeIdx);
                }

                phone = phone.Replace(";", ",");
                if (phone.IndexOf(',') != -1)
                {
                    string[] candidats = phone.Split(',');
                    phone = "";
                    foreach (string candidat in candidats)
                    {
                        if (candidat.IndexOf("бро") != -1 && candidat.IndexOf("кас") != -1)
                        {
                            phone = candidat;
                        }
                    }
                    if (phone == "")
                    {
                        phone = candidats[0];
                    }
                }
                for (int i = 0; i < phone.Length; i++)
                {
                    if (phone[i] < '0' || phone[i] > '9')
                    {
                        phone = phone.Remove(i, 1);
                        i--;
                    }
                }
                CinemaPhone = phone;
            }
            else
            {
                CinemaPhone = "";
            }

            if (html.IndexOf("<p>Адрес кинотеатра: <a class=\"on-map\" href=\"#yamaps\">") != -1)
            {
                string adress = html.Replace("<p>Адрес кинотеатра: <a class=\"on-map\" href=\"#yamaps\">", "\0").Split('\0')[1];
                adress = adress.Substring(0, adress.IndexOf("</a></p>"));
                CinemaAdress = adress;
            }
            else
            {
                CinemaAdress = "";
            }
        }
    }
}
