using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoTokenJwt
    {
        string GerarToken(string usuarioLogin, IEnumerable<Permissao> permissionamentos);
    }
}