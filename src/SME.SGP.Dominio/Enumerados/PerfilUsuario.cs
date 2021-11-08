using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum PerfilUsuario
    {
        [Display(Name = "5ae1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_ADMSME = 1,
        [Display(Name = "5be1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_ADMCOTIC = 2,
        [Display(Name = "48e1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_ADMDRE = 3,
        [Display(Name = "45E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_AD = 4,
        [Display(Name = "42e1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_ADMUE = 5,
        [Display(Name = "41e1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_CJ = 6,
        [Display(Name = "44E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_CP = 7,
        [Display(Name = "4be1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_CEFAI = 8,
        [Display(Name = "46E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_DIRETOR = 9,
        [Display(Name = "3fe1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_POA = 10,
        [Display(Name = "40E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_PROFESSOR = 11,
        [Display(Name = "43E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_SECRETARIO = 12,
        [Display(Name = "4EE1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_SUPERVISOR = 13,
        [Display(Name = "60E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_PROFESSOR_INFANTIL = 14,
        [Display(Name = "61E1E074-37D6-E911-ABD6-F81654FE895D")]
        PERFIL_CJ_INFANTIL = 15,
        [Display(Name = "3de1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_PAEE = 16,
        [Display(Name = "3ee1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_PAP = 17,
        [Display(Name = "5ce1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_POEI = 18,
        [Display(Name = "5de1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_POED = 19,
        [Display(Name = "5ee1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_POSL = 20,
        [Display(Name = "64e1e074-37d6-e911-abd6-f81654fe895d")]
        PERFIL_COMUNICADOS_UE = 21,
    }
}
