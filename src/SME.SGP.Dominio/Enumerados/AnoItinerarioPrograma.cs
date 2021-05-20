using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum AnoItinerarioPrograma
    {
        [Display(ShortName = "1")]
        Um = 1,
        [Display(ShortName = "2")]
        Dois = 2,
        [Display(ShortName = "3")]
        Tres = 3,
        [Display(ShortName = "4")]
        Quatro = 4,
        [Display(ShortName = "5")]
        Cinco = 5,
        [Display(ShortName = "6")]
        Seis = 6,
        [Display(ShortName = "7")]
        Sete = 7,
        [Display(ShortName = "8")]
        Oito = 8,
        [Display(ShortName = "9")]
        Nove = 9,
        [Display(ShortName = "Educacao física")]
        EducacaoFisica = 200,
        [Display(ShortName = "Turmas de programa")]
        Programa = 300,
        [Display(ShortName = "Itinerário")]
        Itinerario = 700
    }
}
