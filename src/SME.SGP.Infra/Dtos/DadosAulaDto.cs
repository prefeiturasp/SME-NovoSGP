using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DadosAulaDto
    {
        public bool podeCadastrarAvaliacao;

        public AtividadeAvaliativa Atividade { get; set; }
        public string Disciplina { get; set; }
        public bool EhRegencia { get; set; }
        public string Horario { get; set; }
        public string Modalidade { get; set; }
        public string Tipo { get; set; }
        public string Turma { get; set; }
        public string UnidadeEscolar { get; set; }
    }
}