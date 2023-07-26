using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Funcionario;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorAlunoTurmaUseCase : IObterRecomendacoesPorAlunoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterRecomendacoesPorAlunoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> Executar(FiltroRecomendacaoConselhoClasseAlunoTurmaDto filtro)
        {
            return await mediator.Send(new ObterRecomendacoesPorAlunosTurmasQuery(filtro.CodigoAluno, filtro.CodigoTurma, filtro.AnoLetivo, filtro.Modalidade, filtro.Semestre));
        }
    }
}