using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralTurmaPAPPorAnoSemestreQueryHandler : IRequestHandler<ObterRelatorioSemestralTurmaPAPPorAnoSemestreQuery, IEnumerable<long>>
    {
        private readonly IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestralTurmaPAP;

        public ObterRelatorioSemestralTurmaPAPPorAnoSemestreQueryHandler(IRepositorioRelatorioSemestralTurmaPAP repositorioRelatorioSemestralTurmaPAP)
        {
            this.repositorioRelatorioSemestralTurmaPAP = repositorioRelatorioSemestralTurmaPAP ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralTurmaPAP));
        }

        public async Task<IEnumerable<long>> Handle(ObterRelatorioSemestralTurmaPAPPorAnoSemestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRelatorioSemestralTurmaPAP.ObterRelatorioSemestralTurmaPAPPorAnoSemestreAsync(request.AnoLetivo, request.Semestre);
        }
    }
}
