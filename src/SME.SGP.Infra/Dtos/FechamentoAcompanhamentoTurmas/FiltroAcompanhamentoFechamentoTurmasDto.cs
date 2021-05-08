using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoFechamentoTurmasDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public long[] TurmaId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
        public int AnoLetivo { get; set; }
    }
}
