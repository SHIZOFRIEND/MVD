using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVDAPI.Models;

namespace MVDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MVDController : ControllerBase
    {
        public readonly ApplicationDbContext _context;
        public MVDController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetQuery()
        {
            try
            {
                var sotrudnikiList = _context.Sotrudniki.ToList();
                return Ok(sotrudnikiList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
