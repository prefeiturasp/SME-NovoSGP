using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQueryHandler : IRequestHandler<ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery, IEnumerable<long>>
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConsolidacaoConselhoClasseConsulta;

        public ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQueryHandler(IRepositorioConselhoClasseConsolidadoConsulta repositorioConsolidacaoConselhoClasseConsulta)
        {
            this.repositorioConsolidacaoConselhoClasseConsulta = repositorioConsolidacaoConselhoClasseConsulta ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasseConsulta));
        }

        public async Task<IEnumerable<long>> Handle(ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery request, CancellationToken cancellationToken)
          => await repositorioConsolidacaoConselhoClasseConsulta.ObterConsolidacoesAtivasIdPorAlunoETurmaAsync(request.CodigoAluno, request.TurmaId);
    }
}
