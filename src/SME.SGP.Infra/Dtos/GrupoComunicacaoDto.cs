using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class GrupoComunicacaoDto
    {
        public IEnumerable<CicloEnsinoDto> CiclosEnsino { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public IEnumerable<TipoEscolaDto> TiposEscola { get; set; }
    }
}