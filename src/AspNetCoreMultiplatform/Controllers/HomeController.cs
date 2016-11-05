using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreMultiplatform.Data;
using AspNetCoreMultiplatform.Models.EventViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMultiplatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dataContext;

        public HomeController(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var evt = from e in _dataContext.Events
                      where e.Ends >= DateTime.Now
                      orderby e.Begins
                      select e;

            return View(Mapper.Map<List<EventListModel>>(evt.ToList()));
        }

        public IActionResult Event(int id)
        {
            var evt = _dataContext.Events.FirstOrDefault(e => e.Id == id);
            if(evt == null)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
