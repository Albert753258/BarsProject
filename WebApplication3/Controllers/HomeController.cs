using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult SearchHuman(string surname, string fname, string patronymic, string birthday)
        {
            //todo matches
            return Content(MvcApplication.repository.searchHuman(surname, fname, patronymic, birthday));
            //return Content(System.IO.File.ReadAllText("C:\\Users\\albert\\RiderProjects\\WebApplication3\\WebApplication3\\BarsProject\\app\\data\\users.json"));
            //return Content("{'success': true, 'users': [{'userID': 1, 'name': 'Вася', 'surname': 'Иванов', 'birthday': '10/08/1991', 'patronymic': 'Иванович'}]}");
        }

        public ActionResult AddHuman(string surname, string fname, string patronymic, string birthday)
        {
            //MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday));
            return Content(MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday)) + "");
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
    }
}
//todo изайн кнопок, формат даты через точк, обновление в сторе, id при вставке
//{
// 'success': true, 'users': [{'userID': 1, 'name': 'Вася', 'surname': 'Иванов', 'birthday': '10/08/1991', 'patronymic': 'Иванович'}