﻿namespace Its.Systems.HR.Interface.Web.Helpers.Extensions
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public static string ToCasId(this string value)
        {
            return value.Split('@')[0];
        }
    }
}