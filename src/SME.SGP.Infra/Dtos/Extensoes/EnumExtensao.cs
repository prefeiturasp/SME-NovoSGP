using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SME.SGP.Infra
{
    public static class EnumExtensao
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static IEnumerable<EnumeradoRetornoDto> ListarDto<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();

            return ((TEnum[])Enum.GetValues(typeof(TEnum))).Cast<Enum>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)Convert.ChangeType(v, Enum.GetUnderlyingType(v.GetType()))
            }).ToList();
        }

        public static string Name(this Enum enumValue)
            => enumValue.GetAttribute<DisplayAttribute>().Name;

        public static string ShortName(this Enum enumValue)
            => enumValue.GetAttribute<DisplayAttribute>().ShortName;
    }
}