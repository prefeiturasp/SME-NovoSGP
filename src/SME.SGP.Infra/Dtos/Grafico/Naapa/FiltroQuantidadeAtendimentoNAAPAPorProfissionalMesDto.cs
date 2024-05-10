using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto
    {
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long? UeId { get; set; }
        public int? Mes { get; set; }
        public Modalidade? Modalidade { get; set; }
    }
}
