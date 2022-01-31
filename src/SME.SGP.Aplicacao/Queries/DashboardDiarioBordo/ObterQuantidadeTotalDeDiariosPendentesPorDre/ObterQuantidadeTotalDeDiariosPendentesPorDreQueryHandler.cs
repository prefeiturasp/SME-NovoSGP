using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPendentesPorDreQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosPendentesPorDreQuery, IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>>
    {
        private readonly IRepositorioConsolidacaoDiariosBordo repositorio;

        public ObterQuantidadeTotalDeDiariosPendentesPorDreQueryHandler(IRepositorioConsolidacaoDiariosBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>> Handle(ObterQuantidadeTotalDeDiariosPendentesPorDreQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterQuantidadeTotalDeDiariosPendentesPorDre(request.AnoLetivo, request.Ano);
    }
}
