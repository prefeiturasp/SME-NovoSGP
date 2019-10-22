using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public enum Permissao
    {
        #region Diário de Classe

        [PermissaoMenu(Descricao = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "sondagem/")]
        S_C = 1,

        [PermissaoMenu(Descricao = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "sondagem/novo/")]
        S_I = 2,

        [PermissaoMenu(Descricao = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9)]
        S_E = 3,

        [PermissaoMenu(Descricao = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "sondagem/editar/:id")]
        S_A = 4,

        [PermissaoMenu(Descricao = "Boletim", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "boletim/")]
        B_C = 9,

        [PermissaoMenu(Descricao = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "frequencia/")]
        F_C = 14,

        [PermissaoMenu(Descricao = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "frequencia/novo")]
        F_I = 15,

        [PermissaoMenu(Descricao = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1)]
        F_E = 16,

        [PermissaoMenu(Descricao = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "frequencia/editar/:id")]
        F_A = 17,

        #endregion Diário de Classe

        #region Relatórios

        [PermissaoMenu(Descricao = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "relatorios/sondagem/")]
        SR_C = 5,

        [PermissaoMenu(Descricao = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "relatorios/sondagem/novo")]
        SR_I = 6,

        [PermissaoMenu(Descricao = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        SR_E = 7,

        [PermissaoMenu(Descricao = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "relatorios/sondagem/editar/:id")]
        SR_A = 8,

        [PermissaoMenu(Descricao = "Relatório Consulta", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "relatorios/")]
        R_C = 46,

        #endregion Relatórios

        #region Calendário Escolar

        [Display(Name = "Calendário - Consulta", ShortName = "Calendário", GroupName = "Calendário Escolar")]
        C_C = 10,

        [Display(Name = "Calendário - Inclusão", ShortName = "Calendário", GroupName = "Calendário Escolar")]
        C_I = 11,

        [Display(Name = "Calendário - Exclusão", ShortName = "Calendário", GroupName = "Calendário Escolar")]
        C_E = 12,

        [Display(Name = "Calendário - Alteração", ShortName = "Calendário", GroupName = "Calendário Escolar")]
        C_A = 13,

        #endregion Calendário Escolar

        [Display(Name = "Atribuição de CJ - Consulta")]
        ACJ_C = 18,

        [Display(Name = "Atribuição de CJ - Inclusão")]
        ACJ_I = 19,

        [Display(Name = "Atribuição de CJ - Exclusão")]
        ACJ_E = 20,

        [Display(Name = "Atribuição de CJ - Alteração")]
        ACJ_A = 21,

        [Display(Name = "Nota/Conceito - Consulta")]
        NC_C = 22,

        [Display(Name = "Nota/Conceito - Inclusão")]
        NC_I = 23,

        [Display(Name = "Nota/Conceito - Exclusão")]
        NC_E = 24,

        [Display(Name = "Nota/Conceito - Alteração")]
        NC_A = 25,

        [Display(Name = "Plano Anual - Consulta")]
        PA_C = 26,

        [Display(Name = "Plano Anual - Inclusão")]
        PA_I = 27,

        [Display(Name = "Plano Anual - Exclusão")]
        PA_E = 28,

        [Display(Name = "Plano Anual - Alteração")]
        PA_A = 29,

        [Display(Name = "Plano de Aula- Consulta")]
        PDA_C = 30,

        [Display(Name = "Plano de Aula - Inclusão")]
        PDA_I = 31,

        [Display(Name = "Plano de Aula - Exclusão")]
        PDA_E = 32,

        [Display(Name = "Plano de Aula - Alteração")]
        PDA_A = 33,

        [Display(Name = "Plano de Ciclo - Consulta")]
        PDC_C = 34,

        [Display(Name = "Plano de Ciclo - Inclusão")]
        PDC_I = 35,

        [Display(Name = "Plano de Ciclo - Exclusão")]
        PDC_E = 36,

        [Display(Name = "Plano de Ciclo - Alteração")]
        PDC_A = 37,

        [Display(Name = "Notificação - Consulta")]
        N_C = 38,

        [Display(Name = "Notificação - Inclusão")]
        N_I = 39,

        [Display(Name = "Notificação - Exclusão")]
        N_E = 40,

        [Display(Name = "Notificação - Alteração")]
        N_A = 41,

        [Display(Name = "Atribuição Professor - Consulta")]
        AP_C = 42,

        [Display(Name = "Atribuição Professor - Inclusão")]
        AP_I = 43,

        [Display(Name = "Atribuição Professor - Exclusão")]
        AP_E = 44,

        [Display(Name = "Atribuição Professor - Alteração")]
        AP_A = 45,

        [Display(Name = "Reiniciar Senha - Alteração")]
        AS_C = 47,

        [Display(Name = "Meus Dados - Consulta")]
        M_C = 48,

        [Display(Name = "Meus Dados - Inclusão")]
        M_I = 49,

        [Display(Name = "Meus Dados - Exclusão")]
        M_E = 50,

        [Display(Name = "Meus Dados - Alteração")]
        M_A = 51
    }
}