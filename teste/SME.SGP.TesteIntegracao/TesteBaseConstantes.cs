using SME.SGP.Dominio;
using System;

namespace SME.SGP.TesteIntegracao
{
    public abstract class TesteBaseConstantes
  {
        public const string ALUNO_CODIGO_1111111 = "1111111";
        public const string ALUNO_CODIGO_2222222 = "2222222";
        public const string ALUNO_CODIGO_3333333 = "3333333";
        public const string ALUNO_CODIGO_4444444 = "4444444";
        
        public const string USUARIO_CHAVE = "NomeUsuario";
        public const string USUARIO_RF_CHAVE = "RF";
        public const string USUARIO_LOGIN_CHAVE = "login";

        public const string USUARIO_LOGADO_CHAVE = "UsuarioLogado";

        public const string USUARIO_CLAIMS_CHAVE = "Claims";

        public const string USUARIO_CLAIM_TIPO_RF = "rf";
        public const string USUARIO_CLAIM_TIPO_PERFIL = "perfil";

        public const string TURMA_CODIGO_1 = "1";

        public const string TURMA_CODIGO_2 = "2";
        public const string TURMA_NOME_2 = "Turma Nome 2";

        public const string TURMA_NOME_1 = "Turma Nome 1";
        public const string TURMA_ANO_2 = "2";

        public const string TURMA_NOME_3 = "Turma Nome 3";
        public const string TURMA_CODIGO_3 = "3";
        public const string TURMA_ANO_3 = "3";
        
        public const string TURMA_NOME_4 = "Turma Nome 4";
        public const string TURMA_CODIGO_4 = "4";
        public const string TURMA_ANO_4 = "4";
        
        public const long TURMA_ID_1 = 1;
        public const long TURMA_ID_2 = 2;
        public const long TURMA_ID_3 = 3;
        public const long TURMA_ID_4 = 4;

        public const long DRE_ID_1 = 1;
        public const long DRE_ID_2 = 2;
        public const long UE_ID_1 = 1;
        public const long UE_ID_2 = 2;
        public const long UE_ID_3 = 3;

        public const long USUARIO_ID_1 = 1;
        public const long USUARIO_ID_2 = 2;

        public int ANO_LETIVO_ANO_ATUAL = DateTimeExtension.HorarioBrasilia().Year;
        public int ANO_LETIVO_ANO_ANTERIOR = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
        public const string NOME_TIPO_CALENDARIO_ANO_ATUAL = "Nome do Tipo Calendário no ano letivo atual";
        public const string NOME_TIPO_CALENDARIO_ANO_ANTERIOR = "Nome do Tipo Calendário no ano letivo anterior";
        
        public const string FALSE = "false";
        public const string TRUE = "true";

        public const int SEMESTRE_0 = 0;
        public const int SEMESTRE_1 = 1;
        public const int SEMESTRE_2 = 2;
        public const long COMPONENTE_CURRICULAR_ARTES_ID_139 = 139;
        public const string COMPONENTE_CURRICULAR_ARTES_NOME = "'Artes'";
        public const string COMPONENTE_CURRICULAR_INGLES_NOME = "'InglêsArtes'";
        
        public const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        public const long COMPONENTE_CURRICULAR_INGLES_ID_9 = 9;
        public const string COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME = "'Língua Portuguesa'";
        public const string COMPONENTE_CURRICULAR_PORTUGUES_NOME = "Língua Portuguesa";
        public const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;
        public const string COMPONENTE_CURRICULAR_DESCONHECIDO_NOME = "Desconhecido";

        public const long COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_ID_1116 = 1116;
        public const string COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME = "'LIBRAS COMPARTILHADA'";

        public const long COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061 = 1061;

        public const long COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060 = 1060;
        public const string COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME = "'INFORMATICA - OIE'";

