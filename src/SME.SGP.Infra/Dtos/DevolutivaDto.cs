using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class DevolutivaDto
    {
        public DevolutivaDto()
        {
            DiariosBordo = new List<DiarioBordoDto>();
        }

        public string Descricao { get; set; }

        public List<DiarioBordoDto> DiariosBordo { get; set; }

        public long CodigoComponenteCurricular { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFim { get; set; }

        public bool Excluido { get; set; }
    }
}
