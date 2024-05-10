using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery : IRequest<IEnumerable<FrequenciaGlobalMensalSemanalDto>>
    {
        public ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery(FrequenciasConsolidadacaoPorTurmaEAnoDto frequenciaDto, DateTime dataInicio, DateTime dataFim, int tipoConsolidadoFrequencia)
        {
            AnoLetivo = frequenciaDto.AnoLetivo;
            DreId = frequenciaDto.DreId;
            UeId = frequenciaDto.UeId;
            Modalidade = frequenciaDto.Modalidade;
            AnoTurma = frequenciaDto.AnoTurma;
            DataInicio = dataInicio;
            DataFim = dataFim;
            TipoConsolidadoFrequencia = tipoConsolidadoFrequencia;
            VisaoDre = frequenciaDto.VisaoDre;
            Semestre = frequenciaDto.Semestre;
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
