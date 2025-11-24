using SME.SGP.Infra.Constantes;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public enum Permissao
    {
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_CICLO,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhConsulta = true)]
        PDC_C = 34,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_CICLO,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        PDC_I = 35,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_CICLO,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        PDC_E = 36,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_CICLO,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        PDC_A = 37,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_ANUAL,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_MINUS,
            EhConsulta = true)]
        PA_C = 26,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_ANUAL,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        PA_I = 27,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_ANUAL,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        PA_E = 28,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_ANUAL,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        PA_A = 29,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TERRITORIO_SABER,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhConsulta = true)]
        PT_C = 132,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TERRITORIO_SABER,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        PT_I = 133,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TERRITORIO_SABER,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        PT_E = 134,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TERRITORIO_SABER,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        PT_A = 135,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CARTA_DE_INTENCOES,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_ENVELOPE_OPEN,
            EhConsulta = true)]
        CI_C = 163,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CARTA_DE_INTENCOES,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        CI_I = 164,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CARTA_DE_INTENCOES,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        CI_E = 165,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CARTA_DE_INTENCOES,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        CI_A = 166,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_POA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        RPOA_I = 108,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_POA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        RPOA_A = 109,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_POA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhConsulta = true)]
        RPOA_C = 110,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_PLANEJAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_POA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        RPOA_E = 111,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_LISTAO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhConsulta = true)]
        L_C = 228,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_LISTAO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhInclusao = true)]
        L_I = 229,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_LISTAO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhExclusao = true)]
        L_E = 230,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_LISTAO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhAlteracao = true)]
        L_A = 231,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_AULAS_DADAS_X_PREVISTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_AULAS_DADAS_AULAS_PREVISTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhInclusao = true)]
        ADAP_I = 104,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_AULAS_DADAS_X_PREVISTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_AULAS_DADAS_AULAS_PREVISTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",
            EhAlteracao = true)]
        ADAP_A = 105,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_AULAS_DADAS_X_PREVISTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_AULAS_DADAS_AULAS_PREVISTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",
            EhConsulta = true)]
        ADAP_C = 106,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_AULAS_DADAS_X_PREVISTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_AULAS_DADAS_AULAS_PREVISTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",
            EhExclusao = true)]
        ADAP_E = 107,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_DO_PROFESSOR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhConsulta = true)]
        CP_C = 60,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_DO_PROFESSOR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhInclusao = true)]
        CP_I = 61,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_DO_PROFESSOR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhExclusao = true)]
        CP_E = 62,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_DO_PROFESSOR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        CP_A = 63,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FREQUENCIA_PLANO_DE_AULA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhConsulta = true)]
        PDA_C = 30,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FREQUENCIA_PLANO_DE_AULA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhInclusao = true)]
        PDA_I = 31,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FREQUENCIA_PLANO_DE_AULA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhExclusao = true)]
        PDA_E = 32,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FREQUENCIA_PLANO_DE_AULA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhAlteracao = true)]
        PDA_A = 33,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_NOTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_NOTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhConsulta = true)]
        NC_C = 22,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_NOTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_NOTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        NC_I = 23,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_NOTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_NOTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        NC_E = 24,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_NOTAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_NOTAS,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        NC_A = 25,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMPENSACAO_DE_AUSENCIA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",
            EhConsulta = true)]
        CA_C = 112,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMPENSACAO_DE_AUSENCIA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",
            EhInclusao = true)]
        CA_I = 113,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMPENSACAO_DE_AUSENCIA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",
            EhExclusao = true)]
        CA_E = 114,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMPENSACAO_DE_AUSENCIA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",
            EhAlteracao = true)]
        CA_A = 115,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DIARIO_DE_BORDO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_FILE_ALT,
            EhConsulta = true)]
        DDB_C = 159,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DIARIO_DE_BORDO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        DDB_I = 160,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DIARIO_DE_BORDO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        DDB_E = 161,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DIARIO_DE_BORDO,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        DDB_A = 162,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_INDIVIDUAL,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhConsulta = true)]
        REI_C = 189,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_INDIVIDUAL,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        REI_I = 190,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_INDIVIDUAL,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        REI_E = 191,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_INDIVIDUAL,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        REI_A = 192,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_CJ,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ACJ_C = 18,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_CJ,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        ACJ_I = 19,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_CJ,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        ACJ_E = 20,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_CJ,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ACJ_A = 21,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DE_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ACOMPANHAMENTO_DE_FREQUENCIA,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_ACOMPANHAMENTO_FREQUENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhConsulta = true)]
        AFQ_C = 201,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DE_PAP,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DE_PAP,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PAP_RELATORIO_PAP,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RPAP_C = 246,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DE_PAP,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DE_PAP,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PAP_RELATORIO_PAP,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhInclusao = true)]
        RPAP_I = 247,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DE_PAP,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DE_PAP,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PAP_RELATORIO_PAP,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhExclusao = true)]
        RPAP_E = 248,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DE_PAP,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DE_PAP,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PAP_RELATORIO_PAP,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RPAP_A = 249,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_SONDAGEM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_SONDAGEM_ACESSO,
            Url = ConstantesMenuPermissao.ROTA_SONDAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        S_C = 5,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_MAPEAMENTO_ESTUDANTES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_MAPEAMENTO_ESTUDANTES,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_MAPEAMENTO_ESTUDANTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        ME_C = 272,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_MAPEAMENTO_ESTUDANTES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_MAPEAMENTO_ESTUDANTES,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_MAPEAMENTO_ESTUDANTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhInclusao = true)]
        ME_I = 273,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_MAPEAMENTO_ESTUDANTES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_MAPEAMENTO_ESTUDANTES,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_MAPEAMENTO_ESTUDANTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhExclusao = true)]
        ME_E = 274,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_MAPEAMENTO_ESTUDANTES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DIARIO_DE_CLASSE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_MAPEAMENTO_ESTUDANTES,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_MAPEAMENTO_ESTUDANTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        ME_A = 275,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DO_BIMESTRE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FECHAMENTO_DO_BIMESTRE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
        FB_C = 124,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DO_BIMESTRE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FECHAMENTO_DO_BIMESTRE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhInclusao = true)]
        FB_I = 125,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DO_BIMESTRE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FECHAMENTO_DO_BIMESTRE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhExclusao = true)]
        FB_E = 126,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DO_BIMESTRE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_FECHAMENTO_DO_BIMESTRE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhAlteracao = true)]
        FB_A = 127,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CONSELHO_DE_CLASSE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhConsulta = true)]
        CC_C = 136,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CONSELHO_DE_CLASSE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhInclusao = true)]
        CC_I = 137,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CONSELHO_DE_CLASSE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhExclusao = true)]
        CC_E = 138,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CONSELHO_DE_CLASSE,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhAlteracao = true)]
        CC_A = 139,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DO_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ACOMPANHAMENTO_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
        ACF_C = 218,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PENDENCIAS_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        PF_C = 128,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PENDENCIAS_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhInclusao = true)]
        PF_I = 129,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PENDENCIAS_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhExclusao = true)]
        PF_E = 130,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PENDENCIAS_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhAlteracao = true)]
        PF_A = 131,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_RAA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RAA,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
        RAA_C = 210,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_RAA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RAA,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhInclusao = true)]
        RAA_I = 211,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_RAA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RAA,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhExclusao = true)]
        RAA_E = 212,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO,
            Menu = ConstantesMenuPermissao.MENU_RAA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_FECHAMENTO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RAA,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhAlteracao = true)]
        RAA_A = 213,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS_EI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DEVOLUTIVAS_EI,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhConsulta = true)]
        DE_C = 167,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS_EI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DEVOLUTIVAS_EI,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        DE_I = 168,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS_EI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DEVOLUTIVAS_EI,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        DE_E = 169,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS_EI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DEVOLUTIVAS_EI,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        DE_A = 170,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhConsulta = true)]
        C_C = 10,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhInclusao = true)]
        C_I = 11,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhExclusao = true)]
        C_E = 12,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        C_A = 13,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        DPU_C = 177,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        DPU_I = 178,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        DPU_E = 179,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        DPU_A = 180,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_BIMESTRES,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        PE_C = 68,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_BIMESTRES,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        PE_I = 69,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_BIMESTRES,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        PE_E = 70,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_BIMESTRES,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        PE_A = 71,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_ABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        PFA_C = 72,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_ABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        PFA_I = 73,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_ABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        PFA_E = 74,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_ABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        PFA_A = 75,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_REABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        PFR_C = 76,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_REABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        PFR_I = 77,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_REABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        PFR_E = 78,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PERIODOS_ESCOLARES_REABERTURA,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        PFR_A = 79,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_EVENTOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_EVENTOS,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_CHECK,
            EhConsulta = true)]
        E_C = 88,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_EVENTOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_EVENTOS,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        E_I = 89,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_EVENTOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_EVENTOS,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        E_E = 90,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_EVENTOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_EVENTOS,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        E_A = 91,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_EVENTO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        TE_C = 84,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_EVENTO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        TE_I = 85,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_EVENTO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        TE_E = 86,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_EVENTO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        TE_A = 87,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_FERIADO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        TF_C = 80,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_FERIADO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        TF_I = 81,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_FERIADO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        TF_E = 82,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPO_DE_FERIADO,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        TF_A = 83,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPOS_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
        TCE_C = 64,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPOS_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
        TCE_I = 65,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPOS_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
        TCE_E = 66,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_TIPOS_DE_CALENDARIO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPOS_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        TCE_A = 67,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        ARP_C = 96,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        ARP_I = 97,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        ARP_E = 98,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_DE_RESPONSAVEIS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ARP_A = 99,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_ESPORADICA,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        AE_C = 92,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_ESPORADICA,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        AE_I = 93,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_ESPORADICA,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        AE_E = 94,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATRIBUICAO_ESPORADICA,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        AE_A = 95,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMUNICADOS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        CO_C = 140,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMUNICADOS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        CO_E = 142,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMUNICADOS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS_NOVO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        CO_A = 143,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_COMUNICADOS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS_NOVO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        CO_I = 141,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_OCORRENCIAS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        OCO_C = 193,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_OCORRENCIAS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        OCO_I = 194,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_OCORRENCIAS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        OCO_E = 195,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_OCORRENCIAS,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        OCO_A = 196,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_INFORMES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMES,
            Url = ConstantesMenuPermissao.ROTA_INFORMES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        INF_C = 251,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_INFORMES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMES,
            Url = ConstantesMenuPermissao.ROTA_INFORMES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        INF_I = 252,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_INFORMES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMES,
            Url = ConstantesMenuPermissao.ROTA_INFORMES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        INF_E = 253,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_INFORMES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMES,
            Url = ConstantesMenuPermissao.ROTA_INFORMES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        INF_A = 254,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CADASTRO_DE_ABAE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CADASTRO_DE_ABAE,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_CADASTRO_DE_ABAE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ABA_A = 255,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CADASTRO_DE_ABAE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CADASTRO_DE_ABAE,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_CADASTRO_DE_ABAE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        ABA_E = 256,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CADASTRO_DE_ABAE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CADASTRO_DE_ABAE,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_CADASTRO_DE_ABAE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        ABA_I = 257,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
            Menu = ConstantesMenuPermissao.MENU_CADASTRO_DE_ABAE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_CADASTRO_DE_ABAE,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_CADASTRO_DE_ABAE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        ABA_C = 258,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhConsulta = true)]
        AEE_C = 197,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhInclusao = true)]
        AEE_I = 198,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhExclusao = true)]
        AEE_E = 199,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhAlteracao = true)]
        AEE_A = 200,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_PLANO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_PLANO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhConsulta = true)]
        PAEE_C = 202,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_PLANO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_PLANO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhInclusao = true)]
        PAEE_I = 203,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_PLANO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_PLANO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhExclusao = true)]
        PAEE_E = 204,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_PLANO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_PLANO_AEE,
            Url = ConstantesMenuPermissao.ROTA_AEE_PLANO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhAlteracao = true)]
        PAEE_A = 205,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_AEE_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhConsulta = true)]
        RI_C = 206,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_AEE_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhInclusao = true)]
        RI_I = 207,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_AEE_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhExclusao = true)]
        RI_E = 208,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_AEE,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_AEE,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_AEE_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_UNIVERSAL_ACCESS,
            EhAlteracao = true)]
        RI_A = 209,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ATENDIMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATENDIMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ATENDIMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        NAAPA_C = 235,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ATENDIMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATENDIMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ATENDIMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        NAAPA_I = 236,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ATENDIMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATENDIMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ATENDIMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        NAAPA_E = 237,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ATENDIMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATENDIMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ATENDIMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        NAAPA_A = 238,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        ENC_NAAPA_C = 294,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        ENC_NAAPA_I = 295,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        ENC_NAAPA_E = 296,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ENCAMINHAMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ENC_NAAPA_A = 297,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DINAMICO_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DINAMICO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIO_DINAMICO_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RDNAAPA_C = 250,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONS_CRIANCAS_ESTUD_AUSENTES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_CONS_CRIANCAS_ESTUD_AUSENTES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_CONS_CRIAN_ESTUD_AUSENTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        CCEA_NAAPA_A = 259,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONS_CRIANCAS_ESTUD_AUSENTES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_CONS_CRIANCAS_ESTUD_AUSENTES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_CONS_CRIAN_ESTUD_AUSENTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        CCEA_NAAPA_E = 260,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONS_CRIANCAS_ESTUD_AUSENTES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_CONS_CRIANCAS_ESTUD_AUSENTES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_CONS_CRIAN_ESTUD_AUSENTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        CCEA_NAAPA_I = 261,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONS_CRIANCAS_ESTUD_AUSENTES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_CONS_CRIANCAS_ESTUD_AUSENTES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_CONS_CRIAN_ESTUD_AUSENTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        CCEA_NAAPA_C = 262,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_REGISTRO_ACOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_REGISTRO_ACOES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_REGISTRO_ACOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        RABA_NAAPA_A = 263,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_REGISTRO_ACOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_REGISTRO_ACOES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_REGISTRO_ACOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        RABA_NAAPA_E = 264,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_REGISTRO_ACOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_REGISTRO_ACOES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_REGISTRO_ACOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        RABA_NAAPA_I = 265,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BUSCA_ATIVA_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_REGISTRO_ACOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_BUSCA_ATIVA_REGISTRO_ACOES,
            Url = ConstantesMenuPermissao.ROTA_NAAPA_BUSCA_ATIVA_REGISTRO_ACOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        RABA_NAAPA_C = 266,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_COLETIVO_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_COLETIVO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_REGISTRO_COLETIVO_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        RC_NAAPA_C = 268,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_COLETIVO_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_COLETIVO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_REGISTRO_COLETIVO_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        RC_NAAPA_I = 269,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_COLETIVO_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_COLETIVO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_REGISTRO_COLETIVO_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        RC_NAAPA_E = 270,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NAAPA,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_COLETIVO_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_NAAPA,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REGISTRO_COLETIVO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_REGISTRO_COLETIVO_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        RC_NAAPA_A = 271,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FREQUENCIA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_FREQUENCIA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_FREQUENCIA_FREQUENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhAlteracao = true)]
        FF_C = 156,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FREQUENCIA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_FREQUENCIA_MENSAL,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_FREQUENCIA_MENSAL,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_FREQUENCIA_MENSAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhAlteracao = true)]
        RFM_C = 232,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FREQUENCIA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONTROLE_DE_FREQUENCIA_MENSAL,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_CONTROLE_DE_FREQUENCIA_MENSAL,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_FREQUENCIA_CONTROLE_MENSAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhAlteracao = true)]
        RCFM_C = 244,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FREQUENCIA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_COMPENSACAO_AUSENCIA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_COMPENSACAO_AUSENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RCA_C = 172,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FREQUENCIA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_PRODUTIVIDADE_FREQUENCIA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_PRODUTIVIDADE_FREQUENCIA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PRODUTIVIDADE_FREQUENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RFP_C = 278,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DIARIO_DE_CLASSE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONTROLE_DE_GRADE,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_DIARIO_DE_CLASSE,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_DIARIO_CLASSE_CONTROLE_GRADE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RCG_C = 173,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DIARIO_DE_CLASSE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_CONTROLE_DE_PLANEJAMENTO_DIARIO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_CONTROLE_DE_PLANEJAMENTO_DIARIO,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_DIARIO_CLASSE_PLANEJAMENTO_DIARIO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RCP_C = 188,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DIARIO_DE_CLASSE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_DEVOLUTIVAS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PLANEJAMENTO_DEVOLUTIVAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RD_C = 214,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_DE_CLASSE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_DIARIO_DE_CLASSE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_MAPEAMENTO_ESTUDANTES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_MAPEAMENTO_ESTUDANTES,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_DIARIO_CLASSE_MAPEAMENTO_ESTUDANTES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RME_C = 276,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FECHAMENTO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_NOTAS_E_CONCEITOS_FINAIS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_NOTAS_E_CONCEITOS_FINAIS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_NOTAS_CONCEITOS_FINAIS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RNCF_C = 171,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FECHAMENTO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_PARECER_CONCLUSIVO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_PARECER_CONCLUSIVO,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_PARECER_CONCLUSIVO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RPC_C = 158,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FECHAMENTO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DO_FECHAMENTO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_ACOMPANHAMENTO_DO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_FECHAMENTOS_ACOMPANHAMENTO_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RACF_C = 224,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_FECHAMENTO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_HISTORICO_DE_ALTERACAO_DE_NOTAS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_HISTORICO_DE_ALTERACAO_DE_NOTAS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_FECHAMENTO_HISTORICO_ALTERACAO_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RDA_C = 184,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_PENDENCIAS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_PENDENCIAS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_GESTAO_PENDENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RPF_C = 157,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DOS_REGISTROS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_ACOMPANHAMENTO_DOS_REGISTROS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_GESTAO_ACOMPANHAMENTO_REGISTROS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RRP_C = 227,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ATRIBUICOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_ATRIBUICOES,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_GESTAO_ATRIBUICAO_CJ,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RACJ_C = 186,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_USUARIOS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_USUARIOS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_GESTAO_USUARIOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RDU_C = 182,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_NOTIFICACOES,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_NOTIFICACOES,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_NOTIFICACOES_HISTORICO_NOTIFICACOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        RDN_C = 183,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_GESTAO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_GESTAO,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_OCORRENCIAS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_OCORRENCIAS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = true)]
        ROCO_C = 245,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_AEE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_AEE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_ENCAMINHAMENTO_AEE,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_AEE_ENCAMINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        REAEE_C = 240,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_AEE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_AEE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_PLANO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_PLANO_AEE,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_AEE_PLANO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        RPAEE_C = 239,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_AEE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_AEE,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_AEE_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
        RERI_C = 243,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ENCAMINHAMENTO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_ENCAMINHAMENTO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_NAAPA_ENCMAINHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        RENAAPA_C = 241,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_NAAPA,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_BUSCA_ATIVA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_NAAPA_BUSCA_ATIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        RBA_C = 277,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_SONDAGEM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_SONDAGEM,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_RELATORIOS_ANALITICOS,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_RELATORIOS_ANALITICOS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_SONDAGEM_ANALITICOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = false,
            EhConsulta = true)]
        RESON_C = 242,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_SONDAGEM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_SONDAGEM,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_RELATORIOS_SONDAGEM_CONSOLIDADO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_RELATORIOS_CONSOLIDADO,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_SONDAGEM_CONSOLIDADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = false,
            EhConsulta = true)]
        RESON_CO = 287,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_SONDAGEM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_SONDAGEM,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_RELATORIOS_SONDAGEM_POR_TURMA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_RELATORIOS_POR_TURMA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_SONDAGEM_POR_TURMA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = false,
            EhConsulta = true)]
        RESON_P_T = 290,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_ESCOLA_AQUI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_ESCOLA_AQUI,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_ADESAO,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_RELATORIO_ADESAO,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_ESCOLA_AQUI_ADESAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = false,
            EhConsulta = true)]
        RDE_C = 187,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_RELATORIOS,
            Menu = ConstantesMenuPermissao.MENU_ESCOLA_AQUI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_RELATORIOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_RELATORIO_ESCOLA_AQUI,
            EhSubMenu = true,
            SubMenu = ConstantesMenuPermissao.MENU_LEITURA,
            OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_RELATORIO_RELATORIO_LEITURA,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_ESCOLA_AQUI_LEITURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhAlteracao = false,
            EhConsulta = true)]
        RLC_C = 185,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            Menu = ConstantesMenuPermissao.MENU_BOLETIM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_BOLETIM,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_DIARIO_CLASSE_BOLETIM_SIMPLES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
        B_C = 9,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            Menu = ConstantesMenuPermissao.MENU_ATA_BIMESTRAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATA_BIMESTRAL,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_ATAS_ATA_BIMESTRAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        ABR_C = 226,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            Menu = ConstantesMenuPermissao.MENU_ATA_FINAL_DE_RESULTADOS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_ATA_FINAL_DE_RESULTADOS,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_ATAS_ATA_FINAL_RESULTADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        AFR_C = 148,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            Menu = ConstantesMenuPermissao.MENU_HISTORICO_ESCOLAR,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_DOCUMENTOS_ESCOLARES,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_HISTORICO_ESCOLAR,
            Url = ConstantesMenuPermissao.ROTA_RELATORIOS_HISTORICO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
        HE_C = 152,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_FREQUENCIA,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_FREQUENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DF_C = 217,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_FECHAMENTO,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DFE_C = 225,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_INFORMACOES_ESCOLARES,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_INFORMACOES_ESCOLARES,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_INFORMACOES_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DIE_C = 219,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_DIARIO_DE_BORDO,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DB_C = 222,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_DEVOLUTIVAS,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_DEVOLUTIVAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DD_C = 220,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_REGISTRO_INDIVIDUAL,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DRIN_C = 221,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DO_ACOMPANHAMENTO_DE_APRENDIZAGEM,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_RAA,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DAA_C = 223,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_AEE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_AEE,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_AEE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DAEE_C = 215,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_DE_ITINERANCIA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_REGISTRO_DE_ITINERANCIA,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_REGISTRO_ITINERANCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DRI_C = 216,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_NAAPA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_NAAPA,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_NAAPA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DNA_C = 233,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_ESCOLA_AQUI,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_ESCOLA_AQUI,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_ESCOLA_AQUI,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        RDE_A = 181,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GRAFICOS,
            Menu = ConstantesMenuPermissao.MENU_BUSCA_ATIVA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GRAFICOS,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_GRAFICO_BUSCA_ATIVA,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_BUSCA_ATIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        DBA_C = 267,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_CONFIGURACOES,
            Menu = ConstantesMenuPermissao.MENU_REINICIAR_SENHA,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_CONFIGURACAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_REINICIAR_SENHA,
            Url = ConstantesMenuPermissao.ROTA_USUARIOS_REINICIAR_SENHA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhAlteracao = true,
            EhConsulta = true)]
        AS_C = 47,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_CONFIGURACOES,
            Menu = ConstantesMenuPermissao.MENU_SUPORTE,
            OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_CONFIGURACAO,
            OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_SUPORTE,
            Url = ConstantesMenuPermissao.ROTA_MENU_SUPORTE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhInclusao = true,
            EhConsulta = true)]
        US_C = 234,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_MEUS_DADOS,
            Menu = ConstantesMenuPermissao.MENU_MEUS_DADOS,
            Url = ConstantesMenuPermissao.ROTA_MEUS_DADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_EDIT,
            EhConsulta = true)]
        M_C = 48,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_MEUS_DADOS,
            Menu = ConstantesMenuPermissao.MENU_MEUS_DADOS,
            Url = ConstantesMenuPermissao.ROTA_MEUS_DADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_EDIT,
            EhAlteracao = true)]
        M_A = 51,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NOTIFICACAO,
            Menu = ConstantesMenuPermissao.MENU_NOTIFICACAO,
            Url = ConstantesMenuPermissao.ROTA_NOTIFICACOES,
            EhMenu = false,
            EhConsulta = true)]
        N_C = 38,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NOTIFICACAO,
            Menu = ConstantesMenuPermissao.MENU_NOTIFICACAO,
            Url = ConstantesMenuPermissao.ROTA_NOTIFICACOES,
            EhMenu = false,
            EhInclusao = true)]
        N_I = 39,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NOTIFICACAO,
            Menu = ConstantesMenuPermissao.MENU_NOTIFICACAO,
            Url = ConstantesMenuPermissao.ROTA_NOTIFICACOES,
            EhMenu = false,
            EhExclusao = true)]
        N_E = 40,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_NOTIFICACAO,
            Menu = ConstantesMenuPermissao.MENU_NOTIFICACAO,
            Url = ConstantesMenuPermissao.ROTA_NOTIFICACOES,
            EhMenu = false,
            EhAlteracao = true)]
        N_A = 41,



        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhConsulta = true)]
        IE_C = 279,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhInclusao = true)]
        IE_I = 280,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhAlteracao = true)]
        IE_A = 282,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_PAINEL,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhExclusao = true)]
        IE_E = 281,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhConsulta = true)]
        IE_I_P_C = 283,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhInclusao = true)]
        IE_I_P_I = 284,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhAlteracao = true)]
        IE_I_P_A = 285,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO,
          Menu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS,
          OrdemAgrupamento = ConstantesMenuPermissao.ORDEM_AGRUPAMENTO_GESTAO,
          OrdemMenu = ConstantesMenuPermissao.ORDEM_MENU_INFORMACOES_EDUCACIONAIS,
          EhSubMenu = true,
          SubMenu = ConstantesMenuPermissao.MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          OrdemSubMenu = ConstantesMenuPermissao.ORDEM_SUB_MENU_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Url = ConstantesMenuPermissao.ROTA_GESTAO_INFORMACOES_EDUCACIONAIS_IMPORTACOES_DADOS,
          Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
          IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
          EhExclusao = true)]
        IE_I_P_E = 286,
    }
}