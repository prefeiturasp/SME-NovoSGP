using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public static class HelperEnum
    {
        public static string GetEnumDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            return field?.GetCustomAttribute<DisplayAttribute>()?.Name
                ?? field?.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? value.ToString();
        }
    }
}
