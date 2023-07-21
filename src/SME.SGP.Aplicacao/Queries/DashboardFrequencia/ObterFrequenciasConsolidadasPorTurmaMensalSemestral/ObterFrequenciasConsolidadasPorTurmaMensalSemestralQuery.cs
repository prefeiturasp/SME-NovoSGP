using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery : IRequest<IEnumerable<FrequenciaGlobalMensalSemanalDto>>
    {
        public ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string codigoTurma, DateTime dataInicioSemmana, DateTime dataFimSemana, int mes, int tipoPeriodoDashboard, bool visaoDre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semestre = semestre;
            CodigoTurma = codigoTurma;
            DataInicioSemmana = dataInicioSemmana;
            DataFimSemana = dataFimSemana;
            Mes = mes;
            TipoPeriodoDashboard = tipoPeriodoDashboard;
            VisaoDre = visaoDre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime DataInicioSemmana { get; set; }
        public DateTime DataFimSemana { get; set; }
        public int Mes { get; set; }
        public int TipoPeriodoDashboard { get; set; }
        public bool VisaoDre { get; set; }
    }
}
