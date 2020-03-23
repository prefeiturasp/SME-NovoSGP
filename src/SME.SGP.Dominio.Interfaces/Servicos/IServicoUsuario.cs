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

        Task<bool> PodePersistirTurma(string codigoRf, string turmaId, DateTime data);

        Task<bool> PodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null);

        Task<bool> PodePersistirTurmaNasDatas(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null);

        void RemoverPerfisUsuarioAtual();

        void RemoverPerfisUsuarioCache(string login);

        bool UsuarioLogadoPossuiPerfilSme();
    }
}