using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Api.Filtros
{
    public class EnumeradoRequiridoAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value.EhNulo()) return false;
            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }
    }
}