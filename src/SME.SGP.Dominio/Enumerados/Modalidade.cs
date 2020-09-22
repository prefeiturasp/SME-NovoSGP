using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Modalidade
    {
        [Display(Name = "Infantil", ShortName = "EI")]
        Infantil = 1,

        [Display(Name = "Fundamental", ShortName = "EF")]
        Fundamental = 5,

        [Display(Name = "Médio", ShortName = "EM")]
        Medio = 6,

        [Display(Name = "EJA", ShortName = "EJA")]
        EJA = 3
    }

    public static class ModalidadeExtension
    {
        public static ModalidadeTipoCalendario ObterModalidadeTipoCalendario(this Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.Infantil:
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