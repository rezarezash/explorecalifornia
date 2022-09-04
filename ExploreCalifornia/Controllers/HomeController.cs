using ExploreCalifornia.Filters;
using ExploreCalifornia.Infrastructure;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ExploreCalifornia.Controllers
{
    [ResponseCache(Duration = 30)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TimerManager _timerManager;
        private readonly Channel<FileUploadMessage> _channel;

        public HomeController(ILogger<HomeController> logger, TimerManager timerManager,

            Channel<FileUploadMessage> channel)
        {
            _logger = logger;
            _timerManager = timerManager;
            _channel = channel;
        }

        [ServiceFilter(typeof(TimerFilter))]
        public IActionResult Index()
        {
            //var channelWriter = _channel.Writer;
            //Enumerable.Range(0, 1000).ForEach(async(c) =>
            //{
            //    await channelWriter.WriteAsync(new FileUploadMessage
            //    {
            //        FileName = $"File_{c}",
            //        FilePath = $"C:\\TempFoler\\"
            //    });
            //    await Task.Delay(100);
            //});

            //_logger.LogInformation("In HomeController/ index");
            return View();
        }

        public IActionResult Privacy()
        {
            throw new ArgumentNullException("test error");
            //return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
