using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
    public enum FluenciaLeitoraTipoAvaliacaoEnum
    {
        [Display(Name = "Avaliação de entrada")]
        AvaliacaoEntrada = 1,

        [Display(Name = "Avaliação de saída")]
        AvaliacaoSaida = 2
    }
}
