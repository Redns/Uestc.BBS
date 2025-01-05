using FastEnumUtility;

namespace Uestc.BBS.Core.Helpers
{
    public static class EnumHelper
    {
        public static string ToInt32String<T>(this T enumValue)
            where T : struct, Enum
        {
            return enumValue.ToInt32().ToString();
        }

        public static string ToLowerString<T>(this T enumValue)
            where T : struct, Enum
        {
            return enumValue.FastToString().ToLower();
        }

        public static string ToUpperString<T>(this T enumValue)
            where T : struct, Enum
        {
            return enumValue.FastToString().ToUpper();
        }
    }
}
