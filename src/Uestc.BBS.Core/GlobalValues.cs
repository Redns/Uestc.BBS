namespace Uestc.BBS.Core
{
    public static class GlobalValues
    {
        /// <summary>
        /// 用户等级对应的徽章背景颜色（ARGB）
        /// </summary>
        public static readonly uint[] UserLevelBadgeBackgroundBrushes =
        [
            0xFFF44336, // Lv.0  红
            0xFF8383A4, // Lv.1  灰
            0xFFFF9800, // Lv.2  橙
            0xFFF57C00, // Lv.3
            0xFFD3A243, // Lv.4  黄
            0xFFE6B422, // Lv.5
            0xFF4CAF50, // Lv.6  绿
            0xFF388E3C, // Lv.7
            0xFF00BCD4, // Lv.8  青
            0xFF00ACC1, // Lv.9
            0xFF2196F3, // Lv.10 蓝
            0xFF1188E5, // Lv.11
            0xFF673AB7, // Lv.12 紫
            0xFF512DA8, // Lv.13
        ];

        /// <summary>
        /// 用户组对应的徽章背景颜色（ARGB）
        /// </summary>
        public static readonly Dictionary<string, uint> UserVisibleGroups = new()
        {
            { "站长", 0xFF1565C0 }, // 蓝
            { "组织机构", 0xFF1565C0 },
            { "成电校友", 0xFF1976D2 },
            { "版主", 0xFF009688 }, // 青绿
            { "超级版主", 0xFF00796B },
            { "实习版主", 0xFF26A69A },
            { "退休版主", 0xFF00897B },
            { "管理员", 0xFF00BCD4 }, // 蓝绿
            { "星辰工作室", 0xFF4CAF50 }, // 绿
            { "清水河畔VIP", 0xFFFECE11 }, // 黄
            { "禁言中", 0xFFF44336 }, // 红
        };
    }
}
