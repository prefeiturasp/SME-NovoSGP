using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery : IRequest<IEnumerable<FrequenciaGlobalMensalSemanalDto>>
    {
        public ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime dataInicioSemmana, DateTime dataFimSemana, int tipoConsolidadoFrequencia, bool visaoDre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            AnoTurma = anoTurma;
            DataInicioSemmana = dataInicioSemmana;
            DataFimSemana = dataFimSemana;
            TipoConsolidadoFrequencia = tipoConsolidadoFrequencia;
            VisaoDre = visaoDre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public string AnoTurma { get; set; }
        public DateTime DataInicioSemmana { get; set; }
        public DateTime DataFimSemana { get; set; }
        public int TipoConsolidadoFrequencia { get; set; }
        public bool VisaoDre { get; set; }
    }
}
