using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Super.Core.Infrastruct.Query2Exp
{
    public enum CompareType
    {
        [Description("等于")]
        Eq = 1,
        [Description("不等于")]
        NotEq = 2,
        [Description("大于")]
        Gt = 3,
        [Description("大于等于")]
        GtEq = 4,
        [Description("小于")]
        Lt = 5,
        [Description("小于等于")]
        LtEq = 6,
        [Description("模糊匹配")]
        Like = 7,
        [Description("左匹配")]
        LeftLike = 8,
        [Description("右匹配")]
        RightLike = 9
    }
}
