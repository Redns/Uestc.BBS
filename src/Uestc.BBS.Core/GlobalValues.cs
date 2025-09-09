namespace Uestc.BBS.Core
{
    public static class GlobalValues
    {
        /// <summary>
        /// 用户等级对应的徽章背景颜色（ARGB）
        /// </summary>
        public static readonly string[] UserLevelBadgeBackgroundBrushes =
        [
            "#FFF44336", // Lv.0  红
            "#FF8383A4", // Lv.1  灰
            "#FFFF9800", // Lv.2  橙
            "#FFF57C00", // Lv.3
            "#FFD3A243", // Lv.4  黄
            "#FFE6B422", // Lv.5
            "#FF4CAF50", // Lv.6  绿
            "#FF388E3C", // Lv.7
            "#FF00BCD4", // Lv.8  青
            "#FF00ACC1", // Lv.9
            "#FF2196F3", // Lv.10 蓝
            "#FF1188E5", // Lv.11
            "#FF673AB7", // Lv.12 紫
            "#FF512DA8", // Lv.13
        ];

        /// <summary>
        /// 用户组对应的徽章背景颜色（ARGB）
        /// </summary>
        public static readonly Dictionary<string, string> UserVisibleGroups = new()
        {
            { "站长", "#FF1565C0" }, // 蓝
            { "组织机构", "#FF1565C0" },
            { "成电校友", "#FF1976D2" },
            { "版主", "#FF009688" }, // 青绿
            { "超级版主", "#FF00796B" },
            { "实习版主", "#FF26A69A" },
            { "退休版主", "#FF00897B" },
            { "管理员", "#FF00BCD4" }, // 蓝绿
            { "星辰工作室", "#FF4CAF50" }, // 绿
            { "清水河畔VIP", "#FFFECE11" }, // 黄
            { "禁言中", "#FFF44336" }, // 红
        };
    }
}
