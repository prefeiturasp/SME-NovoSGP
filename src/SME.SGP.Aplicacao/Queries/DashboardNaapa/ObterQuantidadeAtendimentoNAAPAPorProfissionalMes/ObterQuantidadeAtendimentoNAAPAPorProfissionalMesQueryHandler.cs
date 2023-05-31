using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler : IRequestHandler<ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery, GraficoEncaminhamentoNAAPADto>
    {
        private IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado;

        public ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQueryHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado)
        {
            this.repositorioConsolidado = repositorioConsolidado ?? throw new ArgumentNullException(nameof(repositorioConsolidado));
        }

        public async Task<GraficoEncaminhamentoNAAPADto> Handle(ObterQuantidadeAtendimentoNAAPAPorProfissionalMesQuery request, CancellationToken cancellationToken)
        {
            var graficos = await this.repositorioConsolidado.ObterQuantidadeAtendimentoNAAPAPorProfissionalMes(request.AnoLetivo, request.DreId, request.UeId, request.Mes);
            var grafico = new GraficoEncaminhamentoNAAPADto()
            {
                DataUltimaConsolidacao = graficos.Any() ? graficos.Select(x => x.DataUltimaConsolidacao).Max() : null
            };

            foreach (var item in graficos)
            {
                grafico.Graficos.Add(new GraficoBaseDto() { Quantidade = item.Quantidade, Descricao = item.Descricao });
            }

            return grafico;
        }
    }
}
