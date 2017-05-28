using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Common
{
    public enum EnumResult
    {
        [Description("Pass")]
        Pass = 1,
        [Description("Fail")]
        Fail = 0,
        [Description("None")]
        None = 2,
    }
}
