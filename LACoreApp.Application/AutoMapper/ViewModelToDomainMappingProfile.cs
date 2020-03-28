using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Application.ViewModels.Product;
using LACoreApp.Application.ViewModels.System;
using LACoreApp.Data.Entities;

namespace LACoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<ProductViewModel, Product>()
           .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
           c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
           c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<AppUserViewModel, AppUser>()
            .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
            c.Email, c.PhoneNumber, c.Avatar, c.Status));

            CreateMap<PermissionViewModel, Permission>()
            .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));



            CreateMap<BillViewModel, Bill>()
              .ConstructUsing(c => new Bill(c.Id, c.CustomerName, c.CustomerAddress,
              c.CustomerMobile, c.CustomerMessage, c.BillStatus,
              c.PaymentMethod, c.Status, c.CustomerId));

            CreateMap<BillDetailViewModel, BillDetail>()
              .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId,
              c.Quantity, c.Price, c.ColorId, c.SizeId));


            CreateMap<ContactViewModel, Contact>()
                .ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

            CreateMap<FeedbackViewModel, Feedback>()
                .ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));

            CreateMap<PageViewModel, Page>()
             .ConstructUsing(c => new Page(c.Id, c.Name, c.Alias, c.Content, c.Status));


            CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.UserId, c.Status));

            CreateMap<AnnouncementUserViewModel, AnnouncementUser>()
                .ConstructUsing(c => new AnnouncementUser(c.AnnouncementId, c.UserId, c.HasRead));

            CreateMap<FunctionViewModel, Function>()
                .ConstructUsing(c => new Function(c.Name, c.URL, c.ParentId, c.IConCss, c.SortOrder));

            CreateMap<BlogViewModel, Blog>().ConstructUsing(c => new Blog(c.Name, c.CategoryId, c.Image, c.Description, c.Content,
                c.HomeFlag, c.HotFlag, c.Tags, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<BlogCategoryViewModel, BlogCategory>().ConstructUsing(c => new BlogCategory(c.Name, c.Description,
                c.ParentId, c.HomeOrder, c.Image, c.HomeFlag, c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias,
                c.SeoKeywords, c.SeoDescription));

            CreateMap<SlideViewModel, Slide>().ConstructUsing(c =>
                new Slide(c.Id, c.Name, c.GroupAlias, c.Description, c.Image, c.Url, c.DisplayOrder, c.Status, c.Content));

            CreateMap<MenuViewModel, Menu>()
               .ConstructUsing(c => new Menu(c.Name, c.URL, c.ParentId, c.Group, c.SortOrder, c.Status));
        }
    }
}