        public const long COMPONENTE_CURRICULAR_APRENDIZAGEM_E_LEITURA_ID_1359 = 1359;
        public const long COMPONENTE_CURRICULAR_TERRITORIO_SABER_1_ID_1214 = 1214;

        public const string COMPONENTE_CURRICULAR_MATEMATICA_NOME = "'MATEMATICA'";

        public const long COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105 = 1105;
        public const string COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105 = "'Regência de Classe Fund I - 5H'";
        public const string COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105 = "'REG CLASSE CICLO ALFAB / INTERD 5HRS'";

        public const long COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114 = 1114;
        public const string COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114 = "'Regência de Classe EJA - Básica'";
        public const string COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114 = "'REG CLASSE EJA ETAPA BASICA'";

        public const long COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213 = 1213;
        public const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME = "'Regencia Classe SP Integral'";
        public const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL = "'REG CLASSE SP INTEGRAL 1A5 ANOS'";

        public const long COMPONENTE_CURRICULAR_512 = 512;
        public const string COMPONENTE_ED_INF_EMEI_4HS_NOME = "'ED.INF. EMEI 4 HS'";
        public const string COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME = "'Regência de Classe Infantil'";
        public const string COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME = "'REGÊNCIA INFANTIL EMEI 4H'";

        public const long COMPONENTE_CURRICULAR_513 = 513;
        public const string COMPONENTE_ED_INF_EMEI_2HS_NOME = "'ED.INF. EMEI 2 HS'";
        public const string COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME = "'REGÊNCIA INFANTIL EMEI 2H'";

        public const long COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214 = 1214;
        public const string COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME = "'TERRIT SABER / EXP PEDAG 1'";

        public const long COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113 = 1113;
        public const string COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME = "'Regencia Classe EJA ALFAB'";

        public const long COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114 = 1114;
        public const string COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_NOME = "'Regencia Classe EJA Basica'";

        public const long COMPONENTE_CURRICULAR_AULA_COMPARTILHADA = 1123;
        public const string COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME = "'AULA COMPARTILHADA'";

        public const long COMPONENTE_CURRICULAR_AEE_COLABORATIVO = 1103;
        public const string COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME = "'AEE COLABORATIVO'";

        public const long COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO = 1770;
        public const string COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME = "'PAP PROJETO COLABORATIVO'";

        public const long COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM = 1312;
        public const string COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME = "'TECNOLOGIAS DE APRENDIZAGEM'";

        public const string COMPONENTE_CURRICULAR = "componente_curricular";
        public const string COMPONENTE_CURRICULAR_AREA_CONHECIMENTO = "componente_curricular_area_conhecimento";
        public const string AREA_DE_CONHECIMENTO_1 = "'Área de conhecimento 1'";
        public const string AREA_DE_CONHECIMENTO_8 = "'Área de conhecimento 8'";
        public const string AREA_DE_CONHECIMENTO_2 = "'Área de conhecimento 2'";
        public const string AREA_DE_CONHECIMENTO_3 = "'Área de conhecimento 3'";
        public const string AREA_DE_CONHECIMENTO_4 = "'Área de conhecimento 4'";
        public const string AREA_DE_CONHECIMENTO_5 = "'Área de conhecimento 5'";
        public const string AREA_DE_CONHECIMENTO_10 = "'Área de conhecimento 10'";

        public const string CLASSIFICACAO_DOCUMENTO = "classificacao_documento";
        public const string TIPO_DOCUMENTO = "tipo_documento";
        public const string DOCUMENTO_ARQUIVO = "documento_arquivo";

        public const string COMPONENTE_CIENCIAS_ID_89 = "89";
        public const string COMPONENTE_CIENCIAS_NOME = "'Ciências'";

