using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQueryHandler : IRequestHandler<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery, IEnumerable<FrequenciaAlunoSimplificadoDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> Handle(ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterFrequenciasPorAulaId(request.AulaId);
        }
    }
}
