using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ListaNotasConceitosConsultaDto
    {
        public int AnoLetivo { get; set; }
        public int? Bimestre { get; set; }
        public string DisciplinaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public string TurmaCodigo { get; set; }
        public bool TurmaHistorico { get; set; }
    }
        

        public class ListaNotasConceitosConsultaRefatoradaDto
        {
            public int AnoLetivo { get; set; }
            public int Bimestre { get; set; }
            public long DisciplinaCodigo { get; set; }
            public Modalidade Modalidade { get; set; }
            public int Semestre { get; set; }
            public string TurmaCodigo { get; set; }
            public bool TurmaHistorico { get; set; }
            public long TurmaId { get; set; }
            public long PeriodoInicioTicks { get; set; }
            public long PeriodoFimTicks { get; set; }
            public long PeriodoEscolarId { get; set; }
        }
    }