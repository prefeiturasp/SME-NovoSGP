using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery : IRequest<IEnumerable<FrequenciaGlobalMensalSemanalDto>>
    {
        public ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime dataInicio, DateTime dataFim, int tipoConsolidadoFrequencia, int semestre, bool visaoDre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            AnoTurma = anoTurma;
            DataInicio = dataInicio;
            DataFim = dataFim;
            TipoConsolidadoFrequencia = tipoConsolidadoFrequencia;
            VisaoDre = visaoDre;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public string AnoTurma { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int TipoConsolidadoFrequencia { get; set; }
        public int Semestre { get; set; }
        public bool VisaoDre { get; set; }
    }
}
