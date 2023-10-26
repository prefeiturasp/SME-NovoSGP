using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioSemestralAlunoPAPPorAnoSemestreQueryHandler : IRequestHandler<ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery, IEnumerable<long>>
    {
        private readonly IRepositorioRelatorioSemestralPAPAluno RrpositorioRelatorioSemestralPAPAluno;

        public ObterRelatorioSemestralAlunoPAPPorAnoSemestreQueryHandler(IRepositorioRelatorioSemestralPAPAluno RrpositorioRelatorioSemestralPAPAluno)
        {
            this.RrpositorioRelatorioSemestralPAPAluno = RrpositorioRelatorioSemestralPAPAluno ?? throw new ArgumentNullException(nameof(RrpositorioRelatorioSemestralPAPAluno));
        }

        public async Task<IEnumerable<long>> Handle(ObterRelatorioSemestralAlunoPAPPorAnoSemestreQuery request, CancellationToken cancellationToken)
        {
            return await RrpositorioRelatorioSemestralPAPAluno.ObterRelatorioSemestralAlunoPAPPorAnoSemestreAsync(request.AnoLetivo, request.Semestre);
        }
    }
}
