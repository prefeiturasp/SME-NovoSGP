using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorio;

        public ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQueryHandler(IRepositorioRegistroIndividual repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
            var retornoConsulta = await repositorio.ObterQuantidadeRegistrosIndividuaisPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade);
            return MontarDto(retornoConsulta, request);
        }

        private IEnumerable<GraficoBaseDto> MontarDto(IEnumerable<QuantidadeRegistrosIndividuaisPorAnoTurmaDTO> retornoConsulta, ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery request)
        {
            var dadosGrafico = new List<GraficoBaseDto>();
            foreach (var item in retornoConsulta)
            {
                dadosGrafico.Add(new GraficoBaseDto()
                {
                    Descricao = ObterDescricaoTurmaAno(request.UeId > 0, item.Ano == 0 ? item.Turma : item.Ano.ToString(), request.Modalidade),
                    Quantidade = item.QuantidadeRegistrosIndividuais
                });
            }
            return dadosGrafico;
        }

        private static string ObterDescricaoTurmaAno(bool possuiFiltroUe, string turmaAno, Modalidade modalidade)
               => possuiFiltroUe
               ? turmaAno
               : $"{modalidade.ShortName()} - {turmaAno}";
    }
}
