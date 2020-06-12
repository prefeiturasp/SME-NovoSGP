using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroListagemAlunosDto
    {
        public string UeCodigo { get; set; }

        public string TurmaCodigo { get; set; }

        public int AnoLetivo { get; set; }

        public Modalidade Modalidade { get; set; }

        public int Semestre { get; set; }

    }
}
