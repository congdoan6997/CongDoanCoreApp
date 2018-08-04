using CongDoanCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CongDoanCoreApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
