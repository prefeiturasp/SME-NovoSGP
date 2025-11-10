using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorPeriodoQueryHandler : IRequestHandler<ObterAnotacaoFrequenciaAlunoPorPeriodoQuery, IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>>
    {
        private readonly IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno;

        public ObterAnotacaoFrequenciaAlunoPorPeriodoQueryHandler(IRepositorioAnotacaoFrequenciaAlunoConsulta repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>> Handle(ObterAnotacaoFrequenciaAlunoPorPeriodoQuery request, CancellationToken cancellationToken)
          => await repositorioAnotacaoFrequenciaAluno.ObterPorAlunoPorPeriodo(request.CodigoAluno, request.DataInicio, request.DataFim);
    }
}
