using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum SituacaoFechamento
    {
        [Display(Name = "Não Iniciado")]
        NaoIniciado = 0,

        [Display(Name = "Em Processamento")]
        EmProcessamento = 1,

        [Display(Name = "Processado com pendências")]
        ProcessadoComPendencias = 2,

        [Display(Name = "Processado com sucesso")]
        ProcessadoComSucesso = 3
    }
}