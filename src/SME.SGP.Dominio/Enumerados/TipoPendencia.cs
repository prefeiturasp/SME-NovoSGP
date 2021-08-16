using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPendencia
    {
        [Display(Name = "Avaliação sem notas / conceitos lançados", GroupName = "Fechamento")]
        AvaliacaoSemNotaParaNenhumAluno = 1,

        [Display(Name = "Aula de reposição pendente de aprovação", GroupName = "Fechamento")]
        AulasReposicaoPendenteAprovacao = 2,

        [Display(Name = "Aula sem plano de aula registrado", GroupName = "Fechamento")]
        AulasSemPlanoAulaNaDataDoFechamento = 3,

        [Display(Name = "Aulas sem frequência registrada", GroupName = "Fechamento")]
        AulasSemFrequenciaNaDataDoFechamento = 4,

        [Display(Name = "Resultados Finais Insuficientes", GroupName = "Fechamento")]
        ResultadosFinaisAbaixoDaMedia = 5,

        [Display(Name = "Alteração de nota de fechamento", GroupName = "Fechamento")]
        AlteracaoNotaFechamento = 6,

        [Display(Name = "Aula sem Frequência registrada", GroupName = "Calendário")]
        Frequencia = 7,

        [Display(Name = "Aula sem Plano de Aula registrado", GroupName = "Calendário")]
        PlanoAula = 8,

        [Display(Name = "Aula sem Diario de Bordo registrado", GroupName = "Calendário")]
        DiarioBordo = 9,

        [Display(Name = "Aula sem Avaliação registrada", GroupName = "Calendário")]
        Avaliacao = 10,

        [Display(Name = "Aulas criadas em dias não letivos", GroupName = "Calendário")]
        AulaNaoLetivo = 11,

        [Display(Name = "Calendário com dias letivos abaixo do permitido", GroupName = "Calendário")]
        CalendarioLetivoInsuficiente = 12,

        [Display(Name = "Cadastro de eventos pendente", GroupName = "Calendário")]
        CadastroEventoPendente = 13,

        [Display(Name = "Ausência de Avaliacao", GroupName = "Fechamento")]
        AusenciaDeAvaliacaoProfessor = 14,

        [Display(Name = "Ausência de Avaliacao", GroupName = "Fechamento")]
        AusenciaDeAvaliacaoCP = 15,

        [Display(Name = "Ausência de Fechamento", GroupName = "Fechamento")]
        AusenciaFechamento = 16,

        [Display(Name = "Ausência de registro individual", GroupName = "Diario de Classe")]
        AusenciaDeRegistroIndividual = 17,
        
        [Display(Name = "AEE", GroupName = "AEE")]
        AEE = 18,
    }
}