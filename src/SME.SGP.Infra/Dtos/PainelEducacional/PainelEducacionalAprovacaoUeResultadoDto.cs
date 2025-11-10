using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAprovacaoUeResultadoDto
    {
        public List<PainelEducacionalAprovacaoUeDto> Turmas { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}
