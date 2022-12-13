using Microsoft.AspNetCore.Authorization;
using SME.SGP.Infra;
using System.Linq;

namespace SME.SGP.Api.Filtros
{
    public class PermissaoAttribute : AuthorizeAttribute
    {
        public PermissaoAttribute(params Permissao[] permissoes)
        {
            var permissoesIds = permissoes.Select(x => (int)x);
            Roles = string.Join(",", permissoesIds);
        }
    }
}