using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPrioridadeEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterPrioridadeEncaminhamentoNAAPAQuery, IEnumerable<PrioridadeAtendimentoNAAPADto>>
    {
        private readonly IRepositorioQuestaoEncaminhamentoNAAPA repositorio;

        public ObterPrioridadeEncaminhamentoNAAPAQueryHandler(IRepositorioQuestaoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PrioridadeAtendimentoNAAPADto>> Handle(ObterPrioridadeEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorio.ObterPrioridadeEncaminhamento();
        }
    }
}
