using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class ObterTurmaRegularESrmPorAlunoQueryHandlerFake : IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        public Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterTurmaRegularESrmPorAlunoQuery request, CancellationToken cancellationToken)
        {
            var turmasDoAluno = new List<TurmasDoAlunoDto>() { new TurmasDoAlunoDto() { CodigoTurma = 1, CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo } };

            return Task.FromResult<IEnumerable<TurmasDoAlunoDto>>(turmasDoAluno);
        }
    }
}
