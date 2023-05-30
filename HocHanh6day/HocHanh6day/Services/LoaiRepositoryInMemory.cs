using HocHanh6day.Data;
using HocHanh6day.Models;
using System.Collections.Generic;
using System.Linq;

namespace HocHanh6day.Services
{

    public class LoaiRepositoryInMemory : ILoaiRepository
    {
        static List<LoaiVM> loais = new List<LoaiVM>
        {
            new LoaiVM{MaLoai = 1, TenLoai = "TiVi"},
            new LoaiVM{MaLoai = 2, TenLoai = "LapTop"},
            new LoaiVM{MaLoai = 3, TenLoai = "Tủ Lạnh"},
            new LoaiVM{MaLoai = 4, TenLoai = "Điều Hòa"}
        };
        public LoaiVM Add(LoaiMD loai)
        {
            var _loai = new LoaiVM
            {
                MaLoai = loais.Max(lo => lo.MaLoai) + 1,
                TenLoai = loai.TenLoai
            };
            loais.Add(_loai); 
            return _loai;
        }

        public void Delete(int id)
        {
            var _loai = loais.SingleOrDefault(lo => lo.MaLoai == id);
            if(_loai != null)
            {
                loais.Remove(_loai);
            }
        }

        public List<LoaiVM> GetAll()
        {
            return loais;
        }

        public LoaiVM GetByID(int id)
        {
            return loais.SingleOrDefault(lo => lo.MaLoai == id);    
        }

        public void Update(LoaiVM loai)
        {
          var _loai = loais.SingleOrDefault(lo => lo.MaLoai == loai.MaLoai);
            if (_loai != null)
            {
                _loai.TenLoai = loai.TenLoai;
            }
        }
    }
}
