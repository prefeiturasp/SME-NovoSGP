using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoFechamentoTurmasDto
    {
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long[] TurmasId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }
        public int? SituacaoFechamento { get; set; }
        public int? SituacaoConselhoClasse { get; set; }
    }
}
