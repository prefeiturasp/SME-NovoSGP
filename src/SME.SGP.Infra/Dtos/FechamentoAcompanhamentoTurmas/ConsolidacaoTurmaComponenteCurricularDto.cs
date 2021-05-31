using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ConsolidacaoTurmaComponenteCurricularDto
    {
        public string Descricao { get; set; }
        public string ProfessorNome { get; set; }
        public string ProfessorRf { get; set; }
        public SituacaoFechamento SituacaoFechamentoCodigo { get; set; }
        public string SituacaoFechamento { get => SituacaoFechamentoCodigo.Name(); }
        public string Professor 
        {
            get => string.IsNullOrEmpty(ProfessorNome) ?
                "Sem Titular" :
                $"{ProfessorNome} ({ProfessorRf})";
        }
    }
}
