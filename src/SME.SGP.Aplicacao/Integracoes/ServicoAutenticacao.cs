using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IServicoEOL servicoEOL;

        public ServicoAutenticacao(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<(UsuarioAutenticacaoRetornoDto, string)> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            var retornoDto = new UsuarioAutenticacaoRetornoDto();
            if (retornoServicoEol != null)
            {
                retornoDto.Autenticado = retornoServicoEol.Status == AutenticacaoStatusEol.Ok;
                retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;

                retornoDto.Token = GeraTokenSeguranca(retornoDto);
            }

            return (retornoDto, retornoServicoEol.CodigoRf);
        }

        private string GeraTokenSeguranca(UsuarioAutenticacaoRetornoDto retornoEol)
        {
            // priorizar os perfis
            //Gerar o token com os permissionamentos do perfil priorizado
            return string.Empty;
        }
    }
}