using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoTokenJwt
    {
        string GerarToken(string usuarioLogin, string codigoRf, IEnumerable<Permissao> permissionamentos);

        bool TemPerfilNoToken(string guid);
    }
}