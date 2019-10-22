using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class MigrarPlanoAnualDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<int> IdsTurmasDestino { get; set; }

        public PlanoAnualDto PlanoAnual { get; set; }

        [Required(ErrorMessage = "O RF do professor deve ser informado")]
        public string RFProfessor { get; set; }
    }
}