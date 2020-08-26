using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUsuario : IConsultasUsuario
    {
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasUsuario(IServicoEol servicoEOL, IServicoUsuario servicoUsuario)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<MeusDadosDto> BuscarMeusDados()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var meusDados = await servicoEOL.ObterMeusDados(login);
            return meusDados;
        }

        public async Task<UsuarioEolAutenticacaoRetornoDto> ObterPerfilsUsuarioPorLogin(string login)
        {
            return await servicoEOL.ObterPerfisPorLogin(login);
        }
    }
}