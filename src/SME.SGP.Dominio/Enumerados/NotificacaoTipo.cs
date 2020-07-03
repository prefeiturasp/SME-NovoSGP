using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum NotificacaoTipo
    {
        [Display(Name = "Calendário")]
        Calendario = 1,

        [Display(Name = "Fechamento")]
        Fechamento = 2,

        [Display(Name = "Frequência")]
        Frequencia = 3,

        [Display(Name = "Notas")]
        Notas = 4,

        [Display(Name = "Sondagem")]
        Sondagem = 5,

        [Display(Name = "Plano de Aula")]
        PlanoDeAula = 6,

        [Display(Name = "Relatório")]
        Relatorio = 7,

        [Display(Name = "Worker")]
        Worker = 8,
    }
}