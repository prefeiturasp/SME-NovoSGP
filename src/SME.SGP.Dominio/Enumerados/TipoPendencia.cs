using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPendencia
    {
        [Display(Name = "Avaliação sem notas / conceitos lançados")]
        AvaliacaoSemNotaParaNenhumAluno = 1,

        [Display(Name = "Aula de reposição pendente de aprovação")]
        AulasReposicaoPendenteAprovacao = 2,

        [Display(Name = "Aula sem plano de aula registrado")]
        AulasSemPlanoAulaNaDataDoFechamento = 3,

        [Display(Name = "Aulas sem frequência registrada")]
        AulasSemFrequenciaNaDataDoFechamento = 4,

        [Display(Name = "Resultados Finais Insuficientes")]
        ResultadosFinaisAbaixoDaMedia = 5,

        [Display(Name = "Alteração de nota de fechamento")]
        AlteracaoNotaFechamento = 6,

        [Display(Name = "Aula sem Frequência registrada")]
        Frequencia = 7,

        [Display(Name = "Aula sem Plano de Aula registrado")]
        PlanoAula = 8,

        [Display(Name = "Aula sem Diario de Bordo registrado")]
        DiarioBordo = 9,

        [Display(Name = "Aula sem Avaliação registrada")]
        Avaliacao = 10,

        [Display(Name = "Aulas criadas em dias não letivos")]
        AulaNaoLetivo = 11,

        [Display(Name = "Calendário com dias letivos abaixo do permitido")]
        CalendarioLetivoInsuficiente = 12,

        [Display(Name = "Cadastro de eventos pendente")]
        CadastroEventoPendente = 13
    }
}