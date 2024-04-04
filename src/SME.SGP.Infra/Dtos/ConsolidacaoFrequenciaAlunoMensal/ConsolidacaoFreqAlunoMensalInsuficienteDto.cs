using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ConsolidacaoFreqAlunoMensalInsuficienteDto
    {
        public string TurmaCodigo { get; set; }
        public string Turma { get; set; }
        public Modalidade Modalidade { get; set; }
        public string Ue { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string Dre { get; set; }
        public string AlunoCodigo { get; set; }
        public int Mes { get; set; }
        public decimal Frequencia { get; set; }
        
    }
}
