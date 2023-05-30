using HocHanh6day.Data;
using HocHanh6day.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace HocHanh6day.Services
{
    public class HangHoaRepository : IHangHoaRepository
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 2;
        public HangHoaRepository(MyDbContext context) 
        {
            _context = context;
        }
        public List<HangHoaModel> GetAll(string search, double? from, double? to, string sortBy, int page = 1)
        {
            #region fiillering
            var allProducts = _context.HangHoas.Include(hh => hh.Loai).AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                allProducts=  allProducts.Where(hh => hh.TenHh.Contains(search));
            }
            if(from.HasValue) 
            {
                allProducts = allProducts.Where(hh => hh.DonGia >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(hh => hh.DonGia <= to);
            }
            #endregion

            #region sorting
            allProducts = allProducts.OrderBy(hh => hh.TenHh);
            if(!string.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "tenhh_desc": allProducts = allProducts.OrderByDescending(hh => hh.TenHh); break;
                    case "gia_asc": allProducts = allProducts.OrderBy(hh => hh.DonGia); break;
                    case "gia_desc": allProducts = allProducts.OrderByDescending(hh => hh.DonGia); break;
                }



            };
            #endregion

            #region Paging
            /*
            allProducts = allProducts.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = allProducts.Select(hh => new HangHoaModel
            {
                MaHangHoa = hh.MaHh,
                TenHangHoa = hh.TenHh,
                DonGia = hh.DonGia,
                TenLoai = hh.Loai.TenLoai
            });    
             return result.ToList();
             */

            var result = PaginatedList<HocHanh6day.Data.HangHoa>.Create(allProducts, page, PAGE_SIZE);

            #endregion
            return result.Select(hh => new HangHoaModel
            {
                MaHangHoa = hh.MaHh,
                TenHangHoa = hh.TenHh,
                DonGia = hh.DonGia,
                TenLoai = hh.Loai.TenLoai
            }).ToList();


        }
    }
}
