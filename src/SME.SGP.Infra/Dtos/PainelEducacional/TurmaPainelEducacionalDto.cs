using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class TurmaPainelEducacionalDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string TurmaId { get; set; }
        public string Nome { get; set; }
        public int ModalidadeCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public string Ano { get; set; }
        public TipoTurnoEOL TipoTurno { get; set; }
    }
}
