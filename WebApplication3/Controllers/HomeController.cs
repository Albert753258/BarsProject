using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FastReport;
using FastReport.Export;
using FastReport.Data;
using InformixConnector;
using Newtonsoft.Json;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchHuman(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo)
        {
            return Content(MvcApplication.repository.searchHuman(surname, fname, patronymic, birthdayFrom, birthdayTo));
        }

        public ActionResult AddHuman(string surname, string fname, string patronymic, string birthday)
        {
            //MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday));
            return Content(MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday)));
        }
        public ActionResult DelHuman(string id)
        {
            MvcApplication.repository.delHuman(id);
            return Content("");
        }
        public ActionResult EditHuman(string id, string surname, string fname, string patronymic, string birthday)
        {
            MvcApplication.repository.editHuman(new Human(int.Parse(id), surname, fname, patronymic, birthday));
            return Content("");
        }
        public ActionResult GenReport()
        {
            string json = MvcApplication.repository.searchHuman("", "", "", "", "");
            Human[] list = JsonConvert.DeserializeObject<Human[]>(json);
            Report report = new Report();
            report.Load("C:\\Users\\albert\\RiderProjects\\WebApplication3\\WebApplication3\\Controllers\\Untitled.frx");
            report.RegisterData(new ArrayList(list), "Humans");
            report.Prepare();
            
            report.Export(new ExportBase(), "file.html");
            return Content("");
        }
    }
}
//todo печать