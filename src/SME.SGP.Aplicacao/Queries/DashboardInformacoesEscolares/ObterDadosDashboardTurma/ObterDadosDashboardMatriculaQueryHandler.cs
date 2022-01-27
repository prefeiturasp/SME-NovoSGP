using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTurmaQueryHandler : IRequestHandler<ObterDadosDashboardTurmaQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ObterDadosDashboardTurmaQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterDadosDashboardTurmaQuery request, CancellationToken cancellationToken)
        {
            var dadosGrafico = await repositorioTurmaConsulta.ObterInformacoesEscolaresTurmasAsync(request.AnoLetivo, request.DreId, request.UeId, request.Anos, request.Modalidade, request.Semestre);
            TratarDescricaoDados(dadosGrafico, request.DreId);
            return dadosGrafico;
        }

        private void TratarDescricaoDados(IEnumerable<GraficoBaseDto> dadosGrafico, long dreId)
        {
            foreach(var itemGrafico in dadosGrafico)
            {
                if (dreId > 0)
                {
                    var valorEhUmNumero = int.TryParse(itemGrafico.Descricao, out _);
                    if (valorEhUmNumero)
                    {
                        itemGrafico.Descricao += "º ano";
                    }
                }
                else
                {
                    itemGrafico.Descricao = FormatarAbreviacaoDre(itemGrafico.Descricao);
                }
            }
        }

        private static string FormatarAbreviacaoDre(string abreviacaoDre)
            => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();

        
    }
}
