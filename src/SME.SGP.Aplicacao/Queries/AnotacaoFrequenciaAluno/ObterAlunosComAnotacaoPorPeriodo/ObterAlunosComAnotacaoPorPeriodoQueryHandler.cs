using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComAnotacaoPorPeriodoQueryHandler : IRequestHandler<ObterAlunosComAnotacaoPorPeriodoQuery, IEnumerable<AnotacaoAlunoAulaDto>>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ObterAlunosComAnotacaoPorPeriodoQueryHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<IEnumerable<AnotacaoAlunoAulaDto>> Handle(ObterAlunosComAnotacaoPorPeriodoQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ListarAlunosComAnotacaoFrequenciaPorPeriodo(request.TurmaCodigo, request.DataInicio, request.DataFim);
    }
}
