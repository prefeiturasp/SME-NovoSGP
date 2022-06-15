using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioEscolaAquiDto
    {
        public string DreCodigo { get; set; }

        public string UeCodigo { get; set; }
        public long? TurmaId { get; set; }

        public int Semestre { get; set; }

        public string TurmaCodigo { get; set; }

        public int AnoLetivo { get; set; }

        public int ModalidadeCodigo { get; set; }

        public string AlunoCodigo { get; set; }

    }
}