        public const string COMPONENTE_EDUCACAO_FISICA_ID_6 = "6";
        public const string COMPONENTE_EDUCACAO_FISICA_NOME = "'ED. FISICA'";
        public const string COMPONENTE_GEOGRAFIA_ID_8 = "8";
        public const string COMPONENTE_GEOGRAFIA_NOME = "'Geografia'";
        public const string COMPONENTE_HISTORIA_ID_7 = "7";
        public const string COMPONENTE_LINGUA_PORTUGUESA_ID_138 = "138";
        public const string COMPONENTE_MATEMATICA_ID_2 = "2";
        
        public const string COMPONENTE_HISTORIA_NOME = "'História'";
        public const string COMPONENTE_LEITURA_OSL_NOME = "'Leitura OSL'";
        
        public const string COMPONENTE_CURRICULAR_GRUPO_MATRIZ = "componente_curricular_grupo_matriz";
        public const string GRUPO_MATRIZ_1 = "'Grupo matriz 1'";
        public const string GRUPO_MATRIZ_2 = "'Grupo matriz 2'";
        public const string GRUPO_MATRIZ_3 = "'Grupo matriz 3'";
        public const string GRUPO_MATRIZ_4 = "'Grupo matriz 4'";
        public const string GRUPO_MATRIZ_8 = "'Grupo matriz 8'";
        public const string GRUPO_MATRIZ_7 = "'Grupo matriz 7'";

        public const string COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO = "componente_curricular_grupo_area_ordenacao";

        public const string CODIGO_1 = "1";
        public const string CODIGO_2 = "2";
        public const string CODIGO_3 = "3";
        public const string CODIGO_8 = "8";
        public const string CODIGO_4 = "4";
        public const string CODIGO_5 = "5";
        public const string CODIGO_7 = "7";
        public const string CODIGO_10 = "10";
        public const string NULO = "null";
        
        public const int NUMERO_0 = 0;
        public const int NUMERO_1 = 1;
        public const int NUMERO_2 = 2;
        public const int NUMERO_3 = 3;
        public const int RETORNAR_4 = 4;
        
        public const  bool ehPorcentagem = true;
        
        public const long NUMERO_LONGO_1 = 1;
        public const long NUMERO_LONGO_2 = 2;
        public const long NUMERO_LONGO_3 = 3;
        public const long NUMERO_LONGO_4 = 4;
        public const long NUMERO_LONGO_5 = 5;
        
        public const int NUMERO_INTEIRO_0 = 0;
        public const int NUMERO_INTEIRO_1 = 1;
        public const int NUMERO_INTEIRO_2 = 2;
        public const int NUMERO_INTEIRO_3 = 3;
        public const int NUMERO_INTEIRO_4 = 4;
        public const int NUMERO_INTEIRO_5 = 5;
        public const int NUMERO_INTEIRO_15 = 15;
        public const int NUMERO_INTEIRO_16 = 16;
        public const int NUMERO_INTEIRO_19 = 19;
        public const int NUMERO_INTEIRO_20 = 20;
        
        public const long PERIODO_ESCOLAR_CODIGO_1 = 1;
        public const long PERIODO_ESCOLAR_CODIGO_2 = 2;
        public const long PERIODO_ESCOLAR_CODIGO_3 = 3;
        public const long PERIODO_ESCOLAR_CODIGO_4 = 4;

        public const long PERIODO_FECHAMENTO_ID_1 = 1;

        public const string PROVA = "Prova";
        public const string TESTE = "Teste";

        public const string ED_INF_EMEI_4_HS = "'ED.INF. EMEI 4 HS'";
        public const string REGENCIA_CLASSE_INFANTIL = "'Regência de Classe Infantil'";
        public const string REGENCIA_INFATIL_EMEI_4H = "'REGÊNCIA INFANTIL EMEI 4H'";

        public const string UE_CODIGO_1 = "1";
        public const string UE_NOME_1 = "Nome da UE";
        
        public const string UE_CODIGO_2 = "2";
        public const string UE_NOME_2 = "UE 2";
        
        public const string UE_CODIGO_3 = "3";
        public const string UE_NOME_3 = "UE 3";

