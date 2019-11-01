using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroDisciplinaPlanejamentoDto
    {
        public int CodigoDisciplina { get; set; }
        public bool Regencia { get; set; }
        [Range(0,long.MaxValue, ErrorMessage = "É necessario informar o codigo da turma")]
        public long CodigoTurma { get; set; }
    }
}
