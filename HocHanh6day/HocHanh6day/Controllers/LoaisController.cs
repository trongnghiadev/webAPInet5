using HocHanh6day.Data;
using HocHanh6day.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HocHanh6day.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LoaisController(MyDbContext context) 
        { 

            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dsloai = _context.Loais.ToList();
            return Ok(dsloai);
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var loai = _context.Loais.SingleOrDefault(loai => loai.MaLoai == id );
            if(loai == null)
            {
                return NotFound();
            }
            return Ok(loai);
        }

        [HttpPost]
        public IActionResult CreateNew(LoaiMD model)
        {
            try
            {
                var loai = new Loai
                {
                    TenLoai = model.TenLoai
                };
                _context.Add(loai);
                _context.SaveChanges();
                return Ok(loai);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, LoaiMD model)
        {
            var loai = _context.Loais.SingleOrDefault(loai => loai.MaLoai == id);
            if (loai == null)
            {
                return NoContent();
            }
            else
            {
                loai.TenLoai = model.TenLoai;
                _context.SaveChanges();
                return Ok();
            }
            
        }


    }
}
