using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum StatusFechamento
    {
        [Display(Description =  "Não Iniciado")]
        NaoIniciado = 1,

        [Display(Description = "Processado com pendências")]
        ProcessadoPendencias = 2,

        [Display(Description = "Processado")]
        Processado = 3, 
    }
}
