using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SituacaoFechamento
    {
        [Display(Name = "Em Processamento")]
        EmProcessamento = 1,

        [Display(Name = "Processado Com Pendências")]
        ProcessadoComPendencias = 2,

        [Display(Name = "Processado Com Sucesso")]
        ProcessadoComSucesso = 3
    }
}