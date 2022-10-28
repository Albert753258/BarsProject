using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using FastReport;
using FastReport.Data.JsonConnection;
using FastReport.Export.Pdf;
using FastReport.Utils;
using InformixConnector;
using Newtonsoft.Json;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        class CountClass
        {
            public string count;
            public List<Human> humans;

            public CountClass(string count, List<Human> humans)
            {
                this.count = count;
                this.humans = humans;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchHuman(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo, string page, string limit)
        {
            SQLRepository.Pair<List<Human>> pair = MvcApplication.repository.searchHuman(surname, fname, patronymic, birthdayFrom,
                birthdayTo, Convert.ToInt32(page), limit);
            string toReturn = JsonConvert.SerializeObject(new CountClass(pair.count, pair.humans));
            return Content(toReturn);
        }

        public ActionResult AddHuman(string surname, string fname, string patronymic, string birthday, string confirm)
        {
            //MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday));
            return Content(MvcApplication.repository.addHuman(new Human(surname, fname, patronymic, birthday), confirm));
        }
        public ActionResult DelHuman(string id)
        {
            MvcApplication.repository.delHuman(id);
            return Content("");
        }
        public ActionResult EditHuman(string id, string surname, string fname, string patronymic, string birthday, string confirm)
        {
            
            return Content(MvcApplication.repository.editHuman(new Human(int.Parse(id), surname, fname, patronymic, birthday), confirm));
        }
        public ActionResult GenReport(string surname, string fname, string patronymic, string birthdayFrom, string birthdayTo)
        {
            SQLRepository.Pair<List<Human>> pair = MvcApplication.repository.searchHuman(surname, fname, patronymic, birthdayFrom,
                birthdayTo, 0, null);
            List<Human> humans = pair.humans;
            MemoryStream stream = new MemoryStream();
            Report report = new Report();
            DataTable table = new DataTable();
            DataSet set = new DataSet();
            table.Columns.Add("id", typeof(string));
            table.Columns.Add("surname", typeof(string));
            table.Columns.Add("fname", typeof(string));
            table.Columns.Add("patronymic", typeof(string));
            table.Columns.Add("birthday", typeof(string));
            for (int i = 0; i < humans.Count; i++)
            {
                Human human = humans[i];
                table.Rows.Add(i + 1, human.surname, human.fname, human.patronymic, human.birthday);
            }
            set.Tables.Add(table);
            report.Load(@"C:\Users\albert\Documents\test.frx");
            report.Dictionary.Clear();
            //DataSourceCollection sourceBase = report.Dictionary.DataSources;
            report.RegisterData(table, "JSON");
            report.GetDataSource("JSON").Enabled = true;
            //((Table)report.FindObject("Table1")).DataSource = report.Dictionary.DataSources[0];
            report.Prepare();
            report.Export(new PDFExport(), stream);
            stream.Position = 0;
            return File(stream, "application/pdf");
        }
    }
}