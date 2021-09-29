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
    public class ObterSePossuiParecerEmAprovacaoQueryHandler : IRequestHandler<ObterSePossuiParecerEmAprovacaoQuery,WFAprovacaoParecerConclusivo>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;

        public ObterSePossuiParecerEmAprovacaoQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorioParecer)
        {
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
        }

        public async Task<WFAprovacaoParecerConclusivo> Handle(ObterSePossuiParecerEmAprovacaoQuery request, CancellationToken cancellationToken)
            => await repositorioParecer.VerificaSePossuiAprovacaoParecerConclusivo(request.ConselhoClasseAlunoId);
    }
}
