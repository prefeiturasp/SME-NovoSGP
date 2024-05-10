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
        Infantil = 3,

        [Display(Name = "CELP")]
        CELP = 4
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
                    return new[] { Modalidade.EducacaoInfantil };     
                case ModalidadeTipoCalendario.CELP:
                    return new[] { Modalidade.CELP };
                default:
                    throw new NegocioException("Modalidade de tipo de calendário não identificado para conversão de modalidade de turma");                    
            }
        }

        public static bool EhEjaOuCelp(this ModalidadeTipoCalendario modalidade)
        {
            return modalidade.EhUmDosValores(ModalidadeTipoCalendario.EJA, ModalidadeTipoCalendario.CELP);
        }
        
        public static bool NaoEhEjaOuCelp(this ModalidadeTipoCalendario modalidade)
        {
            return !EhEjaOuCelp(modalidade);
        }
        
        public static bool EhEJA(this ModalidadeTipoCalendario modalidade)
        {
            return modalidade.EhUmDosValores(ModalidadeTipoCalendario.EJA);
        }
        
        public static bool EhCELP(this ModalidadeTipoCalendario modalidade)
        {
            return modalidade.EhUmDosValores(ModalidadeTipoCalendario.CELP);
        }
        
        public static bool EhEducacaoInfantil(this ModalidadeTipoCalendario modalidade)
        {
            return modalidade.EhUmDosValores(ModalidadeTipoCalendario.Infantil);
        }
        
        public static bool NaoEhEducacaoInfantil(this ModalidadeTipoCalendario modalidade)
        {
            return !EhEducacaoInfantil(modalidade);
        }
        
        public static bool EhFundamentalMedio(this ModalidadeTipoCalendario modalidade)
        {
            return modalidade.EhUmDosValores(ModalidadeTipoCalendario.FundamentalMedio);
        }
    }
}
