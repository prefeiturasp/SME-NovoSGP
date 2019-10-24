using Microsoft.AspNetCore.Authorization;
using SME.SGP.Infra;
using System;
using System.Linq;

namespace SME.SGP.Api.Filtros
{
    public class PermissaoAttribute : AuthorizeAttribute
    {
        public PermissaoAttribute(params Permissao[] permissoes)
        {
            var permissoesStrings = permissoes.Select(x => Enum.GetName(typeof(Permissao), x));
            Roles = string.Join(",", permissoesStrings);
        }
    }
}