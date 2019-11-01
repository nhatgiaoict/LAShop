using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Application.ViewModels.Common;

namespace LACoreApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();
        List<SlideViewModel> GetSlides(string groupAlias);
        SystemConfigViewModel GetSystemConfig(string code);
    }
}
