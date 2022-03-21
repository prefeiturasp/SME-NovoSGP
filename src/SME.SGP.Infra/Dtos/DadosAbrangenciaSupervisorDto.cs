using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DadosAbrangenciaSupervisorDto
    {
        public int Modalidade { get; set; }
        public string AbreviacaoDre { get; set; }
        public string CodigoDre { get; set; }
        public string DreNome { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long UeId { get; set; }
        public int Semestre { get; set; }
        public string CodigoTurma { get; set; }
        public string TurmaAno { get; set; }
        public int TurmaAnoLetivo { get; set; }
        public string TurmaNome { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public TipoTurno TipoTurno { get; set; }
        public bool EnsinoEspecial { get; set; }
        public long TurmaId { get; set; }
        public int TipoTurma { get; set; }
        public string NomeFiltro { get; set; }
    }
}
