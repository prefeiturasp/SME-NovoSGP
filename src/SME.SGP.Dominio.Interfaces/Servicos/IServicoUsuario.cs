using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        Task AlterarEmailUsuarioPorLogin(string login, string novoEmail);

        Task AlterarEmailUsuarioPorRfOuInclui(string codigoRf, string novoEmail);

        string ObterClaim(string nomeClaim);

        string ObterLoginAtual();

        string ObterNomeLoginAtual();

        Guid ObterPerfilAtual();

        Task<IEnumerable<PrioridadePerfil>> ObterPerfisUsuario(string login);

        IEnumerable<Permissao> ObterPermissoes();

        string ObterRf();

        Task<Usuario> ObterUsuarioLogado();

        Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "", string nome = "", string email = "");

        Task PodeModificarPerfil(Guid perfilParaModificar, string login);

        void RemoverPerfisUsuarioAtual();

        void RemoverPerfisUsuarioCache(string login);

        bool UsuarioLogadoPossuiPerfilSme();
    }
}