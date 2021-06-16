using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQuery, IEnumerable<GraficoTotalDiariosEDevolutivasDTO>>
    {
        private readonly IRepositorioDiarioBordo repositorio;

        public ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQueryHandler(IRepositorioDiarioBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoTotalDiariosEDevolutivasDTO>> Handle(ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
            var retornoConsulta =  await repositorio.ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade);
            return MontarDto(retornoConsulta, request);
        }

        private IEnumerable<GraficoTotalDiariosEDevolutivasDTO> MontarDto(IEnumerable<QuantidadeTotalDiariosPendentesPorAnoETurmaDTO> retornoConsulta, ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQuery request)
        {
            var dadosGrafico = new List<GraficoTotalDiariosEDevolutivasDTO>();
            foreach (var item in retornoConsulta)
            {
                var quantidadeTotalDiariosPendentes = new GraficoTotalDiariosEDevolutivasDTO()
                {
                    Descricao = ObterDescricaoTurmaAno(request.UeId > 0, item.Ano == 0 ? item.Turma : item.Ano.ToString(), request.Modalidade),
                    Quantidade = item.QuantidadeTotalDiariosPendentes
                };

                dadosGrafico.Add(quantidadeTotalDiariosPendentes);
            }
            return dadosGrafico;
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
       => possuiFiltroUe
       ? turmaAno
       : $"{modalidade.ShortName()} - {turmaAno}";
    }
}
