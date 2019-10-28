namespace SME.SGP.Infra
{
    public enum Permissao
    {
        #region Diário de Classe

        [PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "/sondagem/", EhConsulta = true)]
        S_C = 1,

        [PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "/sondagem/", EhInclusao = true)]
        S_I = 2,

        [PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "/sondagem/", EhExclusao = true)]
        S_E = 3,

        [PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, Url = "/sondagem/", EhAlteracao = true)]
        S_A = 4,

        [PermissaoMenu(Menu = "Boletim", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "/boletim/", EhConsulta = true)]
        B_C = 9,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/frequencia/", EhAlteracao = true)]
        F_A = 17,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/notas/", EhConsulta = true)]
        NC_C = 22,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/notas/", EhInclusao = true)]
        NC_I = 23,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/notas/", EhExclusao = true)]
        NC_E = 24,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/notas/", EhAlteracao = true)]
        NC_A = 25,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/frequencia/", EhConsulta = true)]
        PDA_C = 30,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/frequencia/", EhInclusao = true)]
        PDA_I = 31,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/frequencia/", EhExclusao = true)]
        PDA_E = 32,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/frequencia/", EhAlteracao = true)]
        PDA_A = 33,

        #endregion Diário de Classe

        #region Relatórios

        [PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "/relatorios/sondagem/")]
        SR_C = 5,

        [PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "/relatorios/sondagem/novo")]
        SR_I = 6,

        [PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        SR_E = 7,

        [PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8, Url = "/relatorios/sondagem/editar/:id")]
        SR_A = 8,

        [PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/frequencia/", EhConsulta = true)]
        F_C = 14,

        [PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/frequencia/", EhInclusao = true)]
        F_I = 15,

        [PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/frequencia/", EhExclusao = true)]
        F_E = 16,

        [PermissaoMenu(Menu = "Relatório Consulta", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/relatorios/")]
        R_C = 46,

        #endregion Relatórios

        #region Calendário Escolar

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendarios/", EhConsulta = true)]
        C_C = 10,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendarios/", EhInclusao = true)]
        C_I = 11,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendarios/", EhExclusao = true)]
        C_E = 12,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendarios/", EhAlteracao = true)]
        C_A = 13,

        #endregion Calendário Escolar

        #region Gestão

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, Url = "/atribuicao-cj/", EhConsulta = true)]
        ACJ_C = 18,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, Url = "/atribuicao-cj/", EhInclusao = true)]
        ACJ_I = 19,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, Url = "/atribuicao-cj/", EhExclusao = true)]
        ACJ_E = 20,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, Url = "/atribuicao-cj/", EhAlteracao = true)]
        ACJ_A = 21,

        #endregion Gestão

        #region Planejamento

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual/", EhConsulta = true)]
        PA_C = 26,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual/", EhInclusao = true)]
        PA_I = 27,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual/", EhExclusao = true)]
        PA_E = 28,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual/", EhAlteracao = true)]
        PA_A = 29,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo/", EhConsulta = true)]
        PDC_C = 34,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo/", EhInclusao = true)]
        PDC_I = 35,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo/", EhExclusao = true)]
        PDC_E = 36,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo/", EhAlteracao = true)]
        PDC_A = 37,

        #endregion Planejamento

        [PermissaoMenu(EhMenu = false, EhConsulta = true, Menu = "Notificação", Agrupamento = "Notificação")]
        N_C = 38,

        [PermissaoMenu(EhMenu = false, EhInclusao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        N_I = 39,

        [PermissaoMenu(EhMenu = false, EhExclusao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        N_E = 40,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        N_A = 41,

        [PermissaoMenu(EhMenu = false, EhConsulta = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_C = 42,

        [PermissaoMenu(EhMenu = false, EhInclusao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_I = 43,

        [PermissaoMenu(EhMenu = false, EhExclusao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_E = 44,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_A = 45,

        [PermissaoMenu(Menu = "Usuários", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 7, OrdemMenu = 1, Url = "/usuarios/reiniciar-senha/", EhAlteracao = true,
           EhSubMenu = true, SubMenu = "Reiniciar Senha")]
        AS_C = 47,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        M_C = 48,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        M_I = 49,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        M_E = 50,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação")]
        M_A = 51,
    }
}