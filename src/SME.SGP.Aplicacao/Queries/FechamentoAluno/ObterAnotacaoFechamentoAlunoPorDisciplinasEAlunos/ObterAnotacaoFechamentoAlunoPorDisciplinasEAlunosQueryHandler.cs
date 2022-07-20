using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQueryHandler : IRequestHandler<ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery, IEnumerable<AnotacaoFechamentoAluno>>
    {
        private readonly IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno;

        public ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQueryHandler(IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno)
        {
            this.repositorioAnotacaoFechamentoAluno = repositorioAnotacaoFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAluno));
        }

        public async Task<IEnumerable<AnotacaoFechamentoAluno>> Handle(ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFechamentoAluno.ObterPorFechamentoEAluno(request.FechamentosTurmasDisciplinasIds, request.AlunosCodigos);
    }
}
