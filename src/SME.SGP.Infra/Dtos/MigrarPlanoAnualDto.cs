using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MigrarPlanoAnualDto
    {
        [ListaTemElementos(ErrorMessage = "A lista de bimestres deve ser preenchida")]
        public IEnumerable<int> BimestresDestino { get; set; }

        [ListaTemElementos(ErrorMessage = "A lista de turmas deve ser preenchida")]
        public IEnumerable<int> IdsTurmasDestino { get; set; }

        public PlanoAnualDto PlanoAnual { get; set; }
    }
}