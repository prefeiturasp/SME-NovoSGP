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
    public class ObterEncaminhamentoAEEComTurmaPorIdQueryHandler : IRequestHandler<ObterEncaminhamentoAEEComTurmaPorIdQuery, EncaminhamentoAEE>
    {
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public ObterEncaminhamentoAEEComTurmaPorIdQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<EncaminhamentoAEE> Handle(ObterEncaminhamentoAEEComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioEncaminhamentoAEE.ObterEncaminhamentoComTurmaPorId(request.EncaminhamentoId);
    }
}
