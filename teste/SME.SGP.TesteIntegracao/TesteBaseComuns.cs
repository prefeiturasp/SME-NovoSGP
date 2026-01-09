using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using Sentry.Protocol;
using Nest;
using System.Linq;

namespace SME.SGP.TesteIntegracao
{
    public abstract class TesteBaseComuns : TesteBase
    {
        protected const string ALUNO_CODIGO_1111111 = "1111111";
        protected const string ALUNO_CODIGO_2222222 = "2222222";
        protected const string ALUNO_CODIGO_3333333 = "3333333";
        protected const string ALUNO_CODIGO_4444444 = "4444444";
        
        private const string USUARIO_CHAVE = "NomeUsuario";
        private const string USUARIO_RF_CHAVE = "RF";
        private const string USUARIO_LOGIN_CHAVE = "login";

        private const string USUARIO_LOGADO_CHAVE = "UsuarioLogado";

        private const string USUARIO_CLAIMS_CHAVE = "Claims";

        private const string USUARIO_CLAIM_TIPO_RF = "rf";
        private const string USUARIO_CLAIM_TIPO_PERFIL = "perfil";

        protected const string TURMA_CODIGO_1 = "1";

        protected const string TURMA_CODIGO_2 = "2";
        protected const string TURMA_NOME_2 = "Turma Nome 2";

        protected const string TURMA_NOME_1 = "Turma Nome 1";
        protected const string TURMA_ANO_2 = "2";

        protected const string TURMA_NOME_3 = "Turma Nome 3";
        protected const string TURMA_CODIGO_3 = "3";
        protected const string TURMA_ANO_3 = "3";
        
        protected const string TURMA_NOME_4 = "Turma Nome 4";
        protected const string TURMA_CODIGO_4 = "4";
        protected const string TURMA_ANO_4 = "4";
        
        protected const long TURMA_ID_1 = 1;
        protected const long TURMA_ID_2 = 2;
        protected const long TURMA_ID_3 = 3;
        protected const long TURMA_ID_4 = 4;

        protected const long DRE_ID_1 = 1;
        protected const long DRE_ID_2 = 2;
        protected const long UE_ID_1 = 1;
        protected const long UE_ID_2 = 2;
        protected const long UE_ID_3 = 3;

        protected const long USUARIO_ID_1 = 1;
        protected const long USUARIO_ID_2 = 2;

        protected int ANO_LETIVO_ANO_ATUAL = DateTimeExtension.HorarioBrasilia().Year;
        protected int ANO_LETIVO_ANO_ANTERIOR = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
        protected const string NOME_TIPO_CALENDARIO_ANO_ATUAL = "Nome do Tipo Calendário no ano letivo atual";
        protected const string NOME_TIPO_CALENDARIO_ANO_ANTERIOR = "Nome do Tipo Calendário no ano letivo anterior";
        
        protected const string FALSE = "false";
        protected const string TRUE = "true";

        protected const int SEMESTRE_0 = 0;
        protected const int SEMESTRE_1 = 1;
        protected const int SEMESTRE_2 = 2;
        protected const long COMPONENTE_CURRICULAR_ARTES_ID_139 = 139;
        protected const string COMPONENTE_CURRICULAR_ARTES_NOME = "'Artes'";
        protected const string COMPONENTE_CURRICULAR_INGLES_NOME = "'InglêsArtes'";
        
        protected const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        protected const long COMPONENTE_CURRICULAR_INGLES_ID_9 = 9;
        protected const string COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME = "'Língua Portuguesa'";
        protected const string COMPONENTE_CURRICULAR_PORTUGUES_NOME = "Língua Portuguesa";
        protected const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;
        protected const string COMPONENTE_CURRICULAR_DESCONHECIDO_NOME = "Desconhecido";

        protected const long COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_ID_1116 = 1116;
        protected const string COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME = "'LIBRAS COMPARTILHADA'";

        protected const long COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061 = 1061;

        protected const long COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060 = 1060;
        protected const string COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME = "'INFORMATICA - OIE'";

        protected const long COMPONENTE_CURRICULAR_APRENDIZAGEM_E_LEITURA_ID_1359 = 1359;
        protected const long COMPONENTE_CURRICULAR_TERRITORIO_SABER_1_ID_1214 = 1214;

        protected const string COMPONENTE_CURRICULAR_MATEMATICA_NOME = "'MATEMATICA'";

        protected const long COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105 = 1105;
        protected const string COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105 = "'Regência de Classe Fund I - 5H'";
        protected const string COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105 = "'REG CLASSE CICLO ALFAB / INTERD 5HRS'";

        protected const long COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114 = 1114;
        protected const string COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114 = "'Regência de Classe EJA - Básica'";
        protected const string COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114 = "'REG CLASSE EJA ETAPA BASICA'";

        protected const long COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213 = 1213;
        protected const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME = "'Regencia Classe SP Integral'";
        protected const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL = "'REG CLASSE SP INTEGRAL 1A5 ANOS'";

        protected const long COMPONENTE_CURRICULAR_512 = 512;
        protected const string COMPONENTE_ED_INF_EMEI_4HS_NOME = "'ED.INF. EMEI 4 HS'";
        protected const string COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME = "'Regência de Classe Infantil'";
        protected const string COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME = "'REGÊNCIA INFANTIL EMEI 4H'";

        protected const long COMPONENTE_CURRICULAR_513 = 513;
        protected const string COMPONENTE_ED_INF_EMEI_2HS_NOME = "'ED.INF. EMEI 2 HS'";
        protected const string COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME = "'REGÊNCIA INFANTIL EMEI 2H'";

        protected const long COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214 = 1214;
        protected const string COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME = "'TERRIT SABER / EXP PEDAG 1'";

        protected const long COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113 = 1113;
        protected const string COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME = "'Regencia Classe EJA ALFAB'";

        protected const long COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_ID_1114 = 1114;
        protected const string COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_NOME = "'Regencia Classe EJA Basica'";

        protected const long COMPONENTE_CURRICULAR_AULA_COMPARTILHADA = 1123;
        protected const string COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME = "'AULA COMPARTILHADA'";

        protected const long COMPONENTE_CURRICULAR_AEE_COLABORATIVO = 1103;
        protected const string COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME = "'AEE COLABORATIVO'";

        protected const long COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO = 1770;
        protected const string COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME = "'PAP PROJETO COLABORATIVO'";

        protected const long COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM = 1312;
        protected const string COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME = "'TECNOLOGIAS DE APRENDIZAGEM'";

