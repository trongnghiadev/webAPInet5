using HocHanh6day.Models;
using System.Collections.Generic;

namespace HocHanh6day.Services
{
    public interface ILoaiRepository
    {
        List<LoaiVM> GetAll();
        LoaiVM GetByID(int id);
        LoaiVM Add(LoaiMD loai);
        void Update(LoaiVM loai);
        void Delete(int id);
    }
}
