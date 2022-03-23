using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalDevolutivasPorDreQueryHandler : IRequestHandler<ObterTotalDevolutivasPorDreQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioConsolidacaoDevolutivas repositorio;

        public ObterTotalDevolutivasPorDreQueryHandler(IRepositorioConsolidacaoDevolutivas repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterTotalDevolutivasPorDreQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterTotalDevolutivasPorDre(request.AnoLetivo, request.Ano);
    }
}