        private const string COMPONENTE_CURRICULAR = "componente_curricular";
        private const string COMPONENTE_CURRICULAR_AREA_CONHECIMENTO = "componente_curricular_area_conhecimento";
        private const string AREA_DE_CONHECIMENTO_1 = "'Área de conhecimento 1'";
        private const string AREA_DE_CONHECIMENTO_8 = "'Área de conhecimento 8'";
        private const string AREA_DE_CONHECIMENTO_2 = "'Área de conhecimento 2'";
        private const string AREA_DE_CONHECIMENTO_3 = "'Área de conhecimento 3'";
        private const string AREA_DE_CONHECIMENTO_4 = "'Área de conhecimento 4'";
        private const string AREA_DE_CONHECIMENTO_5 = "'Área de conhecimento 5'";
        private const string AREA_DE_CONHECIMENTO_10 = "'Área de conhecimento 10'";

        protected const string CLASSIFICACAO_DOCUMENTO = "classificacao_documento";
        protected const string TIPO_DOCUMENTO = "tipo_documento";
        protected const string DOCUMENTO_ARQUIVO = "documento_arquivo";

        protected const string COMPONENTE_CIENCIAS_ID_89 = "89";
        protected const string COMPONENTE_CIENCIAS_NOME = "'Ciências'";

        protected const string COMPONENTE_EDUCACAO_FISICA_ID_6 = "6";
        protected const string COMPONENTE_EDUCACAO_FISICA_NOME = "'ED. FISICA'";
        protected const string COMPONENTE_GEOGRAFIA_ID_8 = "8";
        protected const string COMPONENTE_GEOGRAFIA_NOME = "'Geografia'";
        protected const string COMPONENTE_HISTORIA_ID_7 = "7";
        protected const string COMPONENTE_LINGUA_PORTUGUESA_ID_138 = "138";
        protected const string COMPONENTE_MATEMATICA_ID_2 = "2";
        
        protected const string COMPONENTE_HISTORIA_NOME = "'História'";
        protected const string COMPONENTE_LEITURA_OSL_NOME = "'Leitura OSL'";
        
        private const string COMPONENTE_CURRICULAR_GRUPO_MATRIZ = "componente_curricular_grupo_matriz";
        private const string GRUPO_MATRIZ_1 = "'Grupo matriz 1'";
        private const string GRUPO_MATRIZ_2 = "'Grupo matriz 2'";
        private const string GRUPO_MATRIZ_3 = "'Grupo matriz 3'";
        private const string GRUPO_MATRIZ_4 = "'Grupo matriz 4'";
        private const string GRUPO_MATRIZ_8 = "'Grupo matriz 8'";
        private const string GRUPO_MATRIZ_7 = "'Grupo matriz 7'";

        private const string COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO = "componente_curricular_grupo_area_ordenacao";

        protected const string CODIGO_1 = "1";
        protected const string CODIGO_2 = "2";
        protected const string CODIGO_3 = "3";
        protected const string CODIGO_8 = "8";
        protected const string CODIGO_4 = "4";
        protected const string CODIGO_5 = "5";
        protected const string CODIGO_7 = "7";
        protected const string CODIGO_10 = "10";
        protected const string NULO = "null";
        
        protected const int NUMERO_0 = 0;
        protected const int NUMERO_1 = 1;
        protected const int NUMERO_2 = 2;
        protected const int NUMERO_3 = 3;
        protected const int RETORNAR_4 = 4;
        
        protected const  bool ehPorcentagem = true;
        
        protected const long NUMERO_LONGO_1 = 1;
        protected const long NUMERO_LONGO_2 = 2;
        protected const long NUMERO_LONGO_3 = 3;
        protected const long NUMERO_LONGO_4 = 4;
        protected const long NUMERO_LONGO_5 = 5;
        
        protected const int NUMERO_INTEIRO_0 = 0;
        protected const int NUMERO_INTEIRO_1 = 1;
        protected const int NUMERO_INTEIRO_2 = 2;
        protected const int NUMERO_INTEIRO_3 = 3;
        protected const int NUMERO_INTEIRO_4 = 4;
        protected const int NUMERO_INTEIRO_5 = 5;
        protected const int NUMERO_INTEIRO_15 = 15;
        protected const int NUMERO_INTEIRO_16 = 16;
        protected const int NUMERO_INTEIRO_19 = 19;
        protected const int NUMERO_INTEIRO_20 = 20;
        
        protected const long PERIODO_ESCOLAR_CODIGO_1 = 1;
        protected const long PERIODO_ESCOLAR_CODIGO_2 = 2;
        protected const long PERIODO_ESCOLAR_CODIGO_3 = 3;
        protected const long PERIODO_ESCOLAR_CODIGO_4 = 4;

        protected const long PERIODO_FECHAMENTO_ID_1 = 1;

        protected const string PROVA = "Prova";
        protected const string TESTE = "Teste";

        private const string ED_INF_EMEI_4_HS = "'ED.INF. EMEI 4 HS'";
        private const string REGENCIA_CLASSE_INFANTIL = "'Regência de Classe Infantil'";
        private const string REGENCIA_INFATIL_EMEI_4H = "'REGÊNCIA INFANTIL EMEI 4H'";

        protected const string UE_CODIGO_1 = "1";
        protected const string UE_NOME_1 = "Nome da UE";
        
        protected const string UE_CODIGO_2 = "2";
        protected const string UE_NOME_2 = "UE 2";
        
        protected const string UE_CODIGO_3 = "3";
        protected const string UE_NOME_3 = "UE 3";

        protected const string DRE_CODIGO_1 = "1";
        protected const string DRE_NOME_1 = "DRE 1";
        
        protected const string DRE_CODIGO_2 = "2";
        protected const string DRE_NOME_2 = "DRE 2";

        protected const string SISTEMA_NOME = "Sistema";
        protected const string SISTEMA_CODIGO_RF = "1";

        private const string EVENTO_NOME_FESTA = "Festa";

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

        protected const string USUARIO_CP_LOGIN_3333333 = "3333333";
        protected const string USUARIO_CEFAI_LOGIN_3333333 = "3333333";
        protected const string USUARIO_PAAI_LOGIN_3333333 = "3333333";
        public const string USUARIO_LOGIN_PAAI = "4444444";
        protected const string USUARIO_PAAI_LOGIN_5555555 = "5555555";
        protected const string USUARIO_PAEE_LOGIN_5555555 = "5555555";
        protected const string USUARIO_CP_CODIGO_RF_3333333 = "3333333";
        protected const string USUARIO_CP_NOME_3333333 = "Nome do usuario 3333333";

