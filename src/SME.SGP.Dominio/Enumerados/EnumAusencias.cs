using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EnumAusencias
    {
        [Display(Name = "No dia de hoje")]
        NoDiaDeHoje = 1,
        [Display(Name = "Há 2 dias seguidos")]
        Ha2DiasSeguidos,
        [Display(Name = "Há 3 dias seguidos")]
        Ha3DiasSeguidos,
        [Display(Name = "Há 4 dias seguidos")]
        Ha4DiasSeguidos,
        [Display(Name = "Há 5 dias seguidos")]
        Ha5DiasSeguidos,
        [Display(Name = "Entre 6 e 10 dias seguidos")]
        Entre6e10DiasSeguidos,
        [Display(Name = "Entre 11 e 15 dias seguidos")]
        Entre11e15DiasSeguidos,
        [Display(Name = "Há mais de 15 dias seguidos")]
        HaMaisDe15DiasSeguidos,
        [Display(Name = "3 ausências nos últimos 10 dias")]
        TresAusenciasNosUltimos10Dias
    }
}
