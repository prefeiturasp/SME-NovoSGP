using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUsuario : IConsultasUsuario
    {
        private readonly IServicoEOL servicoEOL;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasUsuario(IServicoEOL servicoEOL, IRepositorioUsuario repositorioUsuario, IServicoUsuario servicoUsuario)
        {

            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }
        public async Task<MeusDadosDto> BuscarMeusDados()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var meusDados = await servicoEOL.ObterMeusDados(login);
            var dadosUsuarioSgp = repositorioUsuario.ObterPorCodigoRfLogin(null, login);
            meusDados.Email = dadosUsuarioSgp.Email;
            return meusDados;
        }
    }
}
