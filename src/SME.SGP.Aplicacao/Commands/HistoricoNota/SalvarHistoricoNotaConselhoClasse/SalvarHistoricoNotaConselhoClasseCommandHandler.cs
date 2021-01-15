using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaConselhoClasseCommandHandler : IRequestHandler<SalvarHistoricoNotaConselhoClasseCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse;

        public SalvarHistoricoNotaConselhoClasseCommandHandler(IMediator mediator, IRepositorioHistoricoNotaConselhoClasse repositorioHistoricoNotaConselhoClasse)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioHistoricoNotaConselhoClasse = repositorioHistoricoNotaConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioHistoricoNotaConselhoClasse));
        }

        public async Task<long> Handle(SalvarHistoricoNotaConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            var historicoNotaId = await mediator.Send(new SalvarHistoricoNotaCommand(request.NotaAnteior, request.NotaNova));

            return await repositorioHistoricoNotaConselhoClasse.SalvarAsync(new Dominio.HistoricoNotaConselhoClasse()
            {
                ConselhoClasseNotaId = request.ConselhoClasseNotaId,
                HistoricoNotaId = historicoNotaId
            });
        }
    }
}
