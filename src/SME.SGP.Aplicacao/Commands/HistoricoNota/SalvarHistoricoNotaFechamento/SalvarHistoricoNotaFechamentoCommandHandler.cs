using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaFechamentoCommandHandler : IRequestHandler<SalvarHistoricoNotaFechamentoCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioHistoricoNotaFechamento repositorioHistoricoNotaFechamento;

        public SalvarHistoricoNotaFechamentoCommandHandler(IRepositorioHistoricoNotaFechamento repositorioHistoricoNotaFechamento, IMediator mediator)
        {
            this.repositorioHistoricoNotaFechamento = repositorioHistoricoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioHistoricoNotaFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(SalvarHistoricoNotaFechamentoCommand request, CancellationToken cancellationToken)
        {
            var historicoNotaId = await mediator.Send(new SalvarHistoricoNotaCommand(request.NotaAnterior, request.NotaNova, request.CriadoRF, request.CriadoPor));

            var historicoNotaFechamento = MapearParaEntidade(request, historicoNotaId);

            return await repositorioHistoricoNotaFechamento.SalvarAsync(historicoNotaFechamento);
        }

        private HistoricoNotaFechamento MapearParaEntidade(SalvarHistoricoNotaFechamentoCommand request, long historicoNotaId)
           => new HistoricoNotaFechamento()
           {
               HistoricoNotaId = historicoNotaId,
               FechamentoNotaId = request.FechamentoNotaId,
               WorkFlowId = request.WorkFlowId
           };
    }
}
