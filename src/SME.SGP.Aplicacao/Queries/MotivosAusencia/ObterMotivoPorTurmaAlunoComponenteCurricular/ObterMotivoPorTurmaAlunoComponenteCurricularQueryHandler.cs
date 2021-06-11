using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosPorTurmaAlunoComponenteCurricular
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularQueryHandler : IRequestHandler<ObterMotivoPorTurmaAlunoComponenteCurricularQuery, IEnumerable<JustificativaAlunoDto>>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterMotivoPorTurmaAlunoComponenteCurricularQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }
        public async Task<IEnumerable<JustificativaAlunoDto>> Handle(ObterMotivoPorTurmaAlunoComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var motivosAusencia = await repositorioAnotacaoFrequenciaAluno.ObterPorTurmaAlunoComponenteCurricular(request.TurmaId, request.AlunoCodigo, request.ComponenteCurricularId);
           
            return motivosAusencia;
        }
    }
}
