using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RetornoConsultaListagemTurmaComponenteDto
    {
        public int Id { get; set; }
        public long TurmaCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string NomeTurma { get; set; }
        public string Ano { get; set; }
        public string NomeComponenteCurricular { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public int Turno { get; set; }

        public string NomeTurmaFormatado(string componenteCurricularNome)
        {
            return $"{Modalidade.ShortName()} - {NomeTurma} - {Ano}ºAno - {componenteCurricularNome}";
        }

    }
}
