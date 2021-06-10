using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum ModalidadeTipoCalendario
    {
        [Display(Name = "Fundamental/Médio")]
        FundamentalMedio = 1,

        [Display(Name = "EJA")]
        EJA = 2,

        [Display(Name = "Infantil")]
        Infantil = 3
    }

    public static class ModalidadeTipoCalendarioExtension
    {
        public static Modalidade[] ObterModalidades(this ModalidadeTipoCalendario modalidade)
        {
            switch (modalidade)
            {
                case ModalidadeTipoCalendario.FundamentalMedio:
                    return new[] { Modalidade.Fundamental, Modalidade.Medio };                    
                case ModalidadeTipoCalendario.EJA:
                    return new[] { Modalidade.EJA };                    
                case ModalidadeTipoCalendario.Infantil:
                    return new[] { Modalidade.InfantilPreEscola };                    
                default:
                    throw new NegocioException("Modalidade de tipo de calendário não identificado para conversão de modalidade de turma");                    
            }
        }
    }
}
