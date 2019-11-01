﻿using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Application.ViewModels.Blog;
using LACoreApp.Application.ViewModels.Common;
using LACoreApp.Utilities.Dtos;

namespace LACoreApp.Application.Interfaces
{
    public interface IBlogService
    {
        BlogViewModel Add(BlogViewModel product);

        void Update(BlogViewModel product);

        void Delete(int id);

        List<BlogViewModel> GetAll();

        PagedResult<BlogViewModel> GetAllPaging(int?categoryId, string keyword, int page, int pageSize);

        List<BlogViewModel> GetLastest(int top);

        List<BlogViewModel> GetHotBlog(int top);

        List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow);

        List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        List<BlogViewModel> GetList(string keyword);

        List<BlogViewModel> GetReatedBlogs(int id, int top);

        List<string> GetListByName(string name);

        BlogViewModel GetById(int id);

        void Save();

        List<TagViewModel> GetListTagById(int id);

        TagViewModel GetTag(string tagId);

        void IncreaseView(int id);

        List<BlogViewModel> GetListByTag(string tagId, int page, int pagesize, out int totalRow);

        List<TagViewModel> GetListTag(string searchText);
    }
}
