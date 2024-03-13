using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryHandler : IRequestHandler<ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery, GraficoEncaminhamentoNAAPADto>
    {
        private IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado;
        private IMediator mediator;

        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado, IMediator mediator)
        {
            this.repositorioConsolidado = repositorioConsolidado ?? throw new ArgumentNullException(nameof(repositorioConsolidado));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoEncaminhamentoNAAPADto> Handle(ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery request, CancellationToken cancellationToken)
        {
            var grafico = await this.repositorioConsolidado.ObterQuantidadeEncaminhamentoNAAPAEmAberto(request.AnoLetivo, request.DreId, (int?)request.Modalidade);

            grafico.DataUltimaConsolidacao = await mediator.Send(new ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA));
            
            return grafico;
        }
    }
}
