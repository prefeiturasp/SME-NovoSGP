using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoUsuario : IServicoUsuario
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEOL servicoEOL;

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,
                              IServicoEOL servicoEOL,
                              IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil;
        }

        public async Task AlterarEmail(string login, string novoEmail)
        {
            Usuario usuario = repositorioUsuario.ObterPorCodigoRfLogin(null, login);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }

            var outrosUsuariosComMesmoEmail = repositorioUsuario.ExisteUsuarioComMesmoEmail(novoEmail, usuario.Id);
            if (outrosUsuariosComMesmoEmail)
            {
                throw new NegocioException("Já existe outro usuário com o e-mail informado.");
            }

            var retornoEol = await servicoEOL.ObterPerfisPorLogin(login);
            if (retornoEol == null || retornoEol.Status != Dto.AutenticacaoStatusEol.Ok)
            {
                throw new NegocioException("Ocorreu um erro ao obter os dados do usuário no EOL.");
            }
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(retornoEol.Perfis);
            usuario.DefinirEmail(novoEmail, perfisUsuario);
            repositorioUsuario.Salvar(usuario);
        }

        public Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "")
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(codigoRf, login);
            if (usuario != null)
                return usuario;

            usuario = new Usuario() { CodigoRf = codigoRf, Login = login };

            repositorioUsuario.Salvar(usuario);

            return usuario;
        }

        public async Task PodeModificarPerfil(string perfilParaModificar, string login)
        {
            var perfisDoUsuario = await servicoEOL.ObterPerfisPorLogin(login);
            if (perfisDoUsuario == null)
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            if (!perfisDoUsuario.Perfis.Contains(Guid.Parse(perfilParaModificar)))
                throw new NegocioException($"O usuário {login} não possui acesso ao perfil {perfilParaModificar}");
        }
    }
}