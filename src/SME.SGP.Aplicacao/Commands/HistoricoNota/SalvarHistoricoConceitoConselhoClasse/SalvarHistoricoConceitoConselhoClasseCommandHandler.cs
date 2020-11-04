using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoConselhoClasseCommandHandler : IRequestHandler<SalvarHistoricoConceitoConselhoClasseCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse;

        public SalvarHistoricoConceitoConselhoClasseCommandHandler(IMediator mediator, IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioHistoricoNotaConselhoClasse = repositorioHistoricoNotaConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioHistoricoNotaConselhoClasse));
        }

        public async Task<long> Handle(SalvarHistoricoConceitoConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            var historicoNotaId = await mediator.Send(new SalvarHistoricoConceitoCommand(request.ConceitoAnteriorId, request.ConceitoNovoId));

            return await repositorioHistoricoNotaConselhoClasse.SalvarAsync(new Dominio.HistoricoNotaConselhoClasse()
            {
                ConselhoClasseNotaId = request.ConselhoClasseNotaId,
                HistoricoNotaId = historicoNotaId
            });
        }
    }
}
