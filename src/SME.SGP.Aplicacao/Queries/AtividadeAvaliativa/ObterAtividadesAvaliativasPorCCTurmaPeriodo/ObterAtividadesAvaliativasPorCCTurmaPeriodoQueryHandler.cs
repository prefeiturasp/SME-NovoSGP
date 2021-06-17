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
    public class ObterAtividadesAvaliativasPorCCTurmaPeriodoQueryHandler : IRequestHandler<ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery, IEnumerable<AtividadeAvaliativa>>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ObterAtividadesAvaliativasPorCCTurmaPeriodoQueryHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }
        public async Task<IEnumerable<AtividadeAvaliativa>> Handle(ObterAtividadesAvaliativasPorCCTurmaPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativa.ObterPorTurmaDisciplinasPeriodoAsync(request.TurmaCodigo, request.ComponentesCurriculares, request.PeriodoInicio, request.PeriodoFim);
        }
    }
}
