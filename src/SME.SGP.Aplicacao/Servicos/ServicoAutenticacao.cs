using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoTokenJwt servicoTokenJwt;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAutenticacao(IServicoEOL servicoEOL, IServicoUsuario servicoUsuario, IServicoTokenJwt servicoTokenJwt)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoTokenJwt = servicoTokenJwt ?? throw new System.ArgumentNullException(nameof(servicoTokenJwt));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            var retornoDto = new UsuarioAutenticacaoRetornoDto();
            if (retornoServicoEol != null)
            {
                retornoDto.Autenticado = retornoServicoEol.Status == AutenticacaoStatusEol.Ok;
                retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;

                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfOuAdiciona(login);

                retornoDto.Token = GeraTokenSeguranca(usuario);
            }

            return retornoDto;
        }

        private string GeraTokenSeguranca(Usuario usuario)
        {
            return servicoTokenJwt.GerarToken(usuario.Id.ToString(), usuario.CodigoRf, string.Empty, null);
        }
    }
}