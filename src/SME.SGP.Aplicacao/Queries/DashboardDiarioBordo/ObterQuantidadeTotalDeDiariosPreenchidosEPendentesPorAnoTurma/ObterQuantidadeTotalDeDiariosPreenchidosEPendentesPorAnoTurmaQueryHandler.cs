using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery, IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQueryHandler(IRepositorioDiarioBordo repositorio)
        {
            this.repositorioDiarioBordo = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>> Handle(ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
           var retornoConsulta = await repositorioDiarioBordo.ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.EhPerfilSMEDRE);
           return MontarDto(retornoConsulta);
        }

        private IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO> MontarDto(IEnumerable<QuantidadeTotalDiariosPendentesEPreenchidosPorAnoOuTurmaDTO> retornoConsulta)
        {
            var dadosGrafico = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>();
            foreach (var item in retornoConsulta)
            {
                var quantidadeTotalDiariosPreenchidos = new GraficoTotalDiariosPreenchidosEPendentesDTO()
                {
                    TurmaAno = item.AnoTurma.EhNulo() ? "" : item.AnoTurma,
                    Quantidade = item.QuantidadeTotalDiariosPreenchidos,
                    Descricao = "Preenchidos"
                };
                dadosGrafico.Add(quantidadeTotalDiariosPreenchidos);

                var quantidadeTotalDiariosPendentes = new GraficoTotalDiariosPreenchidosEPendentesDTO()
                {
                    TurmaAno = item.AnoTurma.EhNulo() ? "" : item.AnoTurma,
                    Quantidade = item.QuantidadeTotalDiariosPendentes,
                    Descricao = "Pendentes"
                };
                dadosGrafico.Add(quantidadeTotalDiariosPendentes);
            }
            return dadosGrafico;
        }
    }
}
