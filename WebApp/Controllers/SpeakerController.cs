using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly IAsyncRepository<Speaker> _speakerRepository;

        public SpeakerController(IAsyncRepository<Speaker> speakerRepository)
        {
            _speakerRepository = speakerRepository;
        }

        // GET: Speaker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Speaker/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Speaker/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Speaker/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Speaker/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Speaker/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Speaker/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Speaker
        public async Task<ActionResult> Index()
        {
            var speakers = await _speakerRepository.ListAllAsync();

            return View(speakers);
        }
    }
}