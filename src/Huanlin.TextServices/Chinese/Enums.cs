using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huanlin.TextServices.Chinese
{
    // 中文字出現頻率
    public enum ChineseCharFrequence
    {
        Common = 0,             // 常用字
        LessThanCommon = 1,     // 次常用字
        Rare = 2                // 罕見字
    }
}
