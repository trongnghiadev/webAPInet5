using HocHanh6day.Data;
using HocHanh6day.Models;
using HocHanh6day.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HocHanh6day.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiController : ControllerBase
    {
        private readonly ILoaiRepository _loaiRepository;

        public LoaiController(ILoaiRepository loaiRepository)
        {
            _loaiRepository = loaiRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_loaiRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            try
            {
                var data = _loaiRepository.GetByID(id);
                if (data != null)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, LoaiVM loai)
        {
            if(id != loai.MaLoai)
            {
                return BadRequest();
            }
            try
            {
                _loaiRepository.Update(loai);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _loaiRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public IActionResult Add(LoaiMD loai)
        {
            try
            {
               var data = _loaiRepository.Add(loai);
                return Ok(data);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }


    }
}
