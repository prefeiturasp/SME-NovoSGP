using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class MigrarPlanoAnualDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<long> IdsTurmasDestino { get; set; }

        public PlanoAnualDto PlanoAnual { get; set; }
    }
}