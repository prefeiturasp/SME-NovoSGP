using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAbandonoModalidadeDto : PainelEducacionalAbandonoBaseDto
    {
        public string Modalidade { get; set; }
        public string Ano { get; set; }
    }

    public class PainelEducacionalAbandonoSmeDreDto
    {
        public List<PainelEducacionalAbandonoModalidadeDto> Modalidades { get; set; } = new();
    }
}
