using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum StatusConselhoClasse
    {
        [Display(Description =  "Não Iniciado")]
        NaoIniciado = 1,

        [Display(Description = "Em Andamento")]
        EmAndamento = 2,

        [Display(Description = "Concluído")]
        Concluido = 3, 
    }
}
