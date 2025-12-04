using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAEmAbertoQueryHandler : IRequestHandler<ObterQuantidadeAtendimentoNAAPAEmAbertoQuery, GraficoEncaminhamentoNAAPADto>
    {
        private IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado;
        private IMediator mediator;

        public ObterQuantidadeAtendimentoNAAPAEmAbertoQueryHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado, IMediator mediator)
        {
            this.repositorioConsolidado = repositorioConsolidado ?? throw new ArgumentNullException(nameof(repositorioConsolidado));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoEncaminhamentoNAAPADto> Handle(ObterQuantidadeAtendimentoNAAPAEmAbertoQuery request, CancellationToken cancellationToken)
        {
            var grafico = await this.repositorioConsolidado.ObterQuantidadeEncaminhamentoNAAPAEmAberto(request.AnoLetivo, request.DreId, (int?)request.Modalidade);

            grafico.DataUltimaConsolidacao = await mediator.Send(new ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema.GerarConsolidadoEncaminhamentoNAAPA, request.AnoLetivo));

            return grafico;
        }
    }
}
