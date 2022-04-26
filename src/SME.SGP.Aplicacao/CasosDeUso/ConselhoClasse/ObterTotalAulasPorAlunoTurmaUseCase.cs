using MediatR;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAulasPorAlunoTurmaUseCase : IObterTotalAulasPorAlunoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterTotalAulasPorAlunoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Executar(string codigoAluno, string codigoTurma)
        {
            return await mediator.Send(new ObterTotalAulasPorAlunoTurmaQuery(codigoAluno,codigoTurma));
        }
    }
}
