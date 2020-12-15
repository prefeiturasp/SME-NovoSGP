using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoFechamentoCommandHandler : IRequestHandler<SalvarHistoricoConceitoFechamentoCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioHistoricoNotaFechamento repositorioHistoricoNotaFechamento;

        public SalvarHistoricoConceitoFechamentoCommandHandler(IMediator mediator, IRepositorioHistoricoNotaFechamento repositorioHistoricoNotaFechamento)
        {
            this.repositorioHistoricoNotaFechamento = repositorioHistoricoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioHistoricoNotaFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(SalvarHistoricoConceitoFechamentoCommand request, CancellationToken cancellationToken)
        {
            var historicoNotaId = await mediator.Send(new SalvarHistoricoConceitoCommand(request.ConceitoAnteriorId, request.ConceitoNovoId, request.CriadoRF, request.CriadoPor));

            var historicoNotaFechamento = MapearParaEntidade(request, historicoNotaId);

            return await repositorioHistoricoNotaFechamento.SalvarAsync(historicoNotaFechamento);
        }

        private HistoricoNotaFechamento MapearParaEntidade(SalvarHistoricoConceitoFechamentoCommand request, long historicoNotaId)
           => new HistoricoNotaFechamento()
           {
               HistoricoNotaId = historicoNotaId,
               FechamentoNotaId = request.FechamentoNotaId,
               WorkFlowId = request.WorkFlowId
           };
    }
}