        protected const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        protected const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        protected const string USUARIO_PROFESSOR_NOME_2222222 = "Nome do usuario 2222222";

        protected const long USUARIO_RF_1111111_ID_2 = 2;
        protected const string USUARIO_PROFESSOR_LOGIN_1111111 = "1111111";
        protected const string USUARIO_PROFESSOR_CODIGO_RF_1111111 = "1111111";
        protected const string USUARIO_PROFESSOR_NOME_1111111 = "Nome do usuário 1111111";

        private const string PROFESSOR = "Professor";
        private const int ORDEM_290 = 290;

        private const string PROFESSOR_CJ = "Professor CJ";
        private const int ORDEM_320 = 320;

        private const string CP = "CP";
        private const int ORDEM_240 = 240;

        protected const int BIMESTRE_1 = 1;
        protected const int BIMESTRE_2 = 2;
        protected const int BIMESTRE_3 = 3;
        protected const int BIMESTRE_4 = 4;
        protected const int BIMESTRE_FINAL = 0;

        protected const string EVENTO_NAO_LETIVO = "Evento não letivo";
        protected const long TIPO_EVENTO_21 = 21;
        protected const long TIPO_EVENTO_1 = 1;
        protected const long TIPO_EVENTO_2 = 2;
        protected const long TIPO_EVENTO_13 = 13;
        protected const long TIPO_EVENTO_14 = 14;
        protected const string SUSPENSAO_DE_ATIVIDADES = "Suspensão de Atividades";
        protected const string REPOSICAO_AULA = "Reposição de Aula";
        protected const string REPOSICAO_DIA = "Reposição Dia";
        protected const string REPOSICAO_AULA_DE_GREVE = "Reposição de Aula de Greve";
        protected const string LIBERACAO_EXCEPCIONAL = "Liberação excepcional";
        protected const int TIPO_CALENDARIO_ID = 1;

