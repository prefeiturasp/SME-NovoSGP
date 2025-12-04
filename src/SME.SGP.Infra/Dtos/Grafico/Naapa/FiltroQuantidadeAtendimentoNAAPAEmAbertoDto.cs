using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroQuantidadeAtendimentoNAAPAEmAbertoDto
    {
        public int AnoLetivo {  get; set; }
        public long? DreId { get; set; }
        public Modalidade? Modalidade { get; set; }
    }
}
