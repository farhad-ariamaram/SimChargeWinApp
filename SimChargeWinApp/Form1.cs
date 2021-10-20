using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SimChargeWinApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimChargeWinApp
{
    public partial class Form1 : Form
    {
        EmployeeRequestDBContext _db;
        IWebDriver driver;
        ChromeOptions options;
        TblSimCharge record;

        public Form1()
        {
            InitializeComponent();
            _db = new EmployeeRequestDBContext();
            options = new ChromeOptions();
            options.AddArgument($"user-data-dir={Directory.GetCurrentDirectory()}\\MyRef");
            options.AddArgument($"headless");
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = false;
            new ChromeDriver(chromeDriverService, options);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            while (true)
            {
                await getCharge();
                await Task.Delay(30000);
            }
        }

        public async Task getCharge()
        {
            try
            {
                driver.Url = "https://my.irancell.ir/fa/#!/home";
                await Task.Delay(15000);
                try
                {
                    if (driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div[2]/div/div[1]/div[4]/div/h3")) != null && driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div[2]/div/div[1]/div[4]/div/h3")).Text == "ورود به ایرانسل من")
                    {
                        driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div[2]/div/div[1]/div[5]/div[2]/div/ul/li/div/div/div/input")).SendKeys("09333055598");
                        await Task.Delay(5000);
                        driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div[2]/div/div[1]/div[6]/div[2]/a")).Click();
                        await Task.Delay(10000);
                        driver.FindElement(By.XPath(@"/html/body/div[1]/div/div/div/div[2]/form/div[2]/button[2]")).Click();
                        await Task.Delay(10000);
                        driver.FindElement(By.XPath(@"//*[@id='keyinput']")).SendKeys("Un123123");
                        await Task.Delay(10000);
                        driver.FindElement(By.XPath(@"/html/body/div[1]/div/div/div/div[2]/form/div[2]/button")).Click();
                    }
                }
                catch (Exception)
                {

                }

                await Task.Delay(10000);
                driver.Navigate().Refresh();
                await Task.Delay(10000);
                if (driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div/div/div[1]/div[1]/div[1]/ul/li/div/div[2]/div/p")).Text == "ایرانسل من")
                {
                    var value = driver.FindElement(By.XPath(@"/html/body/div[1]/div/div[1]/div/div/div[1]/div[2]/div/div[3]/div/div/div[2]/div[2]/ul/li/div/div/div[2]/div[1]/h3")).Text;

                    record = new TblSimCharge
                    {
                        DateTime = DateTime.Now,
                        Value = PersianToEnglish(value)
                    };
                    await _db.TblSimCharges.AddAsync(record);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                LogError($"Error Occured;{DateTime.Now};{Environment.NewLine}{e.Message}");
            }
            finally
            {
                record = null;
            }

        }

        //Persian numbers to english
        public static string PersianToEnglish(string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };

            foreach (var item in persianStr)
            {
                try
                {
                    persianStr = persianStr.Replace(item, LettersDictionary[item]);
                }
                catch (Exception)
                {
                    persianStr = persianStr.Replace(item.ToString(), string.Empty);
                    continue;
                }
            }

            return persianStr;
        }

        //Log errors in file named "log.txt"
        private void LogError(string v)
        {
            using (StreamWriter writetext = new StreamWriter("log.txt", true))
            {
                writetext.WriteLine(v);
            }
        }
    }
}