        protected DateTime DATA_01_01_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        protected DateTime DATA_01_05_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 01);
        protected DateTime DATA_29_04_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 29);
        protected DateTime DATA_02_05_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_24_07_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 24);
        protected DateTime DATA_25_07_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected DateTime DATA_02_10_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 02);
        protected DateTime DATA_03_10_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected DateTime DATA_22_12_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);
        
        protected DateTime DATA_03_01_INICIO_BIMESTRE_1_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 03);
        protected DateTime DATA_28_04_FIM_BIMESTRE_1_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 04, 28);
        protected DateTime DATA_02_05_INICIO_BIMESTRE_2_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 05, 02);
        protected DateTime DATA_08_07_FIM_BIMESTRE_2_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 07, 08);
        protected DateTime DATA_25_07_INICIO_BIMESTRE_3_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 07, 25);
        protected DateTime DATA_30_09_FIM_BIMESTRE_3_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 09, 30);
        protected DateTime DATA_03_10_INICIO_BIMESTRE_4_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 10, 03);
        protected DateTime DATA_22_12_FIM_BIMESTRE_4_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 22);        

        protected const long TIPO_CALENDARIO_1 = 1;
        protected const long TIPO_CALENDARIO_2 = 2;
        protected const long TIPO_CALENDARIO_3 = 3;
        protected const long TIPO_CALENDARIO_4 = 4;
        protected const long TIPO_CALENDARIO_5 = 5;
        protected const long TIPO_CALENDARIO_6 = 6;
        protected const long TIPO_CALENDARIO_7 = 7;
        protected const long TIPO_CALENDARIO_8 = 8;
        protected const long TIPO_CALENDARIO_9 = 9;
        protected const long TIPO_CALENDARIO_10 = 10;

        protected string DATA_INICIO_SGP = "DataInicioSGP";
        protected string NUMERO_50 = "50";
        protected string NUMERO_5 = "5";
        protected string PERCENTUAL_ALUNOS_INSUFICIENTES = "PERCENTUAL_ALUNOS_INSUFICIENTES";
        protected string MEDIA_BIMESTRAL = "MEDIA_BIMESTRAL";

        protected DateTime DATA_03_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 02);
        protected DateTime DATA_28_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 28);

        protected const int NUMERO_AULA_1 = 1;
        protected const int NUMERO_AULA_2 = 2;
        protected const int NUMERO_AULA_3 = 3;
        protected const int NUMERO_AULA_4 = 4;
        
        protected const int AULA_ID_1 = 1;
        protected const int AULA_ID_2 = 2;
        protected const int AULA_ID_3 = 3;
        protected const int AULA_ID_4 = 4;
        protected const int AULA_ID_5 = 5;
        protected const int AULA_ID_6 = 6;
        protected const int AULA_ID_7 = 7;
        protected const int AULA_ID_8 = 8;
        protected const int AULA_ID_9 = 9;
        protected const int AULA_ID_10 = 10;
        protected const int AULA_ID_11 = 11;
        protected const int AULA_ID_12 = 12;
        protected const int AULA_ID_13 = 13;
        protected const int AULA_ID_14 = 14;
        protected const int AULA_ID_15 = 15;
        protected const int AULA_ID_16 = 16;
        protected const int AULA_ID_17 = 17;
        protected const int AULA_ID_18 = 18;
        protected const int AULA_ID_19 = 19;
        protected const int AULA_ID_20 = 20;

        protected const long REGISTRO_FREQUENCIA_1 = 1;
        protected const long REGISTRO_FREQUENCIA_2 = 2;

        protected const string ALFABETIZACAO = "ALFABETIZACAO";
        protected const string INTERDISCIPLINAR = "INTERDISCIPLINAR";
        protected const string AUTORAL = "AUTORAL";
        protected const string MEDIO = "MEDIO";
        protected const string EJA_ALFABETIZACAO = "EJA_ALFABETIZACAO";
        protected const string EJA_BASICA = "EJA_BASICA";
        protected const string EJA_COMPLEMENTAR = "EJA_COMPLEMENTAR";
        protected const string EJA_FINAL = "EJA_FINAL";

        protected const string ANO_1 = "1";
        protected const string ANO_2 = "2";
        protected const string ANO_3 = "3";
        protected const string ANO_4 = "4";
        protected const string ANO_5 = "5";
        protected const string ANO_6 = "6";
        protected const string ANO_7 = "7";
        protected const string ANO_8 = "8";
        protected const string ANO_9 = "9";

        protected readonly long ATIVIDADE_AVALIATIVA_1 = 1;
        protected readonly long ATIVIDADE_AVALIATIVA_2 = 2;

        protected DateTime DATA_04_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 04);

        protected readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 04);
        protected readonly DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected readonly DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 07);

        protected readonly DateTime DATA_20_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 20);
        protected readonly DateTime DATA_21_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 21);
        protected readonly DateTime DATA_22_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 22);
        protected readonly DateTime DATA_23_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 23);
        protected readonly DateTime DATA_24_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 24);

        protected readonly DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected readonly DateTime DATA_26_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 26);
        protected readonly DateTime DATA_27_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 27);
        protected readonly DateTime DATA_28_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 28);
        protected readonly DateTime DATA_29_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 29);
        protected readonly DateTime DATA_30_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 30);
        protected readonly DateTime DATA_31_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 31);
        protected readonly DateTime DATA_01_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 01);
        protected readonly DateTime DATA_02_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 02);
        protected readonly DateTime DATA_03_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 03);
        protected readonly DateTime DATA_04_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 04);
        protected readonly DateTime DATA_05_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 05);
        protected readonly DateTime DATA_06_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 06);

        protected readonly DateTime DATA_16_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 16);
        protected readonly DateTime DATA_30_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);

        protected readonly DateTime DATA_01_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 01);
        protected readonly DateTime DATA_03_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected readonly DateTime DATA_22_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        protected readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        protected readonly DateTime DATA_01_01_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01);

        protected readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);
        protected readonly DateTime DATA_31_12_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 31);

        protected readonly DateTime DATA_10_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 10);
        
        protected readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        protected const int AULA_ID = 1;
        protected const int QUANTIDADE_AULA_NORMAL_MAIS_RECORRENTES_3 = 3;
        protected const int QUANTIDADE_AULA_RECORRENTE_2 = 2;
        protected const int QUANTIDADE_AULA = 1;
        protected const int QUANTIDADE_AULA_2 = 2;
        protected const int QUANTIDADE_AULA_3 = 3;
        protected const int QUANTIDADE_AULA_4 = 4;
        protected const string CODIGO_ALUNO_1 = "1";
        protected const string CODIGO_ALUNO_2 = "2";
        protected const string CODIGO_ALUNO_3 = "3";
        protected const string CODIGO_ALUNO_4 = "4";
        protected const string CODIGO_ALUNO_5 = "5";
        protected const string CODIGO_ALUNO_6 = "6";
        protected const string CODIGO_ALUNO_7 = "7";
        protected const string CODIGO_ALUNO_8 = "8";
        protected const string CODIGO_ALUNO_9 = "9";
        protected const string CODIGO_ALUNO_10 = "10";
        protected const string CODIGO_ALUNO_11 = "11";
        protected const string CODIGO_ALUNO_12 = "12";
        protected const string CODIGO_ALUNO_13 = "13";
        protected const string CODIGO_ALUNO_14 = "14";
        protected const string CODIGO_ALUNO_15 = "15";
        protected const int TOTAL_AUSENCIAS_1 = 1;
        protected const int TOTAL_AUSENCIAS_3 = 3;
        protected const int TOTAL_AUSENCIAS_7 = 7;
        protected const int TOTAL_AUSENCIAS_8 = 8;
        protected const int TOTAL_COMPENSACOES_1 = 1;
        protected const int TOTAL_COMPENSACOES_3 = 3;
        protected const int TOTAL_COMPENSACOES_7 = 7;
        protected const int TOTAL_COMPENSACOES_8 = 8;        
        protected const int TOTAL_PRESENCAS_1 = 1;
        protected const int TOTAL_PRESENCAS_2 = 2;
        protected const int TOTAL_PRESENCAS_3 = 3;
        protected const int TOTAL_PRESENCAS_4 = 4;
        protected const int TOTAL_REMOTOS_0 = 0;
        
        protected const int COMPENSACAO_AUSENCIA_ID_1 = 1;

        protected DateTime DATA_01_02_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);
        protected DateTime DATA_25_04_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 25);
        protected const string REABERTURA_GERAL = "Reabrir Geral";
        protected DateTime DATA_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected DateTime DATA_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected DateTime DATA_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected DateTime DATA_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);
        protected DateTime DATA_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected DateTime DATA_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        protected const long ATESTADO_MEDICO_DO_ALUNO_1 = 1;
        protected const long ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2 = 2;
        protected const long DOENCA_NA_FAMILIA_SEM_ATESTADO_3 = 3;
        protected const long OBITO_DE_PESSOA_DA_FAMILIA_4 = 4;
        protected const long INEXISTENCIA_DE_PESSOA_PARA_LEVAR_A_ESCOLA_5 = 5;
        protected const long ENCHENTE_6 = 6;
        protected const long FALTA_DE_TRANSPORTE_7 = 7;
        protected const long VIOLENCIA_NA_AREA_ONDE_MORA_8 = 8;
        protected const long CALAMIDADE_PUBLICA_QUE_ATINGIU_A_ESCOLA_OU_EXIGIU_O_USO_DO_ESPAÇO_COMO_ABRIGAMENTO_9 = 9;
        protected const long ESCOLA_FECHADA_POR_SITUACAO_DE_VIOLENCIA_10 = 10;

        protected const string DESCRICAO_FREQUENCIA_ALUNO_1 = "Lorem Ipsum";
        protected const string DESCRICAO_FREQUENCIA_ALUNO_2 = "é um texto bastante conhecido";

        protected const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_NOME = "PercentualAlunosInsuficientes";
        protected const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_VALOR_50 = "50";
        protected const string PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_DESCRICAO = "Percentual de alunos com nota/conceito insuficientes para exigência de justificativ";


        protected const string ALUNO_CODIGO_1 = "1";
        protected const string ALUNO_NOME_1 = "Nome do Aluno 1";
        protected readonly string ALUNO_CODIGO_2 = "2";
        protected readonly string ALUNO_CODIGO_3 = "3";
        protected readonly string ALUNO_CODIGO_4 = "4";
        protected readonly string ALUNO_CODIGO_5 = "5";
        protected readonly string ALUNO_CODIGO_6 = "6";
        protected readonly string ALUNO_CODIGO_7 = "7";
        protected readonly string ALUNO_CODIGO_8 = "8";
        protected readonly string ALUNO_CODIGO_9 = "9";
        protected readonly string ALUNO_CODIGO_10 = "10";
        protected readonly string ALUNO_CODIGO_11 = "11";
        protected readonly string ALUNO_CODIGO_12 = "12";
        protected readonly string ALUNO_CODIGO_13 = "13";
        

        protected const double NOTA_1 = 1;
        protected const double NOTA_2 = 2;
        protected const double NOTA_3 = 3;
        protected const double NOTA_4 = 4;
        protected const double NOTA_5 = 5;
        protected const double NOTA_6 = 6;
        protected const double NOTA_7 = 7;
        protected const double NOTA_8 = 8;
        protected const double NOTA_9 = 9;
        protected const double NOTA_10 = 10;

        protected const string PLENAMENTE_SATISFATORIO = "P";
        protected const int PLENAMENTE_SATISFATORIO_ID_1 = 1;
        protected const string SATISFATORIO = "S";
        protected const int SATISFATORIO_ID_2 = 2;
        protected const string NAO_SATISFATORIO = "NS";
        protected const int NAO_SATISFATORIO_ID_3 = 3;
        
        protected readonly string NOTA = "NOTA";
        protected readonly string CONCEITO = "CONCEITO";

        protected readonly string PERCENTUAL_FREQUENCIA_CRITICO_NOME = "PercentualFrequenciaCritico";
        protected readonly string PERCENTUAL_FREQUENCIA_CRITICO_DESCRICAO = "Percentual de frequência para definir aluno em situação crítica";
        protected readonly string NUMERO_PAGINA = "NumeroPagina";
        protected readonly string NUMERO_REGISTROS = "NumeroRegistros";
        protected readonly string ADMINISTRADOR = "Administrador";
        protected readonly string NOME_ADMINISTRADOR = "NomeAdministrador";
        
        protected readonly CollectionFixture collectionFixture;

        protected TesteBaseComuns(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            this.collectionFixture = collectionFixture ?? throw new ArgumentNullException(nameof(collectionFixture));
        }

        protected void CriarClaimUsuario(string perfil, string pagina = "0", string registros = "10")
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            
            contextoAplicacao.AdicionarVariaveis(ObterVariaveisPorPerfil(perfil, pagina, registros));
        }

        private Dictionary<string, object> ObterVariaveisPorPerfil(string perfil, string pagina, string registros)
        {
            var rfLoginPerfil = ObterRfLoginPerfil(perfil);
            
            return new Dictionary<string, object>
            {
                { USUARIO_CHAVE, rfLoginPerfil },
                { USUARIO_LOGADO_CHAVE, rfLoginPerfil },
                { USUARIO_RF_CHAVE, rfLoginPerfil },
                { USUARIO_LOGIN_CHAVE, rfLoginPerfil },
                { NUMERO_PAGINA, pagina },
                { NUMERO_REGISTROS, registros },
                { ADMINISTRADOR, rfLoginPerfil },
                { NOME_ADMINISTRADOR, rfLoginPerfil },
                {
                    USUARIO_CLAIMS_CHAVE,
                    new List<InternalClaim> {
                        new InternalClaim { Value = rfLoginPerfil, Type = USUARIO_CLAIM_TIPO_RF },
                        new InternalClaim { Value = perfil, Type = USUARIO_CLAIM_TIPO_PERFIL }
                    }
                }
            };
        }

        private string ObterRfLoginPerfil(string perfil)
        {
            if (perfil.Equals(ObterPerfilProfessor()) || perfil.Equals(ObterPerfilCJ()))
                return USUARIO_PROFESSOR_LOGIN_2222222;

            if (perfil.Equals(ObterPerfilDiretor()))
                return USUARIO_LOGIN_DIRETOR;
            
            if (perfil.Equals(ObterPerfilAD()))
                return USUARIO_LOGIN_AD;

            if (perfil.Equals(ObterPerfilPaai()))
                return USUARIO_LOGIN_PAAI;
            
            if (perfil.Equals(ObterPerfilPaee()))
                return USUARIO_PAAI_LOGIN_5555555;

            if (perfil.Equals(ObterPerfilAdmDre()))
                return USUARIO_LOGIN_ADM_DRE;

            if (perfil.Equals(ObterPerfilAdmSme()))
                return USUARIO_LOGIN_ADM_SME;

            if (perfil.Equals(ObterPerfilPAP()))
                return USUARIO_LOGIN_PAP;

            return USUARIO_PROFESSOR_LOGIN_2222222;
        }

        protected string ObterPerfilProfessor()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR.Name()).ToString();
        }
        
        protected string ObterPerfilCoordenadorNAAPA()
        {
            return Guid.Parse(PerfilUsuario.COORDENADOR_NAAPA.Name()).ToString();
        }
        
        protected string ObterPerfilPsicologoEscolar()
        {
            return Guid.Parse(PerfilUsuario.PSICOLOGO_ESCOLAR.Name()).ToString();
        }
        
        protected string ObterPerfilPsicopedagogo()
        {
            return Guid.Parse(PerfilUsuario.PSICOPEDAGOGO.Name()).ToString();
        }
        
        protected string ObterPerfilAssistenteSocial()
        {
            return Guid.Parse(PerfilUsuario.ASSISTENTE_SOCIAL.Name()).ToString();
        }

        protected string ObterPerfilCJ()
        {
            return Guid.Parse(PerfilUsuario.CJ.Name()).ToString();
        }

        protected string ObterPerfilCoordenadorCefai()
        {
            return Guid.Parse(PerfilUsuario.CEFAI.Name()).ToString();
        }
        protected string ObterPerfilPaai()
        {
            return Guid.Parse(PerfilUsuario.PAAI.Name()).ToString();
        }
        
        protected string ObterPerfilPaee()
        {
            return Guid.Parse(PerfilUsuario.PAEE.Name()).ToString();
        }

        protected string ObterPerfilAdmDre()
        {
            return Guid.Parse(PerfilUsuario.ADMDRE.ObterNome()).ToString();
        }

        protected string ObterPerfilAdmSme()
        {
            return Guid.Parse(PerfilUsuario.ADMSME.ObterNome()).ToString();
        }

        protected string ObterPerfilCJInfantil()
        {
            return Guid.Parse(PerfilUsuario.CJ_INFANTIL.Name()).ToString();
        }

        protected string ObterPerfilProfessorInfantil()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.Name()).ToString();
        }

        protected string ObterPerfilCP()
        {
            return Guid.Parse(PerfilUsuario.CP.Name()).ToString();
        }
        
        protected string ObterPerfilCEFAI()
        {
            return Guid.Parse(PerfilUsuario.CEFAI.Name()).ToString();
        }

        protected string ObterPerfilAD()
        {
            return Guid.Parse(PerfilUsuario.AD.Name()).ToString();
        }

        protected string ObterPerfilDiretor()
        {
            return Guid.Parse(PerfilUsuario.DIRETOR.Name()).ToString();
        }
        protected string ObterPerfilPAP()
        {
            return Guid.Parse(PerfilUsuario.PAP.Name()).ToString();
        }
        protected string ObterPerfilPOA_Portugues()
        {
            return Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()).ToString();
        }

        protected async Task CriarPeriodoEscolarEncerrado()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = BIMESTRE_2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarEvento(EventoLetivo letivo, DateTime dataInicioEvento, DateTime dataFimEvento, bool eventoEscolaAqui = false, long tipoEventoId = 1)
        {
            await InserirNaBase(new EventoTipo
            {
                Id = tipoEventoId,
                Descricao = EVENTO_NOME_FESTA,
                Ativo = true,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                EventoEscolaAqui = eventoEscolaAqui
            });

            await InserirNaBase(new Evento
            {
                Nome = EVENTO_NOME_FESTA,
                TipoCalendarioId = 1,
                TipoEventoId = tipoEventoId,
                UeId = UE_CODIGO_1,
                Letivo = letivo,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicioEvento,
                DataFim = dataFimEvento,
                Status = EntidadeStatus.Aprovado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarComunicadoAluno(string comunicado, int anoLetivo, string codigoDre, string codigoUe, string codigoTurma, string codigoAluno, long comunicadoId = 1)
        {
            await InserirNaBase(new Comunicado
            {
                Id = comunicadoId,
                Descricao = comunicado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                AnoLetivo = anoLetivo,
                CodigoDre = codigoDre,
                CodigoUe = codigoUe,
                Titulo = comunicado,
                TipoComunicado = TipoComunicado.ALUNO,
            });

            await InserirNaBase(new ComunicadoAluno
            {
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                ComunicadoId = comunicadoId,
                AlunoCodigo = codigoAluno,
                AlunoNome = "Nome Teste Aluno",
            });

            await InserirNaBase(new ComunicadoTurma
            {
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                ComunicadoId = comunicadoId,
                CodigoTurma = codigoTurma
            });
        }

        protected async Task CriarEventoTipoResumido(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, long codigo)
        {
            await CriarEventoTipo(descricao, localOcorrencia, concomitancia, tipoData, dependencia, letivo, true, false, codigo, false, false);
        }

        protected async Task CriarEventoTipo(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, bool ativo, bool excluido, long codigo, bool somenteLeitura, bool eventoEscolaAqui)
        {
            await InserirNaBase(new EventoTipo()
            {
                Descricao = descricao,
                LocalOcorrencia = localOcorrencia,
                Concomitancia = concomitancia,
                TipoData = tipoData,
                Dependencia = dependencia,
                Letivo = letivo,
                Ativo = ativo,
                Excluido = excluido,
                Codigo = codigo,
                SomenteLeitura = somenteLeitura,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarEventoResumido(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus)
        {
            await CriarEvento(nomeEvento, dataInicio, dataFim, eventoLetivo, tipoCalendarioId, tipoEventoId, dreId, ueId, eventoStatus, null, null, null, null);
        }

        protected async Task CriarEvento(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus, long? workflowAprovacaoId, TipoPerfil? tipoPerfil, long? eventoPaiId, long? feriadoId, bool migrado = false)
        {
            await InserirNaBase(new Evento
            {
                Nome = nomeEvento,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Letivo = eventoLetivo,
                TipoCalendarioId = tipoCalendarioId,
                TipoEventoId = tipoEventoId,
                DreId = dreId,
                UeId = ueId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Status = eventoStatus,
                WorkflowAprovacaoId = workflowAprovacaoId,
                Migrado = migrado,
                TipoPerfilCadastro = tipoPerfil,
                EventoPaiId = eventoPaiId,
                FeriadoId = feriadoId
            });
        }

        protected async Task CriarAtribuicaoEsporadica(DateTime dataInicio, DateTime dataFim)
        {
            await InserirNaBase(new AtribuicaoEsporadica
            {
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicio,
                DataFim = dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, bool substituir = true)
        {
            await InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                DisciplinaId = componenteCurricularId,
                Modalidade = modalidade,
                Substituir = substituir,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, string professorRf, bool substituir = true)
        {
            await InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = professorRf,
                DisciplinaId = componenteCurricularId,
                Modalidade = modalidade,
                Substituir = substituir,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarUsuarios()
        {
            await InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = USUARIO_PROFESSOR_NOME_2222222,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_1111111,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                Nome = USUARIO_PROFESSOR_NOME_1111111,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_CP_LOGIN_3333333,
                Login = USUARIO_CP_LOGIN_3333333,
                Nome = USUARIO_CP_NOME_3333333,
                PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_CP,
                Login = USUARIO_LOGIN_CP,
                Nome = USUARIO_LOGIN_CP,
                PerfilAtual = Guid.Parse(PerfilUsuario.CP.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_DIRETOR,
                Login = USUARIO_LOGIN_DIRETOR,
                Nome = USUARIO_LOGIN_DIRETOR,
                PerfilAtual = Guid.Parse(PerfilUsuario.DIRETOR.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_AD,
                Login = USUARIO_LOGIN_AD,
                Nome = USUARIO_LOGIN_AD,
                PerfilAtual = Guid.Parse(PerfilUsuario.AD.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_PAAI,
                Login = USUARIO_LOGIN_PAAI,
                Nome = USUARIO_LOGIN_PAAI,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAAI.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
               CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });
            await InserirNaBase(new Usuario
            {
                Login = USUARIO_PAEE_LOGIN_5555555,
                CodigoRf = USUARIO_PAEE_LOGIN_5555555,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAEE.ObterNome()),
                Nome = USUARIO_PAEE_LOGIN_5555555,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Usuario
            {
                Login = USUARIO_LOGIN_PAP,
                CodigoRf = USUARIO_LOGIN_PAP,
                PerfilAtual = Guid.Parse(PerfilUsuario.PAP.ObterNome()),
                Nome = USUARIO_LOGIN_PAP,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new Usuario
            {
                Login = SISTEMA_NOME,
                CodigoRf = SISTEMA_NOME,
                PerfilAtual = Guid.Parse(PerfilUsuario.ADMSME.ObterNome()),
                Nome = SISTEMA_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarTurma(Modalidade modalidade, bool turmaHistorica = false, bool turmasMesmaUe = false, int tipoTurnoEol = 0)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });
            
            await InserirNaBase(new Dominio.Turma
            {
                UeId = turmasMesmaUe ? 1 : 2,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_2,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_2,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });
            
            await InserirNaBase(new Dominio.Turma
            {
                UeId = turmasMesmaUe ? 1 : 3,
                Ano = TURMA_ANO_3,
                CodigoTurma = TURMA_CODIGO_3,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_3,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = tipoTurnoEol
            });
        }

        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, bool turmaHistorica = false, 
            TipoTurma tipoTurma = TipoTurma.Regular, int tipoTurno = 0)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = tipoTurma,
                TipoTurno = tipoTurno
            });
        }

        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, bool turmaHistorica = false)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }
        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, bool turmaHistorica = false )
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = turmaHistorica ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = tipoTurma
            });
        }
        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, long ueId,int anoLetivo,bool turmaHistorica = false, string nomeTurma = null)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = ueId,
                Ano = anoTurma,
                CodigoTurma = codigoTurma,
                Historica = turmaHistorica,
                ModalidadeCodigo = modalidade,
                AnoLetivo = anoLetivo,
                Semestre = SEMESTRE_1,
                Nome = nomeTurma ??TURMA_NOME_1,
                TipoTurma = tipoTurma
            });
        }

        protected async Task CriarDreUe(string codigoDre,string codigoUe)
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = codigoDre,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = 1,
                Nome = UE_NOME_1,
            });
            
            await InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = 1,
                Nome = UE_NOME_2,
            });
        }

        protected async Task CriarDreUe(string codigoDre, string nomeDre, string codigoUe, string nomeUe)
        {
            var dres = ObterTodos<Dre>();
            long? idDre = dres.Where(d => d.CodigoDre.Equals(codigoDre)).FirstOrDefault()?.Id;

            if (!idDre.HasValue)
                await InserirNaBase(new Dre
                {
                    CodigoDre = codigoDre,
                    Abreviacao = nomeDre,
                    Nome = nomeDre
                });
            
            await InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = idDre ?? dres.Count+1,
                Nome = nomeUe,
            });
        }
        protected async Task CriarAtividadeAvaliativaFundamental(DateTime dataAvaliacao)
        {
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAvaliacao, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), USUARIO_PROFESSOR_CODIGO_RF_2222222, false, ATIVIDADE_AVALIATIVA_1);
        }

        protected async Task CrieTipoAtividade()
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, string rf, bool ehCj, long idAtividade)
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                EhCj = ehCj,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                AtividadeAvaliativaId = idAtividade,
                DisciplinaId = componente,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativaFundamental(
                                    DateTime dataAvaliacao,
                                    string componente,
                                    TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                                    bool ehRegencia = false,
                                    bool ehCj = false,
                                    string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriaTipoAvaliacao(tipoAvalicao);

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = componente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }



        protected async Task CriarAtividadeAvaliativaRegencia(string componente, string nomeComponente)
        {

            await InserirNaBase(new AtividadeAvaliativaRegencia
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaContidaRegenciaId = componente,
                DisciplinaContidaRegenciaNome = nomeComponente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior = false, int semestre = SEMESTRE_1)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = considerarAnoAnterior ? ANO_LETIVO_ANO_ANTERIOR : ANO_LETIVO_ANO_ATUAL,
                Nome = considerarAnoAnterior ? NOME_TIPO_CALENDARIO_ANO_ANTERIOR : NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Periodo = Periodo.Semestral,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                Semestre = tipoCalendario.EhEjaOuCelp() ? semestre : null
            });
        }

        protected async Task CriarItensComuns(bool criarPeriodo, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await CriarDreUePerfil();
            if (criarPeriodo) await CriarPeriodoEscolar(dataInicio, dataFim, bimestre, tipoCalendarioId);
            await CriarComponenteCurricular();
        }

        protected async Task CriarDreUePerfilComponenteCurricular()
        {
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
        }

        protected async Task CriaTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao)
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = tipoAvalicao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral, bool ehRegencia = false,
                                                      bool ehCj = false, string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriaTipoAvaliacao(tipoAvalicao);

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarDreUePerfil()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_1,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_1,
                DreId = 1,
                Nome = UE_NOME_1,
                TipoEscola = TipoEscola.EMEF
            });
            
            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_2,
                Abreviacao = DRE_NOME_2,
                Nome = DRE_NOME_2
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_2,
                DreId = 2,
                Nome = UE_NOME_2,
                TipoEscola = TipoEscola.EMEF
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_3,
                DreId = 2,
                Nome = UE_NOME_3,
                TipoEscola = TipoEscola.EMEF
            });

            await InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                NomePerfil = PROFESSOR,
                Ordem = ORDEM_290,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.CJ.Name()),
                NomePerfil = PROFESSOR_CJ,
                Ordem = ORDEM_320,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = ORDEM_240,
                Tipo = TipoPerfil.UE,
                NomePerfil = CP,
                CodigoPerfil = Perfis.PERFIL_CP,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 230,
                Tipo = TipoPerfil.UE,
                NomePerfil = "AD",
                CodigoPerfil = Perfis.PERFIL_AD,
                CriadoEm = DateTime.Now,CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 220,
                Tipo = TipoPerfil.UE,
                NomePerfil = "DIRETOR",
                CodigoPerfil = Perfis.PERFIL_DIRETOR,
                CriadoEm = DateTime.Now,CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });

                await InserirNaBase("tipo_escola", new string[] { "cod_tipo_escola_eol", "descricao", "criado_em", "criado_por", "criado_rf" }, new string[] { "1", "'EMEF'", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'", "'" + SISTEMA_NOME + "'", "'" + SISTEMA_CODIGO_RF + "'" });
        }

        protected async Task CriarPeriodoEscolar(DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = tipoCalendarioId,
                Bimestre = bimestre,
                PeriodoInicio = considerarAnoAnterior ? dataInicio.AddYears(-1) : dataInicio,
                PeriodoFim = considerarAnoAnterior ? dataFim.AddYears(-1) : dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }
        protected async Task CriarComponenteRegenciaInfantil()
        {
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_512.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_4HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_513.ToString(), COMPONENTE_CURRICULAR_513.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_2HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME);
        }

        protected async Task CriarComponenteCurricular()
        {
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_2, GRUPO_MATRIZ_2);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_3, GRUPO_MATRIZ_3);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_4, GRUPO_MATRIZ_4);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_8, GRUPO_MATRIZ_8);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_7, GRUPO_MATRIZ_7);


            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_1, AREA_DE_CONHECIMENTO_1);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_2, AREA_DE_CONHECIMENTO_2);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_3, AREA_DE_CONHECIMENTO_3);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_4, AREA_DE_CONHECIMENTO_4);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_5, AREA_DE_CONHECIMENTO_5);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_8, AREA_DE_CONHECIMENTO_8);
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_10, AREA_DE_CONHECIMENTO_10);

            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_1, CODIGO_1, CODIGO_1);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_2, CODIGO_2, CODIGO_2);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_3, CODIGO_3, CODIGO_3);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_4, CODIGO_4, CODIGO_4);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_4, CODIGO_5, CODIGO_5);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_8, CODIGO_8, CODIGO_8);
            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_AREA_ORDENACAO, CODIGO_8, CODIGO_10, CODIGO_10);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_ARTES_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_ARTES_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_MATEMATICA_ID_2.ToString(), NULO, CODIGO_1, CODIGO_2, COMPONENTE_CURRICULAR_MATEMATICA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_MATEMATICA_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_GEOGRAFIA_ID_8.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_GEOGRAFIA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_GEOGRAFIA_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), NULO, CODIGO_1, CODIGO_8, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_HISTORIA_ID_7.ToString(), NULO, CODIGO_1, CODIGO_4, COMPONENTE_HISTORIA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_HISTORIA_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061.ToString(), NULO, CODIGO_3, CODIGO_8, COMPONENTE_LEITURA_OSL_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_LEITURA_OSL_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_INGLES_ID_9.ToString(), NULO, CODIGO_2, CODIGO_5, COMPONENTE_CURRICULAR_INGLES_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_CURRICULAR_INGLES_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_EDUCACAO_FISICA_ID_6.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_EDUCACAO_FISICA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_EDUCACAO_FISICA_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CIENCIAS_ID_89.ToString(), NULO, CODIGO_1, CODIGO_3, COMPONENTE_CIENCIAS_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CIENCIAS_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_INFORMATICA_OIE_ID_1060.ToString(), NULO, CODIGO_3, NULO, COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_CURRICULAR_INFORMATICA_OIE_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214.ToString(), NULO, CODIGO_4, NULO, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME, FALSE, FALSE, TRUE, FALSE, TRUE, FALSE, COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_NOME, NULO);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_512.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_4HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME);
            
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(), COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME, COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO_NOME);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_513.ToString(), COMPONENTE_CURRICULAR_512.ToString(), CODIGO_1, NULO, COMPONENTE_ED_INF_EMEI_2HS_NOME, TRUE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_REGENCIA_CLASSE_INFANTIL_NOME, COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME);
            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM.ToString(), NULO, CODIGO_7, CODIGO_10, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, COMPONENTE_CURRICULAR_TEC_APRENDIZAGEM_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_ID_1116.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME, FALSE, FALSE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_LIBRAS_COMPARTILHADA_NOME, NULO);
        }
        
        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia : dataReferencia.AddDays(-5), BIMESTRE_4);
        }

        protected async Task CriarAula(DateTime dataAula, RecorrenciaAula recorrenciaAula, TipoAula tipoAula, string professorRf, string turmaCodigo, string ueCodigo, string disciplinaCodigo, long tipoCalendarioId, bool aulaCJ = false)
        {
            await InserirNaBase(new Dominio.Aula()
            {
                UeId = ueCodigo,
                DisciplinaId = disciplinaCodigo,
                TurmaId = turmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                ProfessorRf = professorRf,
                Quantidade = 1,
                DataAula = dataAula.Date,
                RecorrenciaAula = recorrenciaAula,
                TipoAula = tipoAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                AulaCJ = aulaCJ
            });
        }

        protected async Task CriarPeriodoEscolarReabertura(long tipoCalendarioId)
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, tipoCalendarioId);

            await CriarPeriodoReabertura(tipoCalendarioId);
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = dataInicio.HasValue ? dataInicio.Value : DATA_01_01,
                Fim = dataFim.HasValue ? dataFim.Value : DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected async Task CrieConceitoValores()
        {
            await InserirNaBase(new Conceito()
            {
                Valor = PLENAMENTE_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.P.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await InserirNaBase(new Conceito()
            {
                Valor = SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.S.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            await InserirNaBase(new Conceito()
            {
                Valor = NAO_SATISFATORIO,
                InicioVigencia = DATA_01_01,
                Ativo = true,
                Descricao = ConceitoValores.NS.GetAttribute<DisplayAttribute>().Name,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected async Task CriarParametrosSistema(int ano)
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = ConstantesTeste.PERCENTUAL_FREQUENCIA_CRITICO_TIPO_4_NOME,
                Descricao = ConstantesTeste.PERCENTUAL_FREQUENCIA_CRITICO_TIPO_4_DESCRICAO,
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Valor = ConstantesTeste.VALOR_75,
                Ano = ano,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
            
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = ConstantesTeste.PERCENTUAL_FREQUENCIA_MINIMO_INFANTIL_TIPO_27_NOME,
                Descricao = ConstantesTeste.PERCENTUAL_FREQUENCIA_MINIMO_INFANTIL_TIPO_27_DESCRICAO,
                Tipo = TipoParametroSistema.PercentualFrequenciaMinimaInfantil,
                Valor = ConstantesTeste.VALOR_60,
                Ano = ano,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA,true,SEMESTRE_1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1,true);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA,true, SEMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_2,true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_2,true);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP,true, SEMESTRE_1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_3,true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_3,true);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP,true, SEMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_4,true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_4,true);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, true);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_5,true);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_5,true);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_5,true);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_5,true);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA,semestre:1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_6);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_6);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.EJA,semestre:2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_7);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_7);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP,semestre:1);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_8);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_8);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP,semestre:2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_1, TIPO_CALENDARIO_9);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_2, TIPO_CALENDARIO_9);
            
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_10);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_10);
        }
        protected async Task CriarConselhoClasseConsolidadoTurmaAlunos()
        {
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 1,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "1",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 2,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "2",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 3,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "3",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 4,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "4",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 5,
                Status = SituacaoConselhoClasse.NaoIniciado,
                AlunoCodigo = "5",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 6,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "6",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 7,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "7",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 8,
                Status = SituacaoConselhoClasse.EmAndamento,
                AlunoCodigo = "8",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 9,
                Status = SituacaoConselhoClasse.Concluido,
                AlunoCodigo = "9",
                TurmaId = 1,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }
        public static DateTime ObterTerceiroSabadoDoMes(int ano, int mes)
        {
            // Define a data do primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(ano, mes, 1);

            // Obtém o primeiro sábado do mês
            DateTime primeiroSabado = primeiroDiaDoMes.AddDays((DayOfWeek.Saturday - primeiroDiaDoMes.DayOfWeek + 7) % 7);

            // Calcula o terceiro sábado adicionando 14 dias ao primeiro sábado
            DateTime terceiroSabado = primeiroSabado.AddDays(14);

            return terceiroSabado;
        }
    }
}
