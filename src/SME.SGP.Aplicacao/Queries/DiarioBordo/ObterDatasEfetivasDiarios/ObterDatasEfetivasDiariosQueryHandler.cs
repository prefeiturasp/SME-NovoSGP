using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasEfetivasDiariosQueryHandler : IRequestHandler<ObterDatasEfetivasDiariosQuery, IEnumerable<(long Id, DateTime DataAula)>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterDatasEfetivasDiariosQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<IEnumerable<(long Id, DateTime DataAula)>> Handle(ObterDatasEfetivasDiariosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDiarioBordo.ObterDatasPorIds(request.TurmaCodigo, request.ComponenteCurricularCodigo, request.PeriodoInicio, request.PeriodoFim);
        }
    }
}
