using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdep
{
    public class SolicitarConsolidacaoProficienciaIdepCommandHandler : IRequestHandler<SolicitarConsolidacaoProficienciaIdepCommand, bool>
    {
        private readonly IMediator mediator;
        public SolicitarConsolidacaoProficienciaIdepCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<bool> Handle(SolicitarConsolidacaoProficienciaIdepCommand request, CancellationToken cancellationToken)
        {
            var filtroProficienciaIdep = new FiltroConsolidacaoProficienciaIdepDto(request.AnoLetivo);
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdepPainelEducacional, filtroProficienciaIdep), cancellationToken);
        }
    }
}
