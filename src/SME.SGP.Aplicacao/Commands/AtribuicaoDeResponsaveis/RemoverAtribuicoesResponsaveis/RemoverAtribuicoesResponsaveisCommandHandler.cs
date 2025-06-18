using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicoesResponsaveisCommandHandler : IRequestHandler<RemoverAtribuicoesResponsaveisCommand>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public RemoverAtribuicoesResponsaveisCommandHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<Unit> Handle(RemoverAtribuicoesResponsaveisCommand request, CancellationToken cancellationToken)
        {
            if (request.AtribuicoesIds == null || !request.AtribuicoesIds.Any())
                return Unit.Value;

            await repositorioSupervisorEscolaDre.RemoverAtribuicoesEmLote(request.AtribuicoesIds);

            return Unit.Value;
        }
    }
}