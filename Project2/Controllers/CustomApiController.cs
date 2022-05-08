using Microsoft.AspNetCore.Mvc;

namespace Project2.Controllers
{
    public class CustomApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> motorbikes()
        {
            var motorbikes = await DatabaseOperations.AllMotorbikes();

            return Json(motorbikes);
        }

        public async Task<IActionResult> motorbike(int id)
        {
            var motorbike = await DatabaseOperations.SelectMotorbike(id);

            return Json(motorbike);
        }

        public async Task<IActionResult> Offices()
        {
            var offices = await DatabaseOperations.AllOffices();

            return Json(offices);
        }

        public async Task<IActionResult> Office(int id)
        {
            var office = await DatabaseOperations.GetSpecificOffice(id);

            return Json(office);
        }
    }
}
