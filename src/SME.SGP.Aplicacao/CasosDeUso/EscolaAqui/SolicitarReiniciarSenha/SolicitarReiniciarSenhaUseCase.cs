using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaUseCase : ISolicitarReiniciarSenhaUseCase
    {
        private readonly IMediator mediator;

        public SolicitarReiniciarSenhaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task Executar(string cpf)
        {
            await mediator.Send(new SolicitarReiniciarSenhaCommand(cpf));
        }
    }
}
