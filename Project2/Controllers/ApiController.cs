using Microsoft.AspNetCore.Mvc;
using Project2.Models;

namespace Project2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("swagger");
        }


        [HttpGet("motorbike")]
        public async Task<IActionResult> Get()
        {
            List<Motorbike> aa = await DatabaseOperations.AllMotorbikes();
            return Ok(aa);
        }

        

        [HttpGet("motorbike/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Motorbike aa = await DatabaseOperations.SelectMotorbike(id);
            return Ok(aa);
        }

        [HttpGet("office")]
        public async Task<IActionResult> GetOffice()
        {
            List<Office> aa = await DatabaseOperations.AllOffices();
            return Ok(aa);
        }

        [HttpGet("office/{id}")]
        public async Task<IActionResult> GetOffice(int id)
        {
            Office aa = await DatabaseOperations.GetSpecificOffice(id);
            return Ok(aa);
        }

    }
}
