using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoDevolutivasCommandHandler : IRequestHandler<LimparConsolidacaoDevolutivasCommand, bool>
    {
        private readonly IRepositorioConsolidacaoDevolutivas repositorio;
        private readonly IMediator mediator;

        public LimparConsolidacaoDevolutivasCommandHandler(IRepositorioConsolidacaoDevolutivas repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(LimparConsolidacaoDevolutivasCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.LimparConsolidacaoDevolutivasPorAno(request.AnoLetivo);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao limpar consolidação de devolutivas.", LogNivel.Negocio, LogContexto.Devolutivas, ex.Message));                
            }

            return true;
        }
    }
}
