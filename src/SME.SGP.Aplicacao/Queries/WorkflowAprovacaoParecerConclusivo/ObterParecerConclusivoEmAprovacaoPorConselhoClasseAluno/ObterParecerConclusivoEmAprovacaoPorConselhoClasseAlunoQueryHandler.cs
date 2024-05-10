using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQueryHandler : IRequestHandler<ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery, IEnumerable<WFAprovacaoParecerConclusivo>>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQueryHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        public async Task<IEnumerable<WFAprovacaoParecerConclusivo>> Handle(ObterParecerConclusivoEmAprovacaoPorConselhoClasseAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioWFAprovacaoParecerConclusivo.ObterPorConselhoClasseAlunoId(request.ConselhoClasseAlunoId);
        }
    }
}
