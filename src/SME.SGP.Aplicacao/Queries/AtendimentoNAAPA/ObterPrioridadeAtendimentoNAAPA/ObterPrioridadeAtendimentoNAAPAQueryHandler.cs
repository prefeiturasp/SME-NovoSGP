using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPrioridadeAtendimentoNAAPAQueryHandler : IRequestHandler<ObterPrioridadeAtendimentoNAAPAQuery, IEnumerable<PrioridadeAtendimentoNAAPADto>>
    {
        private readonly IRepositorioQuestaoAtendimentoNAAPA repositorio;

        public ObterPrioridadeAtendimentoNAAPAQueryHandler(IRepositorioQuestaoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PrioridadeAtendimentoNAAPADto>> Handle(ObterPrioridadeAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorio.ObterPrioridadeEncaminhamento();
        }
    }
}
