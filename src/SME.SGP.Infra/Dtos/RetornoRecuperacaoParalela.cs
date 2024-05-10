using System;

namespace SME.SGP.Infra
{
    public class RetornoRecuperacaoParalela
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public long AlunoId { get; set; }
        public bool Ativo { get; set; }
        public DateTime? CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public long Id { get; set; }
        public long ObjetivoId { get; set; }
        public int OrdenacaoResposta { get; set; }
        public long PeriodoRecuperacaoParalelaId { get; set; }
        public long RespostaId { get; set; }
        public long TurmaId { get; set; }
        public int BimestreEdicao { get; set; }
    }
}