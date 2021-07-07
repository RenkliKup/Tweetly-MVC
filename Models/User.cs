﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetly_MVC.Models
{
    public class User
    {
        public int Count { get; set; }
        public string PhotoURL { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Cinsiyet { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public double Date { get; set; }
        public int TweetCount { get; set; }
        public double LastTweetsDate { get; set; }
        public string TweetSikligi { get; set; }
        public int LikeCount { get; set; }
        public double LastLikesDate { get; set; }
        public string BegeniSikligi { get; set; }
        public string FollowersStatus { get; set; }
        public string FollowStatus { get; set; }
    }
}