using MediatR;
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
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> Handle(ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterFrequenciasPorAulaId(request.AulaId);
        }
    }
}
