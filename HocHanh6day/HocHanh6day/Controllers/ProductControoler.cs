using HocHanh6day.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HocHanh6day.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IHangHoaRepository _hangHoaReposity;

        public ProductController(IHangHoaRepository hangHoaRepository)
        {
            _hangHoaReposity = hangHoaRepository;
        }

        [HttpGet]
        public IActionResult GetAll(string search, double? from, double? to, string sortBy, int page) {
            try
            {
                var listproduct = _hangHoaReposity.GetAll(search, from, to, sortBy, page);
                return Ok(listproduct);
            }
            catch { return BadRequest("Khong tim thay"); }
        }

    }
    

   
}
