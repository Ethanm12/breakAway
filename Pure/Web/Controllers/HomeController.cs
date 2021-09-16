using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BreakAway.Entities;
using BreakAway.Models.Home;

namespace BreakAway.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel();
   

            return View(viewModel);
        }
    }
}
