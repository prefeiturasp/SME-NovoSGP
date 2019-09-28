using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario : IComandosUsuario
    {
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosUsuario(IRepositorioUsuario repositorioUsuario, IServicoAutenticacao servicoAutenticacao, IServicoUsuario servicoUsuario, IServicoPerfil servicoPerfil)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAutenticacao = servicoAutenticacao ?? throw new System.ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ?? throw new System.ArgumentNullException(nameof(servicoPerfil));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            if (retornoAutenticacaoEol.Item1.Autenticado)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login);
                usuario.AtualizaUltimoLogin();
                repositorioUsuario.Salvar(usuario);

                retornoAutenticacaoEol.Item1.PerfisUsuario = servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);
            }
            return retornoAutenticacaoEol.Item1;
        }
    }
}