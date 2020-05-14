using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IServicoTokenJwt
    {
        string GerarToken(string usuarioLogin, string usuarioNome, string codigoRf, Guid guidPerfil, IEnumerable<Permissao> permissionamentos);

        DateTime ObterDataHoraCriacao();

        DateTime ObterDataHoraExpiracao();

        string ObterLogin();

        Guid ObterPerfil();

        Task RevogarToken(string login);

        bool TemPerfilNoToken(string guid);

        bool TokenAtivo();

        bool TokenPresente();
    }
}