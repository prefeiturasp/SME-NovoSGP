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
        private readonly IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta;

        public ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQueryHandler(IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta)
        {
            this.repositorioAnotacaoFechamentoAlunoConsulta = repositorioAnotacaoFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAlunoConsulta));
        }

        public async Task<IEnumerable<AnotacaoFechamentoAluno>> Handle(ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFechamentoAlunoConsulta.ObterPorFechamentoEAluno(request.FechamentosTurmasDisciplinasIds, request.AlunosCodigos);
    }
}
