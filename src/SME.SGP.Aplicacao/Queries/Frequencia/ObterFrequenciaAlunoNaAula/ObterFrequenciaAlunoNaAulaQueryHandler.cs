using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoNaAulaQueryHandler : IRequestHandler<ObterFrequenciaAlunoNaAulaQuery, IEnumerable<FrequenciaAlunoAulaDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterFrequenciaAlunoNaAulaQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }
        public async Task<IEnumerable<FrequenciaAlunoAulaDto>> Handle(ObterFrequenciaAlunoNaAulaQuery request, CancellationToken cancellationToken)
           => await repositorioRegistroFrequenciaAluno.ObterFrequenciasDoAlunoNaAula(request.AlunoCodigo, request.AulaId);
    }
}
