using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Infra.Dtos.EscolaAqui;
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
        public async Task<RespostaSolicitarReiniciarSenhaDto> Executar(string cpf)
        {
            return await mediator.Send(new SolicitarReiniciarSenhaCommand(cpf));
        }
    }
}