        public const string DRE_CODIGO_1 = "1";
        public const string DRE_NOME_1 = "DRE 1";
        
        public const string DRE_CODIGO_2 = "2";
        public const string DRE_NOME_2 = "DRE 2";

        public const string SISTEMA_NOME = "Sistema";
        public const string SISTEMA_CODIGO_RF = "1";

        public const string EVENTO_NOME_FESTA = "Festa";

        public const string USUARIO_LOGADO_NOME = "João Usuário";
        public const string USUARIO_LOGADO_RF = "2222222";
        public const string USUARIO_ADMIN_RF = "9999999";

        public const string USUARIO_LOGIN_CP = "CP999999";
        public const string USUARIO_LOGIN_DIRETOR = "DIR999998";
        public const string USUARIO_LOGIN_AD = "AD999997";
        public const string USUARIO_LOGIN_COOD_NAAPA = "NAAP11111";
        public const string USUARIO_LOGIN_ADM_DRE = "DRE111111";
        public const string USUARIO_LOGIN_ADM_SME = "SME111111";
        public const string USUARIO_LOGIN_PAP = "PAP111111";        

        public const string USUARIO_CP_LOGIN_3333333 = "3333333";
        public const string USUARIO_CEFAI_LOGIN_3333333 = "3333333";
        public const string USUARIO_PAAI_LOGIN_3333333 = "3333333";
        public const string USUARIO_LOGIN_PAAI = "4444444";
        public const string USUARIO_PAAI_LOGIN_5555555 = "5555555";
        public const string USUARIO_PAEE_LOGIN_5555555 = "5555555";
        public const string USUARIO_CP_CODIGO_RF_3333333 = "3333333";
        public const string USUARIO_CP_NOME_3333333 = "Nome do usuario 3333333";

        public const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        public const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        public const string USUARIO_PROFESSOR_NOME_2222222 = "Nome do usuario 2222222";

        public const long USUARIO_RF_1111111_ID_2 = 2;
        public const string USUARIO_PROFESSOR_LOGIN_1111111 = "1111111";
        public const string USUARIO_PROFESSOR_CODIGO_RF_1111111 = "1111111";
        public const string USUARIO_PROFESSOR_NOME_1111111 = "Nome do usuário 1111111";

        public const string PROFESSOR = "Professor";
        public const int ORDEM_290 = 290;

        public const string PROFESSOR_CJ = "Professor CJ";
        public const int ORDEM_320 = 320;

        public const string CP = "CP";
        public const int ORDEM_240 = 240;

        public const int BIMESTRE_1 = 1;
        public const int BIMESTRE_2 = 2;
        public const int BIMESTRE_3 = 3;
        public const int BIMESTRE_4 = 4;
        public const int BIMESTRE_FINAL = 0;

        public const string EVENTO_NAO_LETIVO = "Evento não letivo";
        public const long TIPO_EVENTO_21 = 21;
        public const long TIPO_EVENTO_1 = 1;
        public const long TIPO_EVENTO_2 = 2;
        public const long TIPO_EVENTO_13 = 13;
        public const long TIPO_EVENTO_14 = 14;
        public const string SUSPENSAO_DE_ATIVIDADES = "Suspensão de Atividades";
        public const string REPOSICAO_AULA = "Reposição de Aula";
        public const string REPOSICAO_DIA = "Reposição Dia";
        public const string REPOSICAO_AULA_DE_GREVE = "Reposição de Aula de Greve";
        public const string LIBERACAO_EXCEPCIONAL = "Liberação excepcional";
        public const int TIPO_CALENDARIO_ID = 1;

