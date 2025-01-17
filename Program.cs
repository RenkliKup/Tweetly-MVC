using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetly_MVC.Init;
using Tweetly_MVC.Tweetly;

namespace Tweetly_MVC
{
    public class Programk
    {

        public static void Main(string[] args)
        {
            Hesap.Instance.LoginUserName = Test.username;
            Hesap.Instance.LoginPass = Test.pass;
            Yardimci.KillProcces();
            Drivers.CreateDrivers();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
