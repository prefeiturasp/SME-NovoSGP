using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EnumImprimirAnexosNAAPA
    {
        [Display(Name = "Não")]
        Nao = 1,
        [Display(Name = "Apenas encaminhamento")]
        ApenasEncaminhamento = 2,
        [Display(Name = "Apenas atendimentos")]
        ApenasAtendimentos = 3,
        [Display(Name = "Encaminhamento e Atendimentos")]
        EncaminhamentoAtendimentos = 4
    }
}
