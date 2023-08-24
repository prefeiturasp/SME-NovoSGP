using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ObterTurmaRegularESrmPorAlunoQueryHandlerFake : IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterTurmaRegularESrmPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return new List<TurmasDoAlunoDto>() { new TurmasDoAlunoDto() { CodigoTurma = 1 } };
        }
    }
}
