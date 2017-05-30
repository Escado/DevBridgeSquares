using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevBridgeSquares.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum element)
        {
            FieldInfo fi = element.GetType().GetField(element.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return element.ToString();
        }
    }
}
