using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Permissao
    {
        [Display(Name = "Sondagem - Consulta")]
        S_C = 1,

        [Display(Name = "Sondagem - Inclusão")]
        S_I = 2,

        [Display(Name = "Sondagem - Exclusão")]
        S_E = 3,

        [Display(Name = "Sondagem - Alteração")]
        S_A = 4,

        [Display(Name = "Sondagem - Relatório")]
        S_R = 5,

        [Display(Name = "Boletim - Consulta")]
        B_C = 6,

        [Display(Name = "Calendário - Consulta")]
        C_C = 7,

        [Display(Name = "Calendário - Inclusão")]
        C_I = 8,

        [Display(Name = "Calendário - Exclusão")]
        C_E = 9,

        [Display(Name = "Calendário - Alteração")]
        C_A = 10,

        [Display(Name = "Frequência- Consulta")]
        F_C = 11,

        [Display(Name = "Frequência - Inclusão")]
        F_I = 12,

        [Display(Name = "Frequência - Exclusão")]
        F_E = 13,

        [Display(Name = "Frequência - Alteração")]
        F_A = 14,

        [Display(Name = "Atribuição de CJ - Consulta")]
        ACJ_C = 15,

        [Display(Name = "Atribuição de CJ - Inclusão")]
        ACJ_I = 16,

        [Display(Name = "Atribuição de CJ - Exclusão")]
        ACJ_E = 17,

        [Display(Name = "Atribuição de CJ - Alteração")]
        ACJ_A = 18,

        [Display(Name = "Nota/Conceito - Consulta")]
        NC_C = 19,

        [Display(Name = "Nota/Conceito - Inclusão")]
        NC_I = 20,

        [Display(Name = "Nota/Conceito - Exclusão")]
        NC_E = 21,

        [Display(Name = "Nota/Conceito - Alteração")]
        NC_A = 22,

        [Display(Name = "Plano Anual - Consulta")]
        PA_C = 23,

        [Display(Name = "Plano Anual - Inclusão")]
        PA_I = 24,

        [Display(Name = "Plano Anual - Exclusão")]
        PA_E = 25,

        [Display(Name = "Plano Anual - Alteração")]
        PA_A = 26,

        [Display(Name = "Plano de Aula- Consulta")]
        PDA_C = 27,

        [Display(Name = "Plano de Aula - Inclusão")]
        PDA_I = 28,

        [Display(Name = "Plano de Aula - Exclusão")]
        PDA_E = 29,

        [Display(Name = "Plano de Aula - Alteração")]
        PDA_A = 30,

        [Display(Name = "Plano de Ciclo - Consulta")]
        PDC_C = 31,

        [Display(Name = "Plano de Ciclo - Inclusão")]
        PDC_I = 32,

        [Display(Name = "Plano de Ciclo - Exclusão")]
        PDC_E = 33,

        [Display(Name = "Plano de Ciclo - Alteração")]
        PDC_A = 34,

        [Display(Name = "Notificação - Consulta")]
        N_C = 35,

        [Display(Name = "Notificação - Inclusão")]
        N_I = 36,

        [Display(Name = "Notificação - Exclusão")]
        N_E = 37,

        [Display(Name = "Notificação - Alteração")]
        N_A = 38,

        [Display(Name = "Atribuição Professor - Consulta")]
        AP_C = 39,

        [Display(Name = "Atribuição Professor - Inclusão")]
        AP_I = 40,

        [Display(Name = "Atribuição Professor - Exclusão")]
        AP_E = 41,

        [Display(Name = "Atribuição Professor - Alteração")]
        AP_A = 42,

        [Display(Name = "Relatório - Consulta")]
        R_C = 43,

        [Display(Name = "Reiniciar Senha - Alteração")]
        AS_C = 44
    }
}