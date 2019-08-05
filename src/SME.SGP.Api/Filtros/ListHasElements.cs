using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Api.Filtros
{
    public class ListaTemElementosAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}