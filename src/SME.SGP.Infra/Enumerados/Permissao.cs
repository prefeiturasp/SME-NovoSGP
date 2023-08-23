using SME.SGP.Infra.Constantes;

namespace SME.SGP.Infra
{
    public enum Permissao
    {
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhConsulta = true)]
        PDC_C = 34,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 1, 
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        PDC_I = 35,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, 
            EhExclusao = true)]
        PDC_E = 36,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_DE_CICLO, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_CICLO, 
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhAlteracao = true)]
        PDC_A = 37,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = 1, 
            OrdemMenu = 2, 
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, 
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_MINUS,  
            EhConsulta = true)]
        PA_C = 26,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL,
            OrdemAgrupamento = 1, 
            OrdemMenu = 2, 
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, 
            EhInclusao = true)]
        PA_I = 27,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)] 
        PA_E = 28,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_PLANO_ANUAL, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL,   
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhAlteracao = true)]
        PA_A = 29,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = 1, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT, 
            EhConsulta = true)]
        PT_C = 132,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER,
            OrdemAgrupamento = 1, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhInclusao = true)]
        PT_I = 133,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        PT_E = 134,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_TERRITORIO_DO_SABER, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 3, 
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_PLANO_ANUAL_TERRITORIO_SABER,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhAlteracao = true)]
        PT_A = 135,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = 1, 
            OrdemMenu = 4, 
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_ENVELOPE_OPEN,
            EhConsulta = true)]
        CI_C = 163,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = 1, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhInclusao = true)]
        CI_I = 164,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = 1, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhExclusao = true)]
        CI_E = 165,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_CARTA_DE_INTENCOES,
            OrdemAgrupamento = 1, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_PLANEJAMENTO_CARTA_DE_INTENCOES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhAlteracao = true)]
        CI_A = 166,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = 1, 
            OrdemMenu = 5, 
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhInclusao = true)]
        RPOA_I = 108,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 5,
            Url =ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,  
            EhAlteracao = true)]
        RPOA_A = 109,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA, 
            OrdemAgrupamento = 1, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhConsulta = true)]
        RPOA_C = 110,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_PLANEJAMENTO,
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_POA,
            OrdemAgrupamento = 1, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_POA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_LIST_ALT,
            EhExclusao = true)]
        RPOA_E = 111,
          
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 1, 
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,
            EhConsulta = true)]
        L_C = 228,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 1, 
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhInclusao = true)]
        L_I = 229,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 1, 
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhExclusao = true)]
        L_E = 230,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_LISTAO,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 1, 
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_LISTAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,
            EhAlteracao = true)]
        L_A = 231,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_AULA_PREVISTA_X_AULA_DADA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,  
            EhInclusao = true)]
        ADAP_I = 104,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_AULA_PREVISTA_X_AULA_DADA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",  
            EhAlteracao = true)]
        ADAP_A = 105,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_AULA_PREVISTA_X_AULA_DADA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",  
            EhConsulta = true)]
        ADAP_C = 106,  
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_AULA_PREVISTA_X_AULA_DADA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_AULA_DADA_AULA_PREVISTA,
            Icone = "",  
            EhExclusao = true)]
        ADAP_E = 107,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 3, 
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        CP_C = 60,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 3, 
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhInclusao = true)]
        CP_I = 61,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 3, 
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhExclusao = true)]
        CP_E = 62,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE,
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_DO_PROFESSOR,   
            OrdemAgrupamento = 2, 
            OrdemMenu = 3, 
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_CALENDARIO_PROFESSOR,
            Icone = ConstantesMenuPermissao.ICONE_FA_CALENDAR_ALT,
            EhAlteracao = true)]
        CP_A = 63, 
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,  
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_CHECK_SQUARE,  
            EhConsulta = true)]
        PDA_C = 30,
                
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,  
            EhInclusao = true)]
        PDA_I = 31,                
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,  
            EhExclusao = true)]
        PDA_E = 32,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_FREQUENCIA_PLANO_AULA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_FREQUENCIA_PLANO_AULA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_BOOK_READER,  
            EhAlteracao = true)]
        PDA_A = 33,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_NOTAS, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,  
            EhConsulta = true)]
        NC_C = 22,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_NOTAS, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,  
            EhInclusao = true)]
        NC_I = 23,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_NOTAS, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,  
            EhExclusao = true)]
        NC_E = 24, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_NOTAS, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_NOTAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,  
            EhAlteracao = true)]
        NC_A = 25,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",  
            EhConsulta = true)]
        CA_C = 112,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",  
            EhInclusao = true)]
        CA_I = 113, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",  
            EhExclusao = true)]
        CA_E = 114,               
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_COMPENSACAO_AUSENCIA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_COMPENSACAO_AUSENCIA,
            Icone = "",  
            EhAlteracao = true)]
        CA_A = 115,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,  
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAR_FA_FILE_ALT,  
            EhConsulta = true)]
        DDB_C = 159,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        DDB_I = 160,               
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        DDB_E = 161,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_DIARIO_BORDO, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DIARIO_BORDO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        DDB_A = 162,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhConsulta = true)]
        REI_C = 189,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
        REI_I = 190,
                
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
        REI_E = 191,   

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_REGISTRO_INDIVIDUAL, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_REGISTRO_INDIVIDUAL,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
        REI_A = 192,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ACJ_C = 18,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
        ACJ_I = 19,                
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
        ACJ_E = 20,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_CJ, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
        ACJ_A = 21,  
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DE_FREQUENCIA, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 10,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_ACOMPANHAMENTO_FREQUENCIA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhConsulta = true)]
        AFQ_C = 201, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_RELATORIO_DE_PAP, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 11,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_CJS_EDITAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
            RPAP_C = 246,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_DIARIO_DE_CLASSE, 
            Menu = ConstantesMenuPermissao.MENU_SONDAGEM, 
            OrdemAgrupamento = 2, 
            OrdemMenu = 12,
            Url = ConstantesMenuPermissao.ROTA_SONDAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PRINT,
            EhConsulta = true)]
            S_C = 5, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DE_BIMESTRE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
            FB_C = 124,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DE_BIMESTRE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhInclusao = true)]
            FB_I = 125,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DE_BIMESTRE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhExclusao = true)]
            FB_E = 126,                
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_FECHAMENTO_DE_BIMESTRE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_FECHAMENTO_BIMESTRE,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhAlteracao = true)]
            FB_A = 127,     
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhConsulta = true)]
            CC_C = 136,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhInclusao = true)]
            CC_I = 137,
     
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhExclusao = true)]
            CC_E = 138,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_CONSELHO_DE_CLASSE, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_CONSELHO_CLASSE,
            Icone = "",
            EhAlteracao = true)]
            CC_A = 139, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_ACOMPANHAMENTO_DO_FECHAMENTO, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_ACOMPANHAMENTO_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_PENCIL_RULER,
            EhConsulta = true)]
        ACF_C = 218,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
        PF_C = 128, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhInclusao = true)]
        PF_I = 129,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhExclusao = true)]
        PF_E = 130,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_PENDENCIAS_DO_FECHAMENTO, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_FECHAMENTO_PENDENCIAS_FECHAMENTO,
            Icone = "",
            EhAlteracao = true)]
        PF_A = 131,   
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_FECHAMENTO, 
            Menu = ConstantesMenuPermissao.MENU_RAA, 
            OrdemAgrupamento = 3, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_DASHBOARD_ACOMPANHAMENTO_APRENDIZAGEM,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CHART_BAR,
            EhConsulta = true)]
            DAA_C = 223, 
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhConsulta = true)]
            DE_C = 167,        
         
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhInclusao = true)]
            DE_I = 168,         
         
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhExclusao = true)]
            DE_E = 169,   
          
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DEVOLUTIVAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 1,
            Url = ConstantesMenuPermissao.ROTA_DIARIO_CLASSE_DEVOLUTIVA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_FILE_ALT,
            EhAlteracao = true)]
            DE_A = 170,
 
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhConsulta = true)]
            C_C = 10,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhInclusao = true)]
            C_I = 11,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhExclusao = true)]
            C_E = 12,        
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 2,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAR_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            C_A = 13,           
 
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
            DPU_C = 177,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
            DPU_I = 178,       
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
            DPU_E = 179,  

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_DOCUMENTOS_E_PLANOS_DE_TRABALHO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 3,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_DOCUMENTOS_PLANOS_TRABALHO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
            DPU_A = 180,  
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            PE_C = 68,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            PE_I = 69,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            PE_E = 70,          
 
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_BIMESTRES, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 4,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODOS_ESCOLARES,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            PE_A = 71,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            PFA_C = 72,  
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            PFA_I = 73,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            PFA_E = 74,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_ABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 5,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_ABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            PFA_A = 75,
 
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            PFR_C = 76,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            PFR_I = 77,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            PFR_E = 78,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_PERIODOS_ESCOLARES_REABERTURA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 6,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_PERIODO_FECHAMENTO_REABERTURA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            PFR_A = 79,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_EVENTOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            IconeDashBoard = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_CHECK,
            EhConsulta = true)]
            E_C = 88,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_EVENTOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            E_I = 89,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_EVENTOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            E_E = 90,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_EVENTOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 7,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            E_A = 91,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            TE_C = 84,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            TE_I = 85,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            TE_E = 86,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_EVENTO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 8,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_EVENTOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            TE_A = 87,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            TF_C = 80,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            TF_I = 81,
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            TF_E = 82,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_FERIADO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 9,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_FERIADO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            TF_A = 83,
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 10,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhConsulta = true)]
            TCE_C = 64,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 10,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhInclusao = true)]
            TCE_I = 65,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 10,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhExclusao = true)]
            TCE_E = 66,
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_CALENDARIO_ESCOLAR, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 10,
            Url = ConstantesMenuPermissao.ROTA_CALENDARIO_ESCOLAR_TIPO_CALENDARIO_ESCOLAR,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_CALENDAR_ALT,
            EhAlteracao = true)]
            TCE_A = 67,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_AVALIACAO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 11,
            Url = ConstantesMenuPermissao.ROTA_CONFIGURACOES_TIPO_AVALIACAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhConsulta = true)]
            TA_C = 100,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_AVALIACAO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 11,
            Url = ConstantesMenuPermissao.ROTA_CONFIGURACOES_TIPO_AVALIACAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhInclusao = true)]
            TA_I = 101,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_AVALIACAO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 11,
            Url = ConstantesMenuPermissao.ROTA_CONFIGURACOES_TIPO_AVALIACAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhExclusao = true)]
            TA_E = 102,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_TIPO_DE_AVALIACAO, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 11,
            Url = ConstantesMenuPermissao.ROTA_CONFIGURACOES_TIPO_AVALIACAO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_USER_COG,
            EhAlteracao = true)]
            TA_A = 103,
           
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 12,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
            ARP_C = 96,
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 12,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
            ARP_I = 97,      

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 12,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
            ARP_E = 98,  

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_DE_RESPONSAVEIS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 12,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_RESPONSAVEIS_LISTA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
            ARP_A = 99,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 13,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
            AE_C = 92,
        
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 13,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
            AE_I = 93,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 13,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
            AE_E = 94,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_ATRIBUICAO_ESPORADICA, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 13,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_ATRIBUICAO_ESPORADICA,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
            AE_A = 95,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 14,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
            CO_C = 140,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 14,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
            CO_E = 142,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 14,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS_NOVO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
            CO_A = 143,

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_COMUNICADOS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 14,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_COMUNICADOS_NOVO,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
            CO_I = 141,
            
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 15,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhConsulta = true)]
            OCO_C = 193,
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 15,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhInclusao = true)]
            OCO_I = 194,        

        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 15,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhExclusao = true)]
            OCO_E = 195,   
             
        [PermissaoMenu(Agrupamento = ConstantesMenuPermissao.AGRUPAMENTO_GESTAO, 
            Menu = ConstantesMenuPermissao.MENU_OCORRENCIAS, 
            OrdemAgrupamento = 4, 
            OrdemMenu = 15,
            Url = ConstantesMenuPermissao.ROTA_GESTAO_OCORRENCIAS,
            Icone = ConstantesMenuPermissao.ICONE_FAS_FA_TASKS,
            EhAlteracao = true)]
            OCO_A = 196,          

             
    }
}