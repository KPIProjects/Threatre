using System.Collections.Generic;
using System.Runtime.Serialization;

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

    public class Session
    {
        public string CinemaName;
        public string CinemaURL;
        public string HallName;
        public bool In3D;
        public Dictionary<string, string> Sessions;

        public Session(SessionResponse response)
        {
            CinemaName = response.k_name;
            CinemaURL = "http://kinoafisha.ua" + response.k_url;
            HallName = response.h_name;
            In3D = (response.h_is3d == "1");
            Sessions = ParseSessions(response.sessions);
        }

        Dictionary<string, string> ParseSessions(string ToParse)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string parseString = ToParse;
            if (ToParse.IndexOf("<a onclick=") != -1)
            {
                string[] sessions = parseString.Replace("<a onclick=\"_gaq.push(['_trackEvent', 'Время сеанса', 'vkino']);\" href=\"", "").
                                                Replace("\" class=\"activeEvent vkino-link\">", "\n").
                                                Replace("</a>", "\0").Split('\0');
                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    string[] sessionData = sessions[i].Split('\n');
                    dict.Add(sessionData[0], sessionData[1]);
                }
            }
            else if (ToParse.IndexOf("<a href=") != -1)
            {
                string[] sessions = parseString.Replace("<a href=\"javascript:void(0)\" title=\"Купить билеты он-лайн\" onclick=\"_gaq.push(['_trackEvent', 'Время сеанса', '2show']); show2_show(28,[['exclusive_show','","").
                                                Replace("'],['provider','","\0").
                                                Replace("'],['event_origin','","\n").
                                                Replace("']]);\" class=\"activeEvent\">","\t").
                                                Replace("</a>","\a").Split('\a');

                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    string[] parts = sessions[i].Split('\0');
                    string movieName = parts[0];
                    parts = parts[1].Split('\n');
                    string provider = parts[0];
                    parts = parts[1].Split('\t');
                    string eventOrigin = parts[0];
                    string time = parts[1];
                    string link = "http://wg.2show.com.ua/picker/widget?partner_site=http://kinoafisha.ua&partner_id=28&exclusive_show="+movieName+"&provider="+provider+"&event_origin="+eventOrigin+"&use_method=infront";
                    dict.Add(time, link);
                }
            }
            else
            {
                string[] sessions = parseString.Replace("<span class=\"event\">", "").Replace("</span>", "\0").Split('\0');
                for (int i = 0; i < sessions.Length - 1; i++)
                {
                    dict.Add(sessions[i], "");
                }
            }
            return dict;
        }
    }
}
