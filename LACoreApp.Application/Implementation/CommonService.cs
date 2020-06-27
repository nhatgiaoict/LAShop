using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Data.Entities;
using LACoreApp.Infrastructure.Interfaces;
using LACoreApp.Utilities.Constants;
using LACoreApp.Utilities.Dtos;
using LACoreApp.Utilities.Helpers;

namespace LACoreApp.Application.Implementation
{
    public class CommonService : ICommonService
    {
        private readonly IRepository<Footer, string> _footerRepository;
        private readonly IRepository<SystemConfig, string> _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Slide, int> _slideRepository;
        private readonly IMapper _mapper;
        public CommonService(IRepository<Footer, string> footerRepository,
            IRepository<SystemConfig, string> systemConfigRepository,
            IUnitOfWork unitOfWork,
            IRepository<Slide, int> slideRepository,
            IMapper mapper)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
            _mapper = mapper;
        }

        public FooterViewModel GetFooter()
        {
            return _mapper.Map<Footer, FooterViewModel>(_footerRepository.FindSingle(x => x.Id ==
            CommonConstants.DefaultFooterId));
        }

        public List<SlideViewModel> GetSlides(string groupAlias)
        {
            return _mapper.ProjectTo<SlideViewModel>(_slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias)).ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return _mapper.Map<SystemConfig, SystemConfigViewModel>(_systemConfigRepository.FindSingle(x => x.Id == code));
        }
    }
}