        public DateTime DATA_01_01_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        public DateTime DATA_01_05_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 01);
        public DateTime DATA_29_04_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 29);
        public DateTime DATA_02_05_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public DateTime DATA_24_07_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 24);
        public DateTime DATA_25_07_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        public DateTime DATA_02_10_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 02);
        public DateTime DATA_03_10_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        public DateTime DATA_22_12_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);
        
        public DateTime DATA_03_01_INICIO_BIMESTRE_1_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 03);
        public DateTime DATA_28_04_FIM_BIMESTRE_1_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 04, 28);
        public DateTime DATA_02_05_INICIO_BIMESTRE_2_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 05, 02);
        public DateTime DATA_08_07_FIM_BIMESTRE_2_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 07, 08);
        public DateTime DATA_25_07_INICIO_BIMESTRE_3_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 07, 25);
        public DateTime DATA_30_09_FIM_BIMESTRE_3_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 09, 30);
        public DateTime DATA_03_10_INICIO_BIMESTRE_4_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 10, 03);
        public DateTime DATA_22_12_FIM_BIMESTRE_4_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 22);        

        public const long TIPO_CALENDARIO_1 = 1;
        public const long TIPO_CALENDARIO_2 = 2;
        public const long TIPO_CALENDARIO_3 = 3;
        public const long TIPO_CALENDARIO_4 = 4;
        public const long TIPO_CALENDARIO_5 = 5;
        public const long TIPO_CALENDARIO_6 = 6;
        public const long TIPO_CALENDARIO_7 = 7;
        public const long TIPO_CALENDARIO_8 = 8;
        public const long TIPO_CALENDARIO_9 = 9;
        public const long TIPO_CALENDARIO_10 = 10;

        public string DATA_INICIO_SGP = "DataInicioSGP";
        public string NUMERO_50 = "50";
        public string NUMERO_5 = "5";
        public string PERCENTUAL_ALUNOS_INSUFICIENTES = "PERCENTUAL_ALUNOS_INSUFICIENTES";
        public string MEDIA_BIMESTRAL = "MEDIA_BIMESTRAL";

        public DateTime DATA_03_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 03);
        public DateTime DATA_28_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 28);

        public const int NUMERO_AULA_1 = 1;
        public const int NUMERO_AULA_2 = 2;
        public const int NUMERO_AULA_3 = 3;
        public const int NUMERO_AULA_4 = 4;
        
        public const int AULA_ID_1 = 1;
        public const int AULA_ID_2 = 2;
        public const int AULA_ID_3 = 3;
        public const int AULA_ID_4 = 4;
        public const int AULA_ID_5 = 5;
        public const int AULA_ID_6 = 6;
        public const int AULA_ID_7 = 7;
        public const int AULA_ID_8 = 8;
        public const int AULA_ID_9 = 9;
        public const int AULA_ID_10 = 10;
        public const int AULA_ID_11 = 11;
        public const int AULA_ID_12 = 12;
        public const int AULA_ID_13 = 13;
        public const int AULA_ID_14 = 14;
        public const int AULA_ID_15 = 15;
        public const int AULA_ID_16 = 16;
        public const int AULA_ID_17 = 17;
        public const int AULA_ID_18 = 18;
        public const int AULA_ID_19 = 19;
        public const int AULA_ID_20 = 20;

        public const long REGISTRO_FREQUENCIA_1 = 1;
        public const long REGISTRO_FREQUENCIA_2 = 2;

        public const string ALFABETIZACAO = "ALFABETIZACAO";
        public const string INTERDISCIPLINAR = "INTERDISCIPLINAR";
        public const string AUTORAL = "AUTORAL";
        public const string MEDIO = "MEDIO";
        public const string EJA_ALFABETIZACAO = "EJA_ALFABETIZACAO";
        public const string EJA_BASICA = "EJA_BASICA";
        public const string EJA_COMPLEMENTAR = "EJA_COMPLEMENTAR";
        public const string EJA_FINAL = "EJA_FINAL";

        public const string ANO_1 = "1";
        public const string ANO_2 = "2";
        public const string ANO_3 = "3";
        public const string ANO_4 = "4";
        public const string ANO_5 = "5";
        public const string ANO_6 = "6";
        public const string ANO_7 = "7";
        public const string ANO_8 = "8";
        public const string ANO_9 = "9";

        public const long ATIVIDADE_AVALIATIVA_1 = 1;
        public const long ATIVIDADE_AVALIATIVA_2 = 2;

        public readonly DateTime DATA_04_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 04);

        public readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public readonly DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        public readonly DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 07);

        public readonly DateTime DATA_20_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 20);
        public readonly DateTime DATA_21_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 21);
        public readonly DateTime DATA_22_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 22);
        public readonly DateTime DATA_23_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 23);
        public readonly DateTime DATA_24_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 24);

        public readonly DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        public readonly DateTime DATA_26_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 26);
        public readonly DateTime DATA_27_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 27);
        public readonly DateTime DATA_28_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 28);
        public readonly DateTime DATA_29_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 29);
        public readonly DateTime DATA_30_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 30);
        public readonly DateTime DATA_31_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 31);
        public readonly DateTime DATA_01_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 01);
        public readonly DateTime DATA_02_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 02);
        public readonly DateTime DATA_03_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 03);
        public readonly DateTime DATA_04_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 04);
        public readonly DateTime DATA_05_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 05);
        public readonly DateTime DATA_06_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 06);

        public readonly DateTime DATA_16_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 16);
        public readonly DateTime DATA_30_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);

        public readonly DateTime DATA_01_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 01);
        public readonly DateTime DATA_03_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        public readonly DateTime DATA_22_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        public readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        public readonly DateTime DATA_01_01_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01);

        public readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);
        public readonly DateTime DATA_31_12_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 31);

        public readonly DateTime DATA_10_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 10);
        
        public readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        public const int AULA_ID = 1;
        public const int QUANTIDADE_AULA_NORMAL_MAIS_RECORRENTES_3 = 3;
        public const int QUANTIDADE_AULA_RECORRENTE_2 = 2;
        public const int QUANTIDADE_AULA = 1;
        public const int QUANTIDADE_AULA_2 = 2;
        public const int QUANTIDADE_AULA_3 = 3;
        public const int QUANTIDADE_AULA_4 = 4;
        public const string CODIGO_ALUNO_1 = "1";
        public const string CODIGO_ALUNO_2 = "2";
        public const string CODIGO_ALUNO_3 = "3";
        public const string CODIGO_ALUNO_4 = "4";
        public const string CODIGO_ALUNO_5 = "5";
        public const string CODIGO_ALUNO_6 = "6";
        public const string CODIGO_ALUNO_7 = "7";
        public const string CODIGO_ALUNO_8 = "8";
        public const string CODIGO_ALUNO_9 = "9";
        public const string CODIGO_ALUNO_10 = "10";
        public const string CODIGO_ALUNO_11 = "11";
        public const string CODIGO_ALUNO_12 = "12";
        public const string CODIGO_ALUNO_13 = "13";
        public const string CODIGO_ALUNO_14 = "14";
        public const string CODIGO_ALUNO_15 = "15";
        public const int TOTAL_AUSENCIAS_1 = 1;
        public const int TOTAL_AUSENCIAS_3 = 3;
        public const int TOTAL_AUSENCIAS_7 = 7;
        public const int TOTAL_AUSENCIAS_8 = 8;
        public const int TOTAL_COMPENSACOES_1 = 1;
        public const int TOTAL_COMPENSACOES_3 = 3;
        public const int TOTAL_COMPENSACOES_7 = 7;
        public const int TOTAL_COMPENSACOES_8 = 8;        
        public const int TOTAL_PRESENCAS_1 = 1;
        public const int TOTAL_PRESENCAS_2 = 2;
        public const int TOTAL_PRESENCAS_3 = 3;
        public const int TOTAL_PRESENCAS_4 = 4;
        public const int TOTAL_REMOTOS_0 = 0;
        
        public const int COMPENSACAO_AUSENCIA_ID_1 = 1;

        public DateTime DATA_01_02_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        public DateTime DATA_25_04_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 25);
        public const string REABERTURA_GERAL = "Reabrir Geral";
        public DateTime DATA_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public DateTime DATA_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        public DateTime DATA_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        public DateTime DATA_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        public DateTime DATA_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        public DateTime DATA_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);
        public DateTime DATA_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        public DateTime DATA_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        public const long ATESTADO_MEDICO_DO_ALUNO_1 = 1;
        public const long ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2 = 2;
        public const long DOENCA_NA_FAMILIA_SEM_ATESTADO_3 = 3;
        public const long OBITO_DE_PESSOA_DA_FAMILIA_4 = 4;
        public const long INEXISTENCIA_DE_PESSOA_PARA_LEVAR_A_ESCOLA_5 = 5;
        public const long ENCHENTE_6 = 6;
        public const long FALTA_DE_TRANSPORTE_7 = 7;
        public const long VIOLENCIA_NA_AREA_ONDE_MORA_8 = 8;
        public const long CALAMIDADE_PUBLICA_QUE_ATINGIU_A_ESCOLA_OU_EXIGIU_O_USO_DO_ESPAÇO_COMO_ABRIGAMENTO_9 = 9;
        public const long ESCOLA_FECHADA_POR_SITUACAO_DE_VIOLENCIA_10 = 10;

        public const string DESCRICAO_FREQUENCIA_ALUNO_1 = "Lorem Ipsum";
        public const string DESCRICAO_FREQUENCIA_ALUNO_2 = "é um texto bastante conhecido";

        public const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_NOME = "PercentualAlunosInsuficientes";
        public const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_VALOR_50 = "50";
        public const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_DESCRICAO = "Percentual de alunos com nota/conceito insuficientes para exigência de justificativ";


        public const string ALUNO_CODIGO_1 = "1";
        public const string ALUNO_NOME_1 = "Nome do Aluno 1";
        public const string ALUNO_CODIGO_2 = "2";
        public const string ALUNO_CODIGO_3 = "3";
        public const string ALUNO_CODIGO_4 = "4";
        public const string ALUNO_CODIGO_5 = "5";
        public const string ALUNO_CODIGO_6 = "6";
        public const string ALUNO_CODIGO_7 = "7";
        public const string ALUNO_CODIGO_8 = "8";
        public const string ALUNO_CODIGO_9 = "9";
        public const string ALUNO_CODIGO_10 = "10";
        public const string ALUNO_CODIGO_11 = "11";
        public const string ALUNO_CODIGO_12 = "12";
        public const string ALUNO_CODIGO_13 = "13";
        

        public const double NOTA_1 = 1;
        public const double NOTA_2 = 2;
        public const double NOTA_3 = 3;
        public const double NOTA_4 = 4;
        public const double NOTA_5 = 5;
        public const double NOTA_6 = 6;
        public const double NOTA_7 = 7;
        public const double NOTA_8 = 8;
        public const double NOTA_9 = 9;
        public const double NOTA_10 = 10;

        public const string PLENAMENTE_SATISFATORIO = "P";
        public const int PLENAMENTE_SATISFATORIO_ID_1 = 1;
        public const string SATISFATORIO = "S";
        public const int SATISFATORIO_ID_2 = 2;
        public const string NAO_SATISFATORIO = "NS";
        public const int NAO_SATISFATORIO_ID_3 = 3;
        
        public const string NOTA = "NOTA";
        public const string CONCEITO = "CONCEITO";

        public const string PERCENTUAL_FREQUENCIA_CRITICO_NOME = "PercentualFrequenciaCritico";
        public const string PERCENTUAL_FREQUENCIA_CRITICO_DESCRICAO = "Percentual de frequência para definir aluno em situação crítica";
        public const string NUMERO_PAGINA = "NumeroPagina";
        public const string NUMERO_REGISTROS = "NumeroRegistros";
        public const string ADMINISTRADOR = "Administrador";
        public const string NOME_ADMINISTRADOR = "NomeAdministrador";
    }
}
