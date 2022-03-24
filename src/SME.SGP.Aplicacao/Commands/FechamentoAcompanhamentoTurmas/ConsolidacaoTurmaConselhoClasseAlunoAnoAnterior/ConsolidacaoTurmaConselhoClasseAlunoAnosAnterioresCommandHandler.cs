using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommandHandler : IRequestHandler<ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota)
        {
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseConsolidadoNota = repositorioConselhoClasseConsolidadoNota ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoNota));
        }


        public Task<bool> Handle(ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand request, CancellationToken cancellationToken)
        {
            //var consolidadoNota = await 
            return true;
        }
    }
}
