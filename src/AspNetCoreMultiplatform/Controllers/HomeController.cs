using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreMultiplatform.Data;
using AspNetCoreMultiplatform.Models.EventViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
            var query = from e in _dataContext.Events
                        where e.Ends >= DateTime.Now
                        orderby e.Begins
                        select e;

            var events = Mapper.Map<List<EventListModel>>(query.ToList());

            return View(events);
        }

        public IActionResult Event(int id)
        {
            var evt = _dataContext.Events.FirstOrDefault(e => e.Id == id);
            if(evt == null)
            {
                return NotFound();
            }

            var model = Mapper.Map<EventViewModel>(evt);

            return View(model);
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            var model = new EventViewModel();

            if(id != null)
            {
                var query = from e in _dataContext.Events
                            where e.Id == id &&
                                  e.Owner.UserName == User.Identity.Name
                            select e;

                var evt = query.FirstOrDefault();
                if (evt == null)
                {
                    return NotFound();
                }

                model = Mapper.Map<EventViewModel>(evt);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EventViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var evt = new Event();

            if(model.Id != 0)
            {
                var query = from e in _dataContext.Events
                            where e.Id == model.Id &&
                                  e.Owner.UserName == User.Identity.Name
                            select e;

                evt = query.FirstOrDefault();
                if (evt == null)
                {
                    return NotFound();
                }
            }

            Mapper.Map(model, evt);

            if(model.Id == 0)
            {
                evt.Owner = _dataContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                _dataContext.Events.Add(evt);
            }

            _dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
