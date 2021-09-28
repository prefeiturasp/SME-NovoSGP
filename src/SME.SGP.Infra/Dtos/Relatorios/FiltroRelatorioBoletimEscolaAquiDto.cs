using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioBoletimEscolaAquiDto
    {
        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }

        public int Semestre { get; set; }

        public string TurmaCodigo { get; set; }

        public int AnoLetivo { get; set; }

        public Modalidade Modalidade { get; set; }

        public ModeloBoletim Modelo { get; set; }

        public string AlunosCodigo { get; set; }

    }
}
