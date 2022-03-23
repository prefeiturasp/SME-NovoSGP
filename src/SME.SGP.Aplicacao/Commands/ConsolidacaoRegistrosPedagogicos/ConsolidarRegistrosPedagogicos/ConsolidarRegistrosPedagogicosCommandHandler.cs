using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosCommandHandler : AsyncRequestHandler<ConsolidarRegistrosPedagogicosCommand>
    {
        private readonly IRepositorioConsolidacaoRegistrosPedagogicos repositorio;
        private readonly IUnitOfWork unitOfWork;

        public ConsolidarRegistrosPedagogicosCommandHandler(IRepositorioConsolidacaoRegistrosPedagogicos repositorio, IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(ConsolidarRegistrosPedagogicosCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();

            await repositorio.Excluir(request.ConsolidacaoRegistrosPedagogicos);

            await repositorio.Inserir(request.ConsolidacaoRegistrosPedagogicos);

            unitOfWork.PersistirTransacao();
        }
    }
}
