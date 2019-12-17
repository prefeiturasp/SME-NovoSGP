using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IServicoTokenJwt
    {
        string GerarToken(string usuarioLogin, string usuarioNome, string codigoRf, Guid guidPerfil, IEnumerable<Permissao> permissionamentos);

        void RevogarToken(string login);

        bool TokenAtivo();

        bool TokenPresente();

        bool TemPerfilNoToken(string guid);
        string ObterLogin();

        Guid ObterPerfil();
    }
}