using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario : IComandosUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoTokenJwt servicoTokenJwt;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosUsuario(IRepositorioUsuario repositorioUsuario, IServicoAutenticacao servicoAutenticacao, IServicoUsuario servicoUsuario,
            IServicoPerfil servicoPerfil, IServicoEOL servicoEOL, IServicoTokenJwt servicoTokenJwt)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAutenticacao = servicoAutenticacao ?? throw new System.ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ?? throw new System.ArgumentNullException(nameof(servicoPerfil));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoTokenJwt = servicoTokenJwt ?? throw new System.ArgumentNullException(nameof(servicoTokenJwt));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            if (retornoAutenticacaoEol.Item1.Autenticado)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login);

                retornoAutenticacaoEol.Item1.PerfisUsuario = servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);
                var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado);

                if (permissionamentos == null || permissionamentos.Count() == 0)
                    throw new NegocioException($"Não foi possível localizar os permissionamentos do usuário {login}.");

                var listaPermissoes = permissionamentos
                    .Distinct()
                    .Select(a => (Permissao)a)
                    .ToList();

                retornoAutenticacaoEol.Item1.Token = servicoTokenJwt.GerarToken(login, listaPermissoes);

                usuario.AtualizaUltimoLogin();
                repositorioUsuario.Salvar(usuario);
            }
            return retornoAutenticacaoEol.Item1;
        }
    }
}