using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using LACoreApp.Application.Interfaces;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;
using LACoreApp.Infrastructure.Interfaces;
using LACoreApp.Utilities.Dtos;
using AutoMapper;

namespace LACoreApp.Application.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        private IRepository<Announcement, string> _announcementRepository;
        private IRepository<AnnouncementUser, int> _announcementUserRepository;
        private IMapper _mapper;

        private IUnitOfWork _unitOfWork;

        public AnnouncementService(IRepository<Announcement, string> announcementRepository,
            IRepository<AnnouncementUser, int> announcementUserRepository,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _announcementUserRepository = announcementUserRepository;
            this._announcementRepository = announcementRepository;
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.FindAll()
                        join y in _announcementUserRepository.FindAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annonUser in xy.DefaultIfEmpty()
                        where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var model = _mapper.ProjectTo<AnnouncementViewModel>(query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize)).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool MarkAsRead(Guid userId, string id)
        {
            bool result = false;
            var announ = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                               && x.UserId == userId);
            if (announ == null)
            {
                _announcementUserRepository.Add(new AnnouncementUser
                {
                    AnnouncementId = id,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announ.HasRead == false)
                {
                    announ.HasRead = true;
                    result = true;
                }

            }
            return result;
        }
    }
}
