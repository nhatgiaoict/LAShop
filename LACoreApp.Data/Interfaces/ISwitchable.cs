using System;
using System.Collections.Generic;
using System.Text;
using LACoreApp.Data.Enums;

namespace LACoreApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
