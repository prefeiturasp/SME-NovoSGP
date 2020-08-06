using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class DevolutivaDiarioBordoDto
    {
        public DevolutivaDiarioBordoDto()
        {
            DiariosBordo = new List<DiarioBordoDto>();
        }

        public string Descricao { get; set; }
        public List<DiarioBordoDto> DiariosBordo { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
    }
}
