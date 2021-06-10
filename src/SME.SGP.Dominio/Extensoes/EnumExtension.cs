using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SME.SGP.Dominio
{
    public static class EnumExtension
    {
        public static bool EhUmDosValores(this Enum valorEnum, params Enum[] valores)
        {
            return valores.Contains(valorEnum);
        }
        public static string ObterNomeCurto(this Enum enumValue)
        {
            return enumValue.ObterAtributo<DisplayAttribute>().ShortName;
        }
        public static string ObterNome(this Enum enumValue)
        {
            return enumValue.ObterAtributo<DisplayAttribute>().Name;
        }
        public static TAttribute ObterAtributo<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
