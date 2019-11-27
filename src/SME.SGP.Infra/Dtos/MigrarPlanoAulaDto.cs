using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
   public class MigrarPlanoAulaDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<int> IdsTurmasDestino { get; set; }

        public PlanoAulaDto PlanoAula { get; set; }

        [Required(ErrorMessage = "O RF do professor deve ser informado")]
        public string RFProfessor { get; set; }
    }
}
