using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroRecuperacaoParalelaDto
    {
        public RecuperacaoParalelaOrdenacao? Ordenacao { get; set; }
        public long PeriodoId { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }

    }
}