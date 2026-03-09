using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais
{
    public class InformacoesEducacionaisRetornoDto
    {
        public IEnumerable<RegistroInformacoesEducacionaisUeDto> Ues { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}
