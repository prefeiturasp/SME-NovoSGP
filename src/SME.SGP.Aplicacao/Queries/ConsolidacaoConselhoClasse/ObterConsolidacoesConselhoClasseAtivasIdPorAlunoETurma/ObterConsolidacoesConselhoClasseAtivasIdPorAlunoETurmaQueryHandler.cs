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
    public class ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQueryHandler : IRequestHandler<ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse;

        public ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQueryHandler(IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse)
        {
            this.repositorioConsolidacaoConselhoClasse = repositorioConsolidacaoConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasse));
        }
        public async Task<IEnumerable<long>> Handle(ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery request, CancellationToken cancellationToken)
          => await repositorioConsolidacaoConselhoClasse.ObterConsolidacoesAtivasIdPorAlunoETurmaAsync(request.CodigoAluno, request.TurmaId);
    }
}