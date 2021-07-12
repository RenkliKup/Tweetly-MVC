﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Tweetly_MVC.Models
{
    public struct İletisim
    {
        public string metin;
        public int veri;
    }
    public static class Hesap
    {
        public static string loginUserName { get; set; }
        public static string loginPass { get; set; }
        public static User OturumBilgileri { get; set; }

        public static List<User> Takipciler = null;

        public static İletisim Iletisim = new İletisim();

        public static List<Cinsiyetler> cins = JsonConvert.DeserializeObject<List<Cinsiyetler>>(File.ReadAllText("Cinsiyetler.json").ToLower());





    }
}
