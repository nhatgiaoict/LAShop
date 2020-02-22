using LACoreApp.Application.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Utilities.Dtos;

namespace LACoreApp.Application.Interfaces
{
    public interface ISlideService : IDisposable
    {
        void Add(SlideViewModel slideVm);

        void Update(SlideViewModel slideVm);

        void Delete(int id);

        List<SlideViewModel> GetAll();

        PagedResult<SlideViewModel> GetAllPaging(string keyword, int page, int pageSize);

        SlideViewModel GetById(int id);

        void SaveChanges();
    }
}
