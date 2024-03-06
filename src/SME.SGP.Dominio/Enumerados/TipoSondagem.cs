using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoSondagem
    {
        [Display(Name = "LP - Escrita")]
        LP_Escrita = 1,
        [Display(Name = "LP - Leitura")]
        LP_Leitura = 2,
        [Display(Name = "IAD - Leitura em voz alta")]
        LP_LeituraVozAlta = 3,
        [Display(Name = "IAD - Capacidade de Leitura")]
        LP_CapacidadeLeitura = 4,
        [Display(Name = "IAD - Produção de texto")]
        LP_ProducaoTexto = 5,
        [Display(Name = "MAT - Números")]
        MAT_Numeros = 6,
        [Display(Name = "MAT - Campo aditivo")]
        MAT_CampoAditivo = 7,
        [Display(Name = "MAT - Campo multiplicativo")]
        MAT_CampoMultiplicativo = 8,
        [Display(Name = "MAT - IAD")]
        MAT_IAD = 9,
    }
}
