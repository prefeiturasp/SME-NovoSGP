using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQueryHandler : IRequestHandler<ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery, IEnumerable<long>>
    {
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota;

        public ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQueryHandler(IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota)
        {
            this.repositorioConsolidacaoConselhoClasseNota = repositorioConsolidacaoConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasseNota));
        }
        public async Task<IEnumerable<long>> Handle(ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery request, CancellationToken cancellationToken)
         => await repositorioConsolidacaoConselhoClasseNota.ObterConsolidacoesConselhoClasseNotaIdsPorConsolidacoesAlunoTurmaIds(request.ConsolidacoesAlunoTurmaIds, request.Bimestre);
    }
}
