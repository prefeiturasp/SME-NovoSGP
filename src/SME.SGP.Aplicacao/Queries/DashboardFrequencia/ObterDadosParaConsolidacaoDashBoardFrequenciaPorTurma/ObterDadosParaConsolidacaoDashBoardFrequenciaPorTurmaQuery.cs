using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery : IRequest<DadosParaConsolidacaoDashBoardFrequenciaDto>
    {
        public ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(int anoLetivo, long turmaId, Modalidade modalidade, TipoPeriodoDashboardFrequencia tipoPeriodo, int mes, DateTime dataInicio, DateTime dataFim)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Modalidade = modalidade;
            TipoPeriodo = tipoPeriodo;
            Mes = mes;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public TipoPeriodoDashboardFrequencia TipoPeriodo { get; set; }
        public int Mes { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
