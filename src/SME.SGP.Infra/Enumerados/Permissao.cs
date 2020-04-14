namespace SME.SGP.Infra
{
    public enum Permissao
    {
        /*Retirar comentário após a implementação dos menus*/
        //[PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, EhConsulta = true)]
        //S_C = 1,

        //[PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, EhInclusao = true)]
        //S_I = 2,

        //[PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, EhExclusao = true)]
        //S_E = 3,

        //[PermissaoMenu(Menu = "Sondagem", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 9, EhAlteracao = true)]
        //S_A = 4,

        //[PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        //SR_C = 5,

        //[PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        //SR_I = 6,

        //[PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        //SR_E = 7,

        //[PermissaoMenu(Menu = "Relatório de Sondagem", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 8)]
        //SR_A = 8,

        [PermissaoMenu(Menu = "Boletim", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 3, EhConsulta = true)]
        B_C = 9,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendario-escolar", EhConsulta = true)]
        C_C = 10,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendario-escolar", EhInclusao = true)]
        C_I = 11,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendario-escolar", EhExclusao = true)]
        C_E = 12,

        [PermissaoMenu(Menu = "Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 1, Url = "/calendario-escolar", EhAlteracao = true)]
        C_A = 13,

        //[PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", EhMenu = false, EhConsulta = true)]
        //F_C = 14,

        //[PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", EhMenu = false, EhInclusao = true)]
        //F_I = 15,

        //[PermissaoMenu(Menu = "Frequência", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", EhMenu = false, EhExclusao = true)]
        //F_E = 16,

        [PermissaoMenu(Menu = "Plano de aula/Frequência", Icone = "fas fa-book-reader", EhMenu = false, EhAlteracao = true)]
        F_A = 17,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, EhConsulta = true, Url = "/gestao/atribuicao-cjs")]
        ACJ_C = 18,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, EhInclusao = true, Url = "/gestao/atribuicao-cjs/editar")]
        ACJ_I = 19,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, EhExclusao = true, Url = "/gestao/atribuicao-cjs/editar")]
        ACJ_E = 20,

        [PermissaoMenu(Menu = "Atribuição de CJ", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 2, EhAlteracao = true, Url = "/gestao/atribuicao-cjs/editar")]
        ACJ_A = 21,

        [PermissaoMenu(Menu = "Comunicados", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 6, EhConsulta = true, Url = "/gestao/acompanhamento-escolar/comunicados")]
        CO_C = 140,

        [PermissaoMenu(Menu = "Comunicados", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 6, EhExclusao = true, Url = "/gestao/acompanhamento-escolar/comunicados")]
        CO_E = 142,

        [PermissaoMenu(Menu = "Comunicados", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 6, EhAlteracao = true, Url = "/gestao/acompanhamento-escolar/comunicados/novo")]
        CO_A = 143,

        [PermissaoMenu(Menu = "Comunicados", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 6, EhInclusao = true, Url = "/gestao/acompanhamento-escolar/comunicados/novo")]
        CO_I = 141,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhConsulta = true, Url = "/diario-classe/notas")]
        NC_C = 22,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhInclusao = true, Url = "/diario-classe/notas")]
        NC_I = 23,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhExclusao = true, Url = "/diario-classe/notas")]
        NC_E = 24,

        [PermissaoMenu(Menu = "Notas", Icone = "fas fa-file-alt", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhAlteracao = true, Url = "/diario-classe/notas")]
        NC_A = 25,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual", EhConsulta = true)]
        PA_C = 26,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual", EhInclusao = true)]
        PA_I = 27,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual", EhExclusao = true)]
        PA_E = 28,

        [PermissaoMenu(Menu = "Plano Anual", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/planejamento/plano-anual", EhAlteracao = true)]
        PA_A = 29,

        [PermissaoMenu(Menu = "Frequência/Plano de aula", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, EhConsulta = true, Url = "/diario-classe/frequencia-plano-aula")]
        PDA_C = 30,

        [PermissaoMenu(Menu = "Frequência/Plano de aula", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, EhInclusao = true, Url = "/diario-classe/frequencia-plano-aula")]
        PDA_I = 31,

        [PermissaoMenu(Menu = "Frequência/Plano de aula", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, EhExclusao = true, Url = "/diario-classe/frequencia-plano-aula")]
        PDA_E = 32,

        [PermissaoMenu(Menu = "Frequência/Plano de aula", Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 1, EhAlteracao = true, Url = "/diario-classe/frequencia-plano-aula")]
        PDA_A = 33,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo", EhConsulta = true)]
        PDC_C = 34,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo", EhInclusao = true)]
        PDC_I = 35,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo", EhExclusao = true)]
        PDC_E = 36,

        [PermissaoMenu(Menu = "Plano de Ciclo", Icone = "fas fa-list-alt", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/planejamento/plano-ciclo", EhAlteracao = true)]
        PDC_A = 37,

        [PermissaoMenu(EhMenu = false, EhConsulta = true, Menu = "Notificação", Agrupamento = "Notificação", Url = "/notificacoes")]
        N_C = 38,

        [PermissaoMenu(EhMenu = false, EhInclusao = true, Menu = "Notificação", Agrupamento = "Notificação", Url = "/notificacoes")]
        N_I = 39,

        [PermissaoMenu(EhMenu = false, EhExclusao = true, Menu = "Notificação", Agrupamento = "Notificação", Url = "/notificacoes")]
        N_E = 40,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Notificação", Agrupamento = "Notificação", Url = "/notificacoes")]
        N_A = 41,

        [PermissaoMenu(EhMenu = false, EhConsulta = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_C = 42,

        [PermissaoMenu(EhMenu = false, EhInclusao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_I = 43,

        [PermissaoMenu(EhMenu = false, EhExclusao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_E = 44,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Atribuição Professor", Agrupamento = "Atribuição Professor")]
        AP_A = 45,

        //[PermissaoMenu(Menu = "Relatório Consulta", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 2, OrdemMenu = 2)]
        //R_C = 46,

        [PermissaoMenu(Menu = "Usuários", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 8, OrdemMenu = 1, Url = "/usuarios/reiniciar-senha", EhAlteracao = true,
           EhSubMenu = true, EhConsulta = true, SubMenu = "Reiniciar Senha")]
        AS_C = 47,

        [PermissaoMenu(EhMenu = false, EhConsulta = true, Menu = "Meus Dados", Agrupamento = "Meus Dados", Url = "/meus-dados")]
        M_C = 48,

        [PermissaoMenu(EhMenu = false, EhInclusao = true, Menu = "Meus Dados", Agrupamento = "Meus Dados", Url = "/meus-dados")]
        M_I = 49,

        [PermissaoMenu(EhMenu = false, EhExclusao = true, Menu = "Meus Dados", Agrupamento = "Meus Dados", Url = "/meus-dados")]
        M_E = 50,

        [PermissaoMenu(EhMenu = false, EhAlteracao = true, Menu = "Meus Dados", Agrupamento = "Meus Dados", Url = "/meus-dados")]
        M_A = 51,

        [PermissaoMenu(Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhConsulta = true, Menu = "Notas")]
        NT_C = 52,

        [PermissaoMenu(Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhInclusao = true, Menu = "Notas")]
        NT_I = 53,

        [PermissaoMenu(Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhExclusao = true, Menu = "Notas")]
        NT_E = 54,

        [PermissaoMenu(Icone = "fas fa-book-reader", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 2, EhAlteracao = true, Menu = "Notas")]
        NT_A = 55,

        [PermissaoMenu(EhMenu = false, Icone = "fas fa-book-reader", Agrupamento = "Registro POA", OrdemAgrupamento = 2, OrdemMenu = 3, EhConsulta = true, Menu = "Registro POA")]
        RP_C = 56,

        [PermissaoMenu(EhMenu = false, Icone = "fas fa-book-reader", Agrupamento = "Registro POA", OrdemAgrupamento = 2, OrdemMenu = 3, EhInclusao = true, Menu = "Registro POA")]
        RP_I = 57,

        [PermissaoMenu(EhMenu = false, Icone = "fas fa-book-reader", Agrupamento = "Registro POA", OrdemAgrupamento = 2, OrdemMenu = 3, EhExclusao = true, Menu = "Registro POA")]
        RP_E = 58,

        [PermissaoMenu(EhMenu = false, Icone = "fas fa-book-reader", Agrupamento = "Registro POA", OrdemAgrupamento = 2, OrdemMenu = 3, EhAlteracao = true, Menu = "Registro POA")]
        RP_A = 59,

        [PermissaoMenu(Menu = "Calendário Professor", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 2, EhConsulta = true, Url = "/calendario-escolar/calendario-professor")]
        CP_C = 60,

        [PermissaoMenu(Menu = "Calendário Professor", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 2, EhInclusao = true, Url = "/calendario-escolar/calendario-professor")]
        CP_I = 61,

        [PermissaoMenu(Menu = "Calendário Professor", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 2, EhExclusao = true, Url = "/calendario-escolar/calendario-professor")]
        CP_E = 62,

        [PermissaoMenu(Menu = "Calendário Professor", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 2, EhAlteracao = true, Url = "/calendario-escolar/calendario-professor")]
        CP_A = 63,

        [PermissaoMenu(Menu = "Tipo de Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 3, EhConsulta = true, Url = "/calendario-escolar/tipo-calendario-escolar")]
        TCE_C = 64,

        [PermissaoMenu(Menu = "Tipo de Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 3, EhInclusao = true, Url = "/calendario-escolar/tipo-calendario-escolar")]
        TCE_I = 65,

        [PermissaoMenu(Menu = "Tipo de Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 3, EhExclusao = true, Url = "/calendario-escolar/tipo-calendario-escolar")]
        TCE_E = 66,

        [PermissaoMenu(Menu = "Tipo de Calendário Escolar", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 3, EhAlteracao = true, Url = "/calendario-escolar/tipo-calendario-escolar")]
        TCE_A = 67,

        [PermissaoMenu(Menu = "Períodos Escolares", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 4, EhConsulta = true, Url = "/calendario-escolar/periodos-escolares")]
        PE_C = 68,

        [PermissaoMenu(Menu = "Períodos Escolares", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 4, EhInclusao = true, Url = "/calendario-escolar/periodos-escolares")]
        PE_I = 69,

        [PermissaoMenu(Menu = "Períodos Escolares", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 4, EhExclusao = true, Url = "/calendario-escolar/periodos-escolares")]
        PE_E = 70,

        [PermissaoMenu(Menu = "Períodos Escolares", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 4, EhAlteracao = true, Url = "/calendario-escolar/periodos-escolares")]
        PE_A = 71,

        [PermissaoMenu(Menu = "Períodos de fechamento (Abertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 5, EhConsulta = true, Url = "/calendario-escolar/periodo-fechamento-abertura")]
        PFA_C = 72,

        [PermissaoMenu(Menu = "Períodos de fechamento (Abertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 5, EhInclusao = true, Url = "/calendario-escolar/periodo-fechamento-abertura")]
        PFA_I = 73,

        [PermissaoMenu(Menu = "Períodos de fechamento (Abertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 5, EhExclusao = true, Url = "/calendario-escolar/periodo-fechamento-abertura")]
        PFA_E = 74,

        [PermissaoMenu(Menu = "Períodos de fechamento (Abertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 5, EhAlteracao = true, Url = "/calendario-escolar/periodo-fechamento-abertura")]
        PFA_A = 75,

        /*Retirar comentário após a implementação dos menus*/

        [PermissaoMenu(Menu = "Períodos de fechamento (Reabertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 6, EhConsulta = true, Url = "/calendario-escolar/periodo-fechamento-reabertura")]
        PFR_C = 76,

        [PermissaoMenu(Menu = "Períodos de fechamento (Reabertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 6, EhInclusao = true, Url = "/calendario-escolar/periodo-fechamento-reabertura")]
        PFR_I = 77,

        [PermissaoMenu(Menu = "Períodos de fechamento (Reabertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 6, EhExclusao = true, Url = "/calendario-escolar/periodo-fechamento-reabertura")]
        PFR_E = 78,

        [PermissaoMenu(Menu = "Períodos de fechamento (Reabertura)", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 6, EhAlteracao = true, Url = "/calendario-escolar/periodo-fechamento-reabertura")]
        PFR_A = 79,

        [PermissaoMenu(Menu = "Tipo de Feriado", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 7, EhConsulta = true, Url = "/calendario-escolar/tipo-feriado")]
        TF_C = 80,

        [PermissaoMenu(Menu = "Tipo de Feriado", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 7, EhInclusao = true, Url = "/calendario-escolar/tipo-feriado")]
        TF_I = 81,

        [PermissaoMenu(Menu = "Tipo de Feriado", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 7, EhExclusao = true, Url = "/calendario-escolar/tipo-feriado")]
        TF_E = 82,

        [PermissaoMenu(Menu = "Tipo de Feriado", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 7, EhAlteracao = true, Url = "/calendario-escolar/tipo-feriado")]
        TF_A = 83,

        [PermissaoMenu(Menu = "Tipo de Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 8, EhConsulta = true, Url = "/calendario-escolar/tipo-eventos")]
        TE_C = 84,

        [PermissaoMenu(Menu = "Tipo de Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 8, EhInclusao = true, Url = "/calendario-escolar/tipo-eventos")]
        TE_I = 85,

        [PermissaoMenu(Menu = "Tipo de Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 8, EhExclusao = true, Url = "/calendario-escolar/tipo-eventos")]
        TE_E = 86,

        [PermissaoMenu(Menu = "Tipo de Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 8, EhAlteracao = true, Url = "/calendario-escolar/tipo-eventos")]
        TE_A = 87,

        [PermissaoMenu(Menu = "Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 9, EhConsulta = true, Url = "/calendario-escolar/eventos")]
        E_C = 88,

        [PermissaoMenu(Menu = "Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 9, EhInclusao = true, Url = "/calendario-escolar/eventos")]
        E_I = 89,

        [PermissaoMenu(Menu = "Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 9, EhExclusao = true, Url = "/calendario-escolar/eventos")]
        E_E = 90,

        [PermissaoMenu(Menu = "Eventos", Icone = "fas fa-calendar-alt", Agrupamento = "Calendário Escolar", OrdemAgrupamento = 5, OrdemMenu = 9, EhAlteracao = true, Url = "/calendario-escolar/eventos")]
        E_A = 91,

        [PermissaoMenu(Menu = "Atribuição esporádica", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, EhMenu = false, EhConsulta = true, Url = "/gestao/atribuicao-esporadica")]
        AE_C = 92,

        [PermissaoMenu(Menu = "Atribuição esporádica", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, EhMenu = false, EhInclusao = true, Url = "/gestao/atribuicao-esporadica")]
        AE_I = 93,

        [PermissaoMenu(Menu = "Atribuição esporádica", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, EhMenu = false, EhExclusao = true, Url = "/gestao/atribuicao-esporadica")]
        AE_E = 94,

        [PermissaoMenu(Menu = "Atribuição esporádica", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, EhMenu = false, EhAlteracao = true, Url = "/gestao/atribuicao-esporadica")]
        AE_A = 95,

        [PermissaoMenu(Menu = "Atribuição Supervisor", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 5, EhConsulta = true, Url = "/gestao/atribuicao-supervisor-lista")]
        ASP_C = 96,

        [PermissaoMenu(Menu = "Atribuição Supervisor", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 5, EhInclusao = true, Url = "/gestao/atribuicao-supervisor-lista")]
        ASP_I = 97,

        [PermissaoMenu(Menu = "Atribuição Supervisor", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 5, EhExclusao = true, Url = "/gestao/atribuicao-supervisor-lista")]
        ASP_E = 98,

        [PermissaoMenu(Menu = "Atribuição Supervisor", Icone = "fas fa-user-cog", Agrupamento = "Gestão", OrdemAgrupamento = 6, OrdemMenu = 5, EhAlteracao = true, Url = "/gestao/atribuicao-supervisor-lista")]
        ASP_A = 99,

        [PermissaoMenu(Menu = "Tipo de Avaliação", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 8, OrdemMenu = 2, EhConsulta = true, Url = "/configuracoes/tipo-avaliacao")]
        TA_C = 100,

        [PermissaoMenu(Menu = "Tipo de Avaliação", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 8, OrdemMenu = 2, EhInclusao = true, Url = "/configuracoes/tipo-avaliacao")]
        TA_I = 101,

        [PermissaoMenu(Menu = "Tipo de Avaliação", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 8, OrdemMenu = 2, EhExclusao = true, Url = "/configuracoes/tipo-avaliacao")]
        TA_E = 102,

        [PermissaoMenu(Menu = "Tipo de Avaliação", Icone = "fas fa-book-reader", Agrupamento = "Configurações", OrdemAgrupamento = 8, OrdemMenu = 2, EhAlteracao = true, Url = "/configuracoes/tipo-avaliacao")]
        TA_A = 103,

        [PermissaoMenu(Menu = "Aula prevista X Aula dada", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 4, EhInclusao = true, Url = "/diario-classe/aula-dada-aula-prevista")]
        ADAP_I = 104,

        [PermissaoMenu(Menu = "Aula prevista X Aula dada", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 4, EhAlteracao = true, Url = "/diario-classe/aula-dada-aula-prevista")]
        ADAP_A = 105,

        [PermissaoMenu(Menu = "Aula prevista X Aula dada", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 4, EhConsulta = true, Url = "/diario-classe/aula-dada-aula-prevista")]
        ADAP_C = 106,

        [PermissaoMenu(Menu = "Aula prevista X Aula dada", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 4, EhExclusao = true, Url = "/diario-classe/aula-dada-aula-prevista")]
        ADAP_E = 107,

        [PermissaoMenu(Menu = "Registro POA", Icone = "", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhInclusao = true, Url = "/diario-classe/registro-poa")]
        RPOA_I = 108,

        [PermissaoMenu(Menu = "Registro POA", Icone = "", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhAlteracao = true, Url = "/diario-classe/registro-poa")]
        RPOA_A = 109,

        [PermissaoMenu(Menu = "Registro POA", Icone = "", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhConsulta = true, Url = "/diario-classe/registro-poa")]
        RPOA_C = 110,

        [PermissaoMenu(Menu = "Registro POA", Icone = "", Agrupamento = "Planejamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhExclusao = true, Url = "/diario-classe/registro-poa")]
        RPOA_E = 111,

        [PermissaoMenu(Menu = "Compensação de Ausência", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 6, EhConsulta = true, Url = "/diario-classe/compensacao-ausencia")]
        CA_C = 112,

        [PermissaoMenu(Menu = "Compensação de Ausência", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 6, EhInclusao = true, Url = "/diario-classe/compensacao-ausencia")]
        CA_I = 113,

        [PermissaoMenu(Menu = "Compensação de Ausência", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 6, EhExclusao = true, Url = "/diario-classe/compensacao-ausencia")]
        CA_E = 114,

        [PermissaoMenu(Menu = "Compensação de Ausência", Icone = "", Agrupamento = "Diário de Classe", OrdemAgrupamento = 1, OrdemMenu = 6, EhAlteracao = true, Url = "/diario-classe/compensacao-ausencia")]
        CA_A = 115,

        [PermissaoMenu(Menu = "Resumos e gráficos PAP", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 7, OrdemMenu = 2, EhConsulta = true, Url = "/relatorios/pap/resumos-e-graficos")]
        RGP_C = 120,

        [PermissaoMenu(Menu = "Preenchimento PAP", Icone = "fas fa-file-alt", Agrupamento = "Relatórios", OrdemAgrupamento = 7, OrdemMenu = 1, EhConsulta = true, Url = "/relatorios/pap/acompanhamento")]
        RAP_C = 116,

        [PermissaoMenu(Menu = "Fechamento de Bimestre", Icone = "fas fa-pencil-ruler", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 1, EhConsulta = true, Url = "/fechamento/fechamento-bimestre")]
        FB_C = 124,

        [PermissaoMenu(Menu = "Fechamento de Bimestre", Icone = "fas fa-pencil-ruler", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 1, EhInclusao = true, Url = "/fechamento/fechamento-bimestre")]
        FB_I = 125,

        [PermissaoMenu(Menu = "Fechamento de Bimestre", Icone = "fas fa-pencil-ruler", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 1, EhExclusao = true, Url = "/fechamento/fechamento-bimestre")]
        FB_E = 126,

        [PermissaoMenu(Menu = "Fechamento de Bimestre", Icone = "fas fa-pencil-ruler", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 1, EhAlteracao = true, Url = "/fechamento/fechamento-bimestre")]
        FB_A = 127,

        [PermissaoMenu(Menu = "Pendências do Fechamento", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 2, EhConsulta = true, Url = "/fechamento/pendencias-fechamento")]
        PF_C = 128,

        [PermissaoMenu(Menu = "Pendências do Fechamento", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 2, EhInclusao = true, Url = "/fechamento/pendencias-fechamento")]
        PF_I = 129,

        [PermissaoMenu(Menu = "Pendências do Fechamento", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 2, EhExclusao = true, Url = "/fechamento/pendencias-fechamento")]
        PF_E = 130,

        [PermissaoMenu(Menu = "Pendências do Fechamento", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 2, EhAlteracao = true, Url = "/fechamento/pendencias-fechamento")]
        PF_A = 131,
        
        [PermissaoMenu(Menu = "Conselho de Classe", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhConsulta = true, Url = "/fechamento/conselho-classe")]
        CC_C = 136,

        [PermissaoMenu(Menu = "Conselho de Classe", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhInclusao = true, Url = "/fechamento/conselho-classe")]
        CC_I = 137,

        [PermissaoMenu(Menu = "Conselho de Classe", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhExclusao = true, Url = "/fechamento/conselho-classe")]
        CC_E = 138,

        [PermissaoMenu(Menu = "Conselho de Classe", Icone = "", Agrupamento = "Fechamento", OrdemAgrupamento = 2, OrdemMenu = 3, EhAlteracao = true, Url = "/fechamento/conselho-classe")]
        CC_A = 139,
    }
}