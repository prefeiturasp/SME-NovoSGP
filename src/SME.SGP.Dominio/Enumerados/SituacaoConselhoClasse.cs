using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SituacaoConselhoClasse
    {
        [Display(Name = "Não Iniciado")]
        NaoIniciado = 0,

        [Display(Name = "Em Andamento")]
        EmAndamento = 1,

        [Display(Name = "Concluído")]
        Concluido = 2
    }
}
