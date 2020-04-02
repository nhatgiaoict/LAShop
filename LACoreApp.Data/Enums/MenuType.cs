using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LACoreApp.Data.Enums
{
    public enum MenuType
    {
        [Description("Tin tức")]
        Blog = 1,
        [Description("Sản phẩm")]
        Product = 2,
        [Description("Tùy chỉnh")]
        Customize = 3
    }
}
