using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoDocumento
    {
        [Display(Name = "Plano de Trabalho")]
        PlanoTrabalho = 1,

        [Display(Name = "Documento")]
        Documento = 2
    }
}
