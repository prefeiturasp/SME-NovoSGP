using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoComunicado
    {
        [Display(Name = "SME")]
        SME = 1,     // Grupo-6
        [Display(Name = "DRE")]
        DRE = 2,     // DRE-1080
        [Display(Name = "UE")]
        UE = 3,      // UE-10180
        [Display(Name = "UE Mod")]
        UEMOD = 4,   // UE-10180-MOD-2
        [Display(Name = "Turma")]
        TURMA = 5,   // TUR-5689
        [Display(Name = "Aluno")]
        ALUNO = 6,    // ALU-55689
        [Display(Name = "SME Ano")]
        SME_ANO = 7,
        [Display(Name = "DRE Ano")]
        DRE_ANO = 8

    }
}
