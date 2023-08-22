using SME.SGP.Infra.Constantes;

namespace SME.SGP.Infra
{
    public enum Permissao
    {
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_MINUS, 
        Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, OrdemAgrupamento = 1, OrdemMenu = 1, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL, EhConsulta = true)]
        PA_C = 26,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 1, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL, EhInclusao = true)]
        PA_I = 27,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 1, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL, EhExclusao = true)] 
        PA_E = 28,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 1, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL, EhAlteracao = true)]
        PA_A = 29,
        
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
        OrdemAgrupamento = 1, OrdemMenu = 2, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO, EhConsulta = true)]
        PDC_C = 34,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 2, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO, EhInclusao = true)]
        PDC_I = 35,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 2, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO, EhExclusao = true)]
        PDC_E = 36,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 2, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO, EhAlteracao = true)]
        PDC_A = 37,
        
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_ENVELOPE_OPEN, 
        Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, OrdemAgrupamento = 1, OrdemMenu = 3, EhConsulta = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES)]
        CI_C = 163,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 3, EhInclusao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES)]
        CI_I = 164,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 3, EhExclusao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES)]
        CI_E = 165,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 3, EhAlteracao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES)]
        CI_A = 166,
        
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 4, EhConsulta = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER)]
        PT_C = 132,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 4, EhInclusao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER)]
        PT_I = 133,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 4, EhExclusao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER)]
        PT_E = 134,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 4, EhAlteracao = true, Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER)]
        PT_A = 135,
        
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 5, EhInclusao = true, Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA)]
        RPOA_I = 108,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 5, EhAlteracao = true, Url =ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA)]
        RPOA_A = 109,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 5, EhConsulta = true, Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA)]
        RPOA_C = 110,
        [PermissaoMenu(Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
        OrdemAgrupamento = 1, OrdemMenu = 5, EhExclusao = true, Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA)]
        RPOA_E = 111,
    }
}