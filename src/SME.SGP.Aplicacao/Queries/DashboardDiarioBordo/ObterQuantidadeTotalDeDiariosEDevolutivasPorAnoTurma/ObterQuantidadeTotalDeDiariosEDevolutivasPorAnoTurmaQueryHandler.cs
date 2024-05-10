using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQuery, IEnumerable<GraficoTotalDiariosEDevolutivasDTO>>
    {
        private readonly IRepositorioDiarioBordo repositorio;

        public ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQueryHandler(IRepositorioDiarioBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoTotalDiariosEDevolutivasDTO>> Handle(ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
            var retornoConsulta = await repositorio.ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade);
            return MontarDto(retornoConsulta, request);
        }

        private IEnumerable<GraficoTotalDiariosEDevolutivasDTO> MontarDto(IEnumerable<QuantidadeTotalDiariosEDevolutivasPorAnoETurmaDTO> retornoConsulta, ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQuery request)
        {
            var dadosGrafico = new List<GraficoTotalDiariosEDevolutivasDTO>();
            foreach (var item in retornoConsulta)
            {
                var quantidadeTotalDiariosdeBordo = new GraficoTotalDiariosEDevolutivasDTO()
                {
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId > 0, item.Ano == "0" ? item.Turma : item.Ano.ToString(), request.Modalidade), 
                    Descricao = DashboardConstants.QuantidadeTotalDeDiariosDeBordoDescricao,
                    Quantidade = item.QuantidadeTotalDiariosdeBordo 
                };

                var quantidadeTotalDiariosdeBordoComDevolutiva = new GraficoTotalDiariosEDevolutivasDTO()
                {
                    TurmaAno = ObterDescricaoTurmaAno(request.UeId > 0, item.Ano == "0" ? item.Turma : item.Ano.ToString(), request.Modalidade),
                    Descricao = DashboardConstants.QuantidadeTotalDeDiariosDeBordoComDevolutivasDescricao,
                    Quantidade = item.QuantidadeTotalDiariosdeBordoComDevolutiva
                };

                dadosGrafico.Add(quantidadeTotalDiariosdeBordoComDevolutiva);
                dadosGrafico.Add(quantidadeTotalDiariosdeBordo);
            }
            return dadosGrafico;
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
               => possuiFiltroUe
               ? turmaAno
               : $"{modalidade.ShortName()} - {turmaAno}";
    }
}
