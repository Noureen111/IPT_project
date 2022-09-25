using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Timers;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "Service1";
        }
        static async void GetAllProductsAsync(string path)
        {
            // List<Product> products = null;

            HttpClient client = new HttpClient();
            //client.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsAsync<string>();
                res = res.Replace("element", " ");
                res = Regex.Replace(res, @"[^0-9a-zA-Z:,]+", "");
                res = res.Replace(",:", Environment.NewLine);
                using (StreamWriter writer = new StreamWriter("C:/Users/Amna/source/repos/WindowsService1/WindowsService1/result.txt"))
                {
                    writer.WriteLine(res);
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter("C:/Users/Amna/source/repos/WindowsService1/WindowsService1/result.txt"))
                {
                    writer.WriteLine("nothing but created");
                }
            }


        }
        protected override void OnStart(string[] args)
        {
            GetAllProductsAsync("https://localhost:44305/api/voteCasteds/counting");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            GetAllProductsAsync("https://localhost:44305/api/voteCasteds/counting");

        }
        protected override void OnStop()
        {
        }
    }
}
