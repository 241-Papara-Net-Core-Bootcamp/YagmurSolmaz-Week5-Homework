
using DocumentFormat.OpenXml.Office.CustomUI;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UdemyAuthServer.Core.Services;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        private readonly IUserService _userService;

        public HangFireController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("RetrieveDataFromAPI")]
        public IActionResult RetrieveDataFromAPI()
        {
            //Logic to retrieve data from external api 

            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("https://jsonplaceholder.typicode.com/posts");
            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }


            var items = JsonConvert.DeserializeObject(jsonString);
            return Ok();

        }



        [HttpGet]
        [Route("RetrieveData")]
        public IActionResult RetrieveData()
        {
            RecurringJob.AddOrUpdate(() => RetrieveDataFromAPI(), "*/10 * * * * *");
            return Ok();
        }

    }
}