using HocHanh6day.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HocHanh6day.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hanghoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetALl()
        {
            return Ok(hanghoas);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var hangHoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hangHoa == null)
                {
                    return NotFound();  
                }
                return Ok(hangHoa);
            } catch {
                return BadRequest();
            }
      
        }
        [HttpPost]
        public IActionResult Create(HangHoaVM hangHoaVM)
        {
            var hanghoa = new HangHoa
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia
            };
            hanghoas.Add(hanghoa);
            return Ok(new
            {
                Succsess = true, Data = hanghoa
            });
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, HangHoa hangHoaEdit)
        {
            try
            {
                var hangHoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hangHoa == null)
                {
                    return NotFound();
                }
                if(id != hangHoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }
                // update
                hangHoa.TenHangHoa = hangHoaEdit.TenHangHoa;
                hangHoa.DonGia = hangHoaEdit.DonGia;
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var hangHoa = hanghoas.SingleOrDefault(hh => hh.MaHangHoa == Guid.Parse(id));
                if (hangHoa == null)
                {
                    return NotFound();
                }
                if (id != hangHoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }
                // update
                hanghoas.Remove(hangHoa);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        
    }
}
