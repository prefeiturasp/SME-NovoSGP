using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum ClassificacaoDocumento
    {
        [Display(Name = "PAEE")]
        PAEE = 1,

        [Display(Name = "PAP")]
        PAP = 2,
            
        [Display(Name = "POA")]
        POA = 3,
        
        [Display(Name = "POED")]
        POED = 4,

        [Display(Name = "POEI")]
        POEI = 5,
        
        [Display(Name = "POSL")]
        POSL = 6,
        
        [Display(Name = "PEA")]
        PEA = 7,
        
        [Display(Name = "PPP")]
        PPP = 8,
        
        [Display(Name = "Carta Pedagógica")]
        CartaPedagogica = 9,

        [Display(Name = "Documentos da turma")]
        DocumentosTurma = 10
    }
}
