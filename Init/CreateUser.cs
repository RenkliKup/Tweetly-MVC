﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {

        public static User GetProfil(this IWebElement element, bool detay = false)
        {
            var profil = new TakipEdilen();

            string Text = element.Text.Replace("\r","");

            profil.Name = Text.Split('\n')[0].StartsWith('@') ? null : Text.Split('\n')[0];
            profil.Cinsiyet = UserGetMethods.CinsiyetBul(profil.Name);

            int basla = Text.IndexOf('@');
            profil.Username = Text[basla..Text.IndexOf('\n', basla)];

            profil.PhotoURL = element.FindElement(By.TagName("img")).GetAttribute("src").Replace("x96", "200x200");
            profil.IsPrivate = element.FindElements(By.CssSelector("[aria-label='Korumalı hesap']")).Count > 0;

            if (Text.Contains("Seni takip ediyor"))
                profil.FollowersStatus = "Seni takip ediyor";
            else profil.FollowersStatus = "Takip etmiyor";

            if (Text.Contains("Takip ediliyor"))
                profil.FollowStatus = "Takip ediliyor";
            else if (Text.Contains("Takip et")) profil.FollowStatus = "Takip et";

            var bios = element.FindElements(By.XPath("/div/div[2]/div[2]"));
            if (bios.Count > 0) profil.Bio = bios[0].Text;

            if (detay) profil = (TakipEdilen) Drivers.MusaitOlanDriver().GetProfil(profil.Username, profil);
            

            return profil;
        }
        public static User GetProfil(this IWebDriver driver, string username, bool faster = false)
        {
            string link;
        yeniden:
            try
            {
                link = $"https://mobile.twitter.com/{username}";
                driver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            var profil = new TakipEdilen();
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.Count = Hesap.Instance.TakipEdilenler.Count + 1;
                profil.Username = username;
                profil.TweetCount = driver.GetTweetCount();
                profil.Name = driver.GetName();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.PhotoURL = driver.GetPhotoURL(username);
                profil.Following = driver.GetFollowing(username);
                profil.Followers = driver.GetFollowers(username);
                profil.FollowersStatus = driver.IsFollowers();
                profil.FollowStatus = driver.GetfollowStatus();
                profil.Bio = driver.GetBio();
                profil.IsPrivate = driver.IsPrivate();
                profil.Cinsiyet = UserGetMethods.CinsiyetBul(profil.Name);
                profil.TweetSikligi = UserGetMethods.GetGunlukSiklik(profil.TweetCount, profil.Date);
                if(!faster) profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);
                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");
                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = UserGetMethods.GetGunlukSiklik(profil.LikeCount, profil.Date);
                if (!faster) profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }



            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
        public static User GetProfil(this IWebDriver driver, string username, TakipEdilen profil, bool faster = false)
        {
            string link;
        yeniden:
            try
            {
                link = "https://mobile.twitter.com/" + username;
                driver.Navigate().GoToUrl(link);
            }
            catch { goto yeniden; }
            if (Yardimci.Control(driver, username, link, 300000))
            {
                profil.TweetCount = driver.GetTweetCount();
                profil.Date = driver.GetDate();
                profil.Location = driver.GetLocation();
                profil.Following = driver.GetFollowing(username);
                profil.Followers = driver.GetFollowers(username);
                profil.TweetSikligi = UserGetMethods.GetGunlukSiklik(profil.TweetCount, profil.Date);
                if (!faster) profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");

                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = UserGetMethods.GetGunlukSiklik(profil.LikeCount, profil.Date);
                if (!faster) profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }
            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }

    }
}
