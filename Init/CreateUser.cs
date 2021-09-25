﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetly_MVC.Tweetly;
using Tweetly_MVC.Twitter;

namespace Tweetly_MVC.Init
{
    public static class CreateUser
    {

        public static bool Filter(User profil)
        {
            if (profil.Cinsiyet == "Erkek" && !Hesap.Instance.Settings.checkErkek)
                return false;
            if (profil.Cinsiyet == "Kadın" && !Hesap.Instance.Settings.checkKadin)
                return false;
            if (profil.Cinsiyet == "Unisex" && !Hesap.Instance.Settings.checkUnisex)
                return false;
            if (profil.Cinsiyet == "Belirsiz" && !Hesap.Instance.Settings.checkBelirsiz)
                return false;
            if (profil.FollowStatus != "Takip Ediliyor" && Hesap.Instance.Settings.checkTakipEtmediklerim)
                return false;
            if (profil.FollowStatus == "Takip Ediliyor" && Hesap.Instance.Settings.checkTakipEttiklerim)
                return false;
            if (profil.FollowersStatus != "Seni Takip Ediyor" && Hesap.Instance.Settings.checkBeniTakipEtmeyenler)
                return false;
            if (profil.FollowersStatus == "Seni Takip Ediyor" && Hesap.Instance.Settings.checkBeniTakipEdenler)
                return false;
            if (profil.IsPrivate && Hesap.Instance.Settings.checkGizliHesap)
                return false;
            return true;
        }
        public static User GetProfil(this IWebElement element, bool detay = false)
        {
            var profil = new TakipEdilen();
            string Text = element.Text.Replace("\r", "");

            profil.Name = Liste.GetName(Text);
            profil.Cinsiyet = Profil.CinsiyetBul(profil.Name);
            profil.Username = Liste.GetUserName(Text);
            profil.PhotoURL = Liste.GetPhotoURL(element);
            profil.IsPrivate = Liste.İsPrivate(element);
            profil.FollowersStatus = Liste.GetFollowersStatus(Text);
            profil.FollowStatus = Liste.GetFollowStatus(Text);
            profil.Bio = Liste.GetBio(element);
            if (Filter(profil)) return null;
            if (detay)
                profil = (TakipEdilen)Drivers.MusaitOlanDriver().GetProfil(profil.Username, profil);

            return profil;
        }
        public static User GetProfil(this IWebDriver driver, string username)
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
                profil.Cinsiyet = Profil.CinsiyetBul(profil.Name);
                profil.TweetSikligi = Profil.GetGunlukSiklik(profil.TweetCount, profil.Date);
                profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);
                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");
                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = Profil.GetGunlukSiklik(profil.LikeCount, profil.Date);
                profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }



            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }
        private static User GetProfil(this IWebDriver driver, string username, TakipEdilen profil)
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
                profil.TweetSikligi = Profil.GetGunlukSiklik(profil.TweetCount, profil.Date);
                profil.LastTweetsDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.TweetCount);

                driver.JSCodeRun("document.querySelector('[data-testid=ScrollSnap-List] > div:last-child a').click();");

                profil.LikeCount = driver.GetLikeCount();
                profil.BegeniSikligi = Profil.GetGunlukSiklik(profil.LikeCount, profil.Date);
                profil.LastLikesDate = driver.GetLastTweetsoOrLikesDateAVC(profil.Date, profil.LikeCount);
            }
            Drivers.kullanıyorum.Remove(driver);
            return profil;
        }

    }
}
