using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler : IRequestHandler<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery, GraficoAtendimentoNAAPADto>
    {
        private IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado;
        private IMediator mediator;

        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado, IMediator mediator)
        {
            this.repositorioConsolidado = repositorioConsolidado ?? throw new ArgumentNullException(nameof(repositorioConsolidado));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoAtendimentoNAAPADto> Handle(ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery request, CancellationToken cancellationToken)
        {
            var graficos = await this.repositorioConsolidado.ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(request.AnoLetivo, request.DreId, request.UeId, request.Mes, (int?)request.Modalidade);
            var grafico = new GraficoAtendimentoNAAPADto()
            {
                DataUltimaConsolidacao = await mediator.Send(new ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema.GerarConsolidadoAtendimentoNAAPA, request.AnoLetivo))
            };

            foreach (var item in graficos)
            {
                grafico.Graficos.Add(new GraficoBaseDto() { Quantidade = item.Quantidade, Descricao = item.Descricao });
            }

            return grafico;
        }
    }
}
