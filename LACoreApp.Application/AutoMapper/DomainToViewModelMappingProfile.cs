﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;

namespace LACoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2);

            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);

            CreateMap<Blog, BlogViewModel>().MaxDepth(2);
            CreateMap<BlogCategory, BlogCategoryViewModel>().MaxDepth(2);
            CreateMap<BlogTag, BlogTagViewModel>().MaxDepth(2);
            CreateMap<Slide, SlideViewModel>().MaxDepth(2);
            CreateMap<SystemConfig, SystemConfigViewModel>().MaxDepth(2);
            CreateMap<Footer, FooterViewModel>().MaxDepth(2);

            CreateMap<Feedback, FeedbackViewModel>().MaxDepth(2);
            CreateMap<Contact, ContactViewModel>().MaxDepth(2);
            CreateMap<Page, PageViewModel>().MaxDepth(2);

        }
    }
}
