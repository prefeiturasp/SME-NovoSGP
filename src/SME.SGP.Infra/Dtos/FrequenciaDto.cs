using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FrequenciaDto
    {
        public FrequenciaDto(long aulaId)
        {
            AulaId = aulaId;
            ListaFrequencia = new List<RegistroFrequenciaAlunoDto>();
        }

        public FrequenciaDto()
        {
            ListaFrequencia = new List<RegistroFrequenciaAlunoDto>();
        }

        public string ComponenteCurricularSugerido { get; set; }

        public DateTime? AlteradoEm { get; set; }

        public string AlteradoPor { get; set; }

        public string AlteradoRF { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "A aula é obrigatória")]
        public long AulaId { get; set; }

        public DateTime CriadoEm { get; set; }

        public string CriadoPor { get; set; }

        public string CriadoRF { get; set; }

        public bool Desabilitado { get; set; }
        public long Id { get; set; }

        [ListaTemElementos(ErrorMessage = "A lista de frequência é obrigatória")]
        public IList<RegistroFrequenciaAlunoDto> ListaFrequencia { get; set; }

        public bool TemPeriodoAberto { get; set; }
    }
}