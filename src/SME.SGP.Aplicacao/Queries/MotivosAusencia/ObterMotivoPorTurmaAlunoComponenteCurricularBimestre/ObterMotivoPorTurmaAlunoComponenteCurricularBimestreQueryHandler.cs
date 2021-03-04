using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.MotivosAusencia.ObterMotivosPorTurmaAlunoComponenteCurricular
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryHandler : IRequestHandler<ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery, IEnumerable<JustificativaAlunoDto>>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }
        public async Task<IEnumerable<JustificativaAlunoDto>> Handle(ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery request, CancellationToken cancellationToken)
        {
            var motivosAusencia = await repositorioAnotacaoFrequenciaAluno.ObterPorTurmaAlunoComponenteCurricularBimestre(request.TurmaId, request.AlunoCodigo, request.ComponenteCurricularId, request.Bimestre);
           
            return motivosAusencia;
        }
    }
}
