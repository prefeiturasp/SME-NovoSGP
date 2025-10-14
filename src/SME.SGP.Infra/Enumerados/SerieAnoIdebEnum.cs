using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
    public enum SerieAnoIdebEnum
    {
        [Display(Name = "Anos iniciais")]
        AnosIniciais = 1,
        [Display(Name = "Anos finais")]
        AnosFinais = 2,
        [Display(Name = "Ensino Médio")]
        EnsinoMedio = 3
    }
}
