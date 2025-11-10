using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAbandonoUeDto
    {
        public List<PainelEducacionalAbandonoTurmaDto> Modalidades { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}