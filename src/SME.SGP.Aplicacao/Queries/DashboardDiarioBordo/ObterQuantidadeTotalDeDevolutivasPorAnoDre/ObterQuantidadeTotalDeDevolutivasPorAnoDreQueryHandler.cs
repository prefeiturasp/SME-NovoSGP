using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDevolutivasPorAnoDreQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery, IEnumerable<GraficoTotalDevolutivasPorAnoDTO>>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterQuantidadeTotalDeDevolutivasPorAnoDreQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }
        public async Task<IEnumerable<GraficoTotalDevolutivasPorAnoDTO>> Handle(ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery request, CancellationToken cancellationToken)
        {
            var dadosConsulta = await repositorioDevolutiva.ObterDevolutivasPorAnoDre(request.AnoLetivo, request.Mes, request.DreId);
            return MontarDto(dadosConsulta);
        }

        private IEnumerable<GraficoTotalDevolutivasPorAnoDTO> MontarDto(IEnumerable<QuantidadeTotalDevolutivasPorAnoDTO> retornoConsulta)
        {
            var dadosGrafico = new List<GraficoTotalDevolutivasPorAnoDTO>();
            foreach (var item in retornoConsulta)
            {
                var quantidadeTotalDiariosPendentes = new GraficoTotalDevolutivasPorAnoDTO()
                {
                    Descricao = "Devolutivas",
                    Quantidade = item.Quantidade,
                    Ano = item.Ano
                };

                dadosGrafico.Add(quantidadeTotalDiariosPendentes);
            }
            return dadosGrafico;
        }
    }
}
