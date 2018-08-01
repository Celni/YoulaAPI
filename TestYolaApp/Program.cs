using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoulaApi;
using YoulaApi.Models;

namespace TestYolaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new WebProxy("localhost:8888");
            var settings = new ClientSettings("", "", "", 47.640068, -122.129858, null, "", proxy);

            var client = new YoulaClient(settings);


            Console.ReadKey();
        }
    }
}