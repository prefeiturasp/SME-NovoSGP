using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.ConselhoClasse;
using SME.SGP.Aplicacao.Queries.ConselhoClasse.ObterTotalAlunosSemFrequenciaPorTurmaQuery;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse
{
    public class ObterTotalAulasSemFrequenciaPorTurmaUseCase : IObterTotalAulasSemFrequenciaPorTurmaUseCase
    {
        private readonly IMediator mediator;
        public ObterTotalAulasSemFrequenciaPorTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Executar(string codigoTurma)
        {
            return await mediator.Send(new ObterTotalAulasSemFrequenciaPorTurmaQuery(codigoTurma));
        }
    }
}
