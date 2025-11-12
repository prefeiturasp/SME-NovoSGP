using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAprovacaoUeDto
    {
        public string Modalidade { get; set; }
        public List<PainelEducacionalAprovacaoUeTurmaDto> Turmas { get; set; }
    }
    public class PainelEducacionalAprovacaoUeTurmaDto
    {
        public string Turma { get; set; }
        public int TotalPromocoes { get; set; }
        public int TotalRetencoesAusencias { get; set; }
        public int TotalRetencoesNotas { get; set; }
    }

    public class PainelEducacionalAprovacaoUeItemDto : PainelEducacionalAprovacaoUeTurmaDto
    {
        public string Modalidade { get; set; }
    }

    public class PainelEducacionalAprovacaoUeRetorno
    {
        public List<PainelEducacionalAprovacaoUeDto> Modalidades { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}
