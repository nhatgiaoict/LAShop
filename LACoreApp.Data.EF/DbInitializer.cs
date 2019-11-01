using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LACoreApp.Data.Entities;
using LACoreApp.Data.Enums;
using LACoreApp.Utilities.Constants;

namespace LACoreApp.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
                });
                await _roleManager.CreateAsync(new AppRole()
                {
                    Name = "Customer",
                    NormalizedName = "Customer",
                    Description = "Customer"
                });
            }
            if (!_userManager.Users.Any())
            {
                await _userManager.CreateAsync(new AppUser()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "nhatgiaoict@gmail.com",
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = Status.Active
                }, "123@123Aa");
                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            if (!_context.Contacts.Any())
            {
                _context.Contacts.Add(new Contact()
                {
                    Id = CommonConstants.DefaultContactId,
                    Address = "No 36 Lane 133 Nguyen Phong Sac Cau Giay",
                    Email = "nhatgiaoict@gmail.com",
                    Name = "Linh Anh Shop",
                    Phone = "0973 489 705",
                    Status = Status.Active,
                    Website = "http://pandashop.com",
                    Lat = 21.0435009,
                    Lng = 105.7894758
                });
            }
            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<Function>()
                {
                    new Function() {Id = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"), Name = "System",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {Id = new Guid("df32522b-da5d-4c80-9e30-5d97d07f377c"), Name = "Role",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                    new Function() {Id = new Guid("eb0f21b2-b608-4006-8d70-57fb35a8db00"), Name = "Function",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                    new Function() {Id = new Guid("1840bd0e-7638-46ae-9a9e-8c34356267db"), Name = "User",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                    new Function() {Id = new Guid("bc263398-b5c6-4ae0-ac0a-a33dc52039f8"), Name = "Activity",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                    new Function() {Id = new Guid("9a32d14f-4116-4dbd-81bc-d8a947211a00"), Name = "Error",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                    new Function() {Id = new Guid("c8363ad5-b57b-4115-b25a-bf32771eaa74"), Name = "Configuration",ParentId = new Guid( "7a52b8c1-c0e0-4c5a-970a-417d86f561dd"),SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },

                    new Function() {Id = new Guid("d6fc13c8-b1b0-4c60-9b02-df4ac94357f8"),Name = "Product Management",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                    new Function() {Id = new Guid("f07615f2-f278-457a-89e2-720f26225f4d"),Name = "Category",ParentId = new Guid("d6fc13c8-b1b0-4c60-9b02-df4ac94357f8"),SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = new Guid("68677f8d-2751-44fb-94e0-801bb01d9cf5"),Name = "Product",ParentId = new Guid("d6fc13c8-b1b0-4c60-9b02-df4ac94357f8"),SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = new Guid("a8f5a45e-51a1-4ac7-b230-982a988c1f62"),Name = "Bill",ParentId = new Guid("d6fc13c8-b1b0-4c60-9b02-df4ac94357f8"),SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },

                    new Function() {Id = new Guid("19622f24-1ef4-4fcf-8fbf-612861fabe4e"),Name = "Content",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                    new Function() {Id = new Guid("ced6a65a-319a-4029-809a-359f31b62049"),Name = "Blog",ParentId = new Guid("19622f24-1ef4-4fcf-8fbf-612861fabe4e"),SortOrder = 1,Status = Status.Active,URL = "/admin/blog/index",IconCss = "fa-table"  },
                    new Function() {Id = new Guid("5a26a7f5-faab-42c0-a694-d9d88b6dd9d2"),Name = "Page",ParentId = new Guid("19622f24-1ef4-4fcf-8fbf-612861fabe4e"),SortOrder = 2,Status = Status.Active,URL = "/admin/page/index",IconCss = "fa-table"  },

                    new Function() {Id = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),Name = "Utilities",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("9931b1bf-b9c9-4b37-a1bc-a9a07d83ecd7"),Name = "Footer",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("9856f262-9593-4300-964b-926588e4748f"),Name = "Feedback",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("c29277f5-c15f-4f31-ac07-d284691bb42d"),Name = "Announcement",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("b68e432b-c352-48ef-bbef-8507ac839ef1"),Name = "Contact",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("5f5d8794-c99d-48ed-a546-eb9390a6a49a"),Name = "Slide",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                    new Function() {Id = new Guid("363a3eeb-dda4-4c0d-9574-1e9be2316355"),Name = "Advertisement",ParentId = new Guid("0cbf764b-e4ab-44ed-9d87-d616348dc849"),SortOrder = 6,Status = Status.Active,URL = "/admin/advertistment/index",IconCss = "fa-clone"  },

                    new Function() {Id = new Guid("b8477d20-d584-4572-8fc1-9e0e2e2dab99"),Name = "Report",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = new Guid("34bd292a-3400-4947-9937-1596958f93dc"),Name = "Revenue report",ParentId = new Guid("b8477d20-d584-4572-8fc1-9e0e2e2dab99"),SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = new Guid("c7a035ac-5ebc-4442-95fe-8cadc40d68a7"),Name = "Visitor Report",ParentId = new Guid("b8477d20-d584-4572-8fc1-9e0e2e2dab99"),SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = new Guid("914e6813-1c51-4d47-b4ed-bdc904822650"),Name = "Reader Report",ParentId = new Guid("b8477d20-d584-4572-8fc1-9e0e2e2dab99"),SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  },
                });
            }

            if (_context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
                string content = "Footer";
                _context.Footers.Add(new Footer()
                {
                    Id = CommonConstants.DefaultFooterId,
                    Content = content
                });
            }

            if (!_context.Colors.Any())
            {
                List<Color> listColor = new List<Color>()
                {
                    new Color() {Name="Black", Code="#000000" },
                    new Color() {Name="White", Code="#FFFFFF"},
                    new Color() {Name="Red", Code="#ff0000" },
                    new Color() {Name="Blue", Code="#1000ff" },
                };
                _context.Colors.AddRange(listColor);
            }
            if (!_context.AdvertisementPages.Any())
            {
                List<AdvertisementPage> pages = new List<AdvertisementPage>()
                {
                    new AdvertisementPage() {Id="home", Name="Home",AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="home-left",Name="Bên trái"}
                    } },
                    new AdvertisementPage() {Id="product-cate", Name="Product category" ,
                        AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="product-cate-left",Name="Bên trái"}
                    }},
                    new AdvertisementPage() {Id="product-detail", Name="Product detail",
                        AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="product-detail-left",Name="Bên trái"}
                    } },

                };
                _context.AdvertisementPages.AddRange(pages);
            }


            if (!_context.Slides.Any())
            {
                List<Slide> slides = new List<Slide>()
                {
                    new Slide() {Name="Slide 1",Image="/client-side/images/slider/slide-1.jpg",Url="#",DisplayOrder = 0,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/slider/slide-2.jpg",Url="#",DisplayOrder = 1,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/slider/slide-3.jpg",Url="#",DisplayOrder = 2,GroupAlias = "top",Status = true },

                    new Slide() {Name="Slide 1",Image="/client-side/images/brand1.png",Url="#",DisplayOrder = 1,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/brand2.png",Url="#",DisplayOrder = 2,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/brand3.png",Url="#",DisplayOrder = 3,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 4",Image="/client-side/images/brand4.png",Url="#",DisplayOrder = 4,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 5",Image="/client-side/images/brand5.png",Url="#",DisplayOrder = 5,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 6",Image="/client-side/images/brand6.png",Url="#",DisplayOrder = 6,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 7",Image="/client-side/images/brand7.png",Url="#",DisplayOrder = 7,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 8",Image="/client-side/images/brand8.png",Url="#",DisplayOrder = 8,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 9",Image="/client-side/images/brand9.png",Url="#",DisplayOrder = 9,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 10",Image="/client-side/images/brand10.png",Url="#",DisplayOrder = 10,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 11",Image="/client-side/images/brand11.png",Url="#",DisplayOrder = 11,GroupAlias = "brand",Status = true },
                };
                _context.Slides.AddRange(slides);
            }


            if (!_context.Sizes.Any())
            {
                List<Size> listSize = new List<Size>()
                {
                    new Size() { Name="XXL" },
                    new Size() { Name="XL"},
                    new Size() { Name="L" },
                    new Size() { Name="M" },
                    new Size() { Name="S" },
                    new Size() { Name="XS" }
                };
                _context.Sizes.AddRange(listSize);
            }

            if (!_context.ProductCategories.Any())
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name="Men shirt",SeoAlias="men-shirt",ParentId = null,Status=Status.Active,SortOrder=1,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Product 1",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-1",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 2",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-2",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 3",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-3",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 4",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-4",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 5",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-5",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                        }
                    },
                    new ProductCategory() { Name="Women shirt",SeoAlias="women-shirt",ParentId = null,Status=Status.Active ,SortOrder=2,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Product 6",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-6",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 7",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-7",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 8",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-8",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 9",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-9",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 10",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-10",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                        }},
                    new ProductCategory() { Name="Men shoes",SeoAlias="men-shoes",ParentId = null,Status=Status.Active ,SortOrder=3,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Product 11",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-11",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 12",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-12",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 13",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-13",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 14",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-14",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 15",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-15",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                        }},
                    new ProductCategory() { Name="Women shoes",SeoAlias="women-shoes",ParentId = null,Status=Status.Active,SortOrder=4,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Product 16",DateCreated=DateTime.Now, Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-16",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 17",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-17",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 18",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-18",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 19",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-19",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                            new Product(){Name = "Product 20",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-20",Price = 1000,Status = Status.Active,OriginalPrice = 1000},
                        }}
                };
                _context.ProductCategories.AddRange(listProductCategory);
            }

            if (!_context.SystemConfigs.Any(x => x.Id == "HomeTitle"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeTitle",
                    Name = "Home's title",
                    Value1 = "Linh Anh Shop home",
                    Status = Status.Active
                });
            }
            if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaKeyword"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaKeyword",
                    Name = "Home Keyword",
                    Value1 = "shopping, sales",
                    Status = Status.Active
                });
            }
            if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaDescription"))
            {
                _context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaDescription",
                    Name = "Home Description",
                    Value1 = "Home LA",
                    Status = Status.Active
                });
            }
            await _context.SaveChangesAsync();

        }
    }
}
