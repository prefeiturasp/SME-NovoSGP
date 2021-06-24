using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Modalidade
    {
        [Display(Name = "Infantil", ShortName = "EI")]
        EducacaoInfantil = 1,

        [Display(Name = "EJA", ShortName = "EJA")]
        EJA = 3,

        [Display(Name = "CIEJA", ShortName = "CIEJA")]
        CIEJA = 4,

        [Display(Name = "Fundamental", ShortName = "EF")]
        Fundamental = 5,

        [Display(Name = "Médio", ShortName = "EM")]
        Medio = 6,

        [Display(Name = "CMCT", ShortName = "CMCT")]
        CMCT = 7,

        [Display(Name = "MOVA", ShortName = "MOVA")]
        MOVA = 8,

        [Display(Name = "ETEC", ShortName = "ETEC")]
        ETEC = 9
    }

    public static class ModalidadeExtension
    {
        public static ModalidadeTipoCalendario ObterModalidadeTipoCalendario(this Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EducacaoInfantil:
                    return ModalidadeTipoCalendario.Infantil;
                case Modalidade.Fundamental:
                case Modalidade.Medio:
                    return ModalidadeTipoCalendario.FundamentalMedio;
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;
                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }
    }
}