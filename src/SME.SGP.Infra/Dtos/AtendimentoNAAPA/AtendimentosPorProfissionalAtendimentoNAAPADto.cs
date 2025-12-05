using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtendimentosPorProfissionalAtendimentoNAAPADto
    {
        public string Nome { get; set; }
        public string Rf { get; set; }
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}