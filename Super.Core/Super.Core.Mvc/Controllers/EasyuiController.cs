﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Super.Core.Mvc.Controllers
{
    public class EasyuiController : Controller
    {




        public IActionResult Index()
        {
            throw new Exception("yyyyg");
            return View();
        }

        [HttpPost]
        public IActionResult Post([FromBody]TestModel model)
        {
            return this.Ok(model);
        }

    }

    public class TestModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public List<string> cars { get; set; }
    }

}