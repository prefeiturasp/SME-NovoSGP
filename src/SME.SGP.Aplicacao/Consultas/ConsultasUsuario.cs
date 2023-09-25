using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUsuario : IConsultasUsuario
    {
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasUsuario(IServicoUsuario servicoUsuario,IMediator mediator)
        {
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<MeusDadosDto> BuscarMeusDados()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var meusDados = await mediator.Send(new ObterUsuarioCoreSSOQuery(login));
            return meusDados;
        }

        public async Task<PerfisApiEolDto> ObterPerfilsUsuarioPorLogin(string login)
        {
            return await mediator.Send(new ObterPerfisPorLoginQuery(login));
        }
    }
}