using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorTurmaQueryHandler : IRequestHandler<ObterDevolutivaPorTurmaQuery, ConsolidacaoDevolutivaTurmaDTO>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterDevolutivaPorTurmaQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<ConsolidacaoDevolutivaTurmaDTO> Handle(ObterDevolutivaPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterDevolutivasPorTurma(request.TurmaCodigo);
    }
}
