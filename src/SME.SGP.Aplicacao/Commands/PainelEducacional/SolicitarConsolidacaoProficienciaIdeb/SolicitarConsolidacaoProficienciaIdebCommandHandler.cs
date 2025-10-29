using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdeb
{
    public class SolicitarConsolidacaoProficienciaIdebCommandHandler : IRequestHandler<SolicitarConsolidacaoProficienciaIdebCommand, bool>
    {
        private readonly IMediator mediator;
        public SolicitarConsolidacaoProficienciaIdebCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<bool> Handle(SolicitarConsolidacaoProficienciaIdebCommand request, CancellationToken cancellationToken)
        {
            var filtroProficienciaIdeb = new FiltroConsolidacaoProficienciaIdebDto(request.AnoLetivo);
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdebPainelEducacional, filtroProficienciaIdeb), cancellationToken);
        }
    }
}
