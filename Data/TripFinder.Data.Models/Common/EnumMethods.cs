namespace TripFinder.Data.Models.Common
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumMethods
    {
        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }
    }
}
