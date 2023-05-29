using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryHandler : IRequestHandler<ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery, IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>>
    {
        private IRepositorioConsolidadoAtendimentoNAAPA repositorio;

        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>> Handle(ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterQuantidadeEncaminhamentoNAAPAEmAberto(request.AnoLetivo, request.CodigoDre);
        }
    }
}
