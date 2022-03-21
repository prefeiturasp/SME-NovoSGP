USE [GestaoPedagogica]
GO


TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
GO


-- DANDO CARGA NA TABELA DE ETL (SQL)
INSERT INTO [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
SELECT DISTINCT
       CPA.tpc_id                            AS [BIMESTRE],
       TMP.IdDisciplina                      AS [DISCIPLINA_ID],
	   TUR.tur_codigoEOL                     AS [TURMA_ID],
	   NULL                                  AS [NOME],
	   CPA.cpa_atividadesDesenvolvidas       AS [DESCRICAO],
	   CPA.cpa_dataCriacao                   AS [CRIADO_EM],
	   CPA.cpa_dataAlteracao                 AS [ALTERADO_EM],
	   CAL.cal_ano                           AS [ANO_LETIVO],
	   AAC.alc_matricula                     AS [CODIGO_ALUNO],
	   CPA.cpa_quantidadeAulasCompensadas    AS [QTD_FALTAS_COMPENSADAS]                               

  FROM            [GestaoPedagogica].[dbo].[CLS_CompensacaoAusencia]			AS CPA (NOLOCK)

       INNER JOIN [GestaoPedagogica].[dbo].[CLS_CompensacaoAusenciaAluno]		AS CAA (NOLOCK)
	           ON CAA.cpa_id = CPA.cpa_id
			  AND CAA.tud_id = CPA.tud_id

	   INNER JOIN [GestaoPedagogica].[dbo].[MTR_MatriculaTurma]					AS MTR (NOLOCK)
	           ON MTR.alu_id = CAA.alu_id
			  AND MTR.mtu_id = CAA.mtu_id

	   INNER JOIN [GestaoPedagogica].[dbo].[TUR_Turma]							AS TUR (NOLOCK)
	           ON TUR.tur_id = MTR.tur_id

--     INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]                         AS ESC (NOLOCK)
--             ON ESC.esc_id = TUR.esc_id  
 
	   INNER JOIN [GestaoPedagogica].[dbo].[ACA_AlunoCurriculo]					AS AAC (NOLOCK)
	           ON AAC.alu_id = CAA.alu_id
			  AND AAC.alu_id = MTR.alu_id
			  AND AAC.esc_id = TUR.esc_id
			  AND AAC.uni_id = TUR.uni_id

	   INNER JOIN [GestaoPedagogica].[dbo].[ACA_CalendarioAnual]				AS CAL (NOLOCK)
	           ON CAL.cal_id = TUR.cal_id

	   INNER JOIN [GestaoPedagogica].[dbo].[TUR_TurmaDisciplinaRelDisciplina]	AS TDD (NOLOCK)
               ON TDD.tud_id = CPA.tud_id
			  AND TDD.tud_id = CAA.tud_id

       INNER JOIN [GestaoPedagogica].[dbo].[ACA_Disciplina]						AS DIS (NOLOCK)
               ON DIS.dis_id = TDD.dis_id

       INNER JOIN [Manutencao].[dbo].[ETL_SGP_DISCIPLINAS]				        AS TMP (NOLOCK)
               ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS VARCHAR) COLLATE Latin1_General_CI_AS)

 WHERE CPA.cpa_situacao = 1
   AND CAA.caa_situacao = 1
   AND AAC.alc_situacao = 1
   AND TUR.tur_situacao IN (1,5,6,8)    -- 1. Ativo, 5. Encerrada, 6. Em matrícula, 8. Aguardando
   AND CAL.cal_ano BETWEEN 2014 AND 2019
-- AND CAL.cal_ano = 2019
-- AND ESC.esc_codigo IN ('094765','019455')

GO


-- CORRIGINDO DISCIPLINAS DE REGÊNCIA DE CLASSE
TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia_508]
GO


INSERT INTO [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia_508]
SELECT DISTINCT
       ETL.[BIMESTRE],
       GCC.cd_componente_curricular AS [DISCIPLINA_ID],
       ETL.[TURMA_ID],
       ETL.[NOME],
       ETL.[DESCRICAO],
       ETL.[CRIADO_EM],
       ETL.[ALTERADO_EM],
       ETL.[ANO_LETIVO],
       ETL.[CODIGO_ALUNO],
       ETL.[QTD_FALTAS_COMPENSADAS]

  FROM            [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]          AS ETL (NOLOCK)

       INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_turma_grade]           AS STG (NOLOCK) 
               ON STG.cd_turma_escola = ETL.TURMA_ID
              AND STG.dt_fim IS NULL

       INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                AS EGR (NOLOCK) 
               ON EGR.cd_escola_grade = STG.cd_escola_grade

       INNER JOIN [10.49.16.60].[se1426].[dbo].[grade_componente_curricular] AS GCC (NOLOCK) 
               ON GCC.cd_grade = EGR.cd_grade
              AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

 WHERE ETL.DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
GO


DELETE
  FROM [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
 WHERE DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
GO



INSERT INTO [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
SELECT DISTINCT
       [BIMESTRE],
       [DISCIPLINA_ID],
	   [TURMA_ID],
	   [NOME],
	   [DESCRICAO],
	   [CRIADO_EM],
	   [ALTERADO_EM],
	   [ANO_LETIVO],
	   [CODIGO_ALUNO],
	   [QTD_FALTAS_COMPENSADAS] 
  FROM [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia_508]
GO


-- CORRIGINDO CAMPOS NOME E DESCRIÇÃO
UPDATE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
   SET [DESCRICAO] = LTRIM(RTRIM([DESCRICAO]))
GO


UPDATE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
   SET [DESCRICAO] = dbo.fc_LimpaSujeira([DESCRICAO])
GO


UPDATE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
   SET [DESCRICAO] = 'Não informado no legado'
 WHERE [DESCRICAO] = ''
GO


UPDATE [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
   SET [NOME] = LEFT([DESCRICAO],1000)
GO



