USE GestaoPedagogica
GO


-- SELECIONANDO O MOVIMENTO
INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento]
SELECT DISTINCT
       TUR.tur_codigoEOL AS [TURMA_ID],
	   CASE WHEN AAT.ava_id IN (1,2,3,4) THEN  -- são bimestres
										 CASE YEAR(aat.atd_dataCriacao) WHEN 2019 THEN
																					CASE TUR.cal_id WHEN 17 THEN		-- 10 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 29
																																	WHEN 2 THEN 30
																																	WHEN 3 THEN 31
																																	WHEN 4 THEN 32
																													END
																									WHEN 19 THEN		-- 11 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 33
																																	WHEN 2 THEN 34
																													END
																									WHEN 20 THEN		-- 12 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 35
																																	WHEN 2 THEN 36
																													END
																					END
																		WHEN 2018 THEN
																					CASE TUR.cal_id WHEN 12 THEN		-- 7 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 21
																																	WHEN 2 THEN 22
																																	WHEN 3 THEN 23
																																	WHEN 4 THEN 24
																													END
																									WHEN 14 THEN		-- 8 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 25
																																	WHEN 2 THEN 26
																													END
																									WHEN 15 THEN		-- 9 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 27
																																	WHEN 2 THEN 28
																													END
																					END
																		WHEN 2017 THEN
																					CASE TUR.cal_id WHEN  7 THEN		-- 4 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 13
																																	WHEN 2 THEN 14
																																	WHEN 3 THEN 15
																																	WHEN 4 THEN 16
																													END
																									WHEN  9 THEN		-- 5 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 17
																																	WHEN 2 THEN 18
																													END
																									WHEN 10 THEN		-- 6 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 19
																																	WHEN 2 THEN 20
																													END
																					END
																		WHEN 2016 THEN
																					CASE TUR.cal_id WHEN  6 THEN		-- 3 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 9
																																	WHEN 2 THEN 10
																																	WHEN 3 THEN 11
																																	WHEN 4 THEN 12
																													END
																					END
																		WHEN 2015 THEN
																					CASE TUR.cal_id WHEN  5 THEN		-- 2 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 5
																																	WHEN 2 THEN 6
																																	WHEN 3 THEN 7
																																	WHEN 4 THEN 8
																													END
																					END
																		WHEN 2014 THEN
																					CASE TUR.cal_id WHEN  4 THEN		-- 1 no SGP
																													CASE AAT.ava_id WHEN 1 THEN 1
																																	WHEN 2 THEN 2
																																	WHEN 3 THEN 3
																																	WHEN 4 THEN 4
																													END
																					END

										 END
            ELSE  NULL  -- Síntese Final
		END AS [PERIODO_ESCOLAR_ID],
        CONVERT(DATETIME,CONVERT(VARCHAR, AAT.atd_dataCriacao, 120)) AS [CRIADO_EM],
        CONVERT(DATETIME,CONVERT(VARCHAR, AAT.atd_dataAlteracao, 120)) AS [ALTERADO_EM],
        TMP.IdDisciplina AS [DISCIPLINA_ID],
        ALC.alc_matricula AS [ALUNO_CODIGO],
		CASE WHEN ISNUMERIC(AAT.atd_avaliacao) = 1 THEN AAT.atd_avaliacao
		                                           ELSE NULL
		END AS [NOTA],
		CASE AAT.atd_avaliacao WHEN 'F (Frequente)'      THEN 1
		                       WHEN 'NF (Não Frequente)' THEN 2
							   ELSE NULL
		END AS [SINTESE_ID],
		CASE AAT.atd_avaliacao WHEN 'P'  THEN 1
		                       WHEN 'S'  THEN 2
							   WHEN 'NS' THEN 3
							   ELSE NULL
		END AS [CONCEITO_ID]

   FROM            CLS_AlunoAvaliacaoTurmaDisciplina AS AAT (NOLOCK)
        INNER JOIN CLS_AlunoFechamento               AS AFE (NOLOCK)
		        ON AFE.tud_id = AAT.tud_id
			   AND AFE.alu_id = AAT.alu_id
			   AND AFE.mtu_id = AAT.mtu_id
			   AND AFE.mtd_id = AAT.mtd_id
 		INNER JOIN MTR_MatriculaTurmaDisciplina      AS MTD (NOLOCK)
		        ON MTD.tud_id = AAT.tud_id
			   AND MTD.alu_id = AAT.alu_id
			   AND MTD.mtu_id = AAT.mtu_id
			   AND MTD.mtd_id = AAT.mtd_id
        INNER JOIN MTR_MatriculaTurma                AS MTU (NOLOCK)
		        ON MTU.alu_id = AAT.alu_id
			   AND MTU.mtu_id = AAT.mtu_id
		INNER JOIN ACA_AlunoCurriculo                AS ALC (NOLOCK)
		        ON ALC.alu_id = AAT.alu_id
		INNER JOIN ACA_Avaliacao                     AS AVA (NOLOCK)
		        ON AVA.ava_id = AAT.ava_id
			   AND AVA.fav_id = AAT.fav_id
		INNER JOIN Tur_Turma                         AS TUR (NOLOCK)
		        ON TUR.tur_id = MTU.tur_id
			   AND TUR.esc_id = ALC.esc_id
        INNER JOIN ESC_Escola                        AS ESC (NOLOCK)
		        ON ESC.esc_id = ALC.esc_id
		INNER JOIN TUR_TurmaDisciplinaRelDisciplina  AS TDD (NOLOCK)
                ON TDD.tud_id = AAT.tud_id
		INNER JOIN ACA_Disciplina                    AS DIS (NOLOCK)
                ON DIS.dis_id = TDD.dis_id
		INNER JOIN [Manutencao].[DBO].[ETL_SGP_DISCIPLINAS] AS TMP (NOLOCK)
				ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS varchar) COLLATE Latin1_General_CI_AS)

  WHERE YEAR(DIS.dis_dataCriacao) BETWEEN 2014 AND 2019
    AND TUR.tur_codigoEOL IS NOT NULL
	-- AND YEAR(aat.atd_dataCriacao) = 2019       -- <= TIRAR
    --AND ESC.esc_codigo IN ('019455','094765')   -- <= TIRAR
    AND UPPER(DIS.dis_nome) <> 'CONCEITO GLOBAL (INFANTIL I E II)'

  ORDER BY TUR.tur_codigoEOL, ALC.alc_matricula, TMP.IdDisciplina



-- GATO (devido à problemas de acesso na base da Prodam)
TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]
GO

INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]
SELECT * FROM [db_educacao.rede.sp].[se1426].[dbo].[serie_turma_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]
GO

INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]
SELECT * FROM [db_educacao.rede.sp].[se1426].[dbo].[escola_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular]
GO

INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular]
SELECT * FROM [db_educacao.rede.sp].[se1426].[dbo].[grade_componente_curricular] 
GO



-- CORRIGINDO DISCIPLINAS DE REGÊNCIA DE CLASSE
TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_508]
GO
 
INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento_508]
SELECT DISTINCT
       ETL.[TURMA_ID],
       ETL.[PERIODO_ESCOLAR_ID],
       ETL.[CRIADO_EM],
       ETL.[ALTERADO_EM],
       GCC.cd_componente_curricular AS [DISCIPLINA_ID],
       ETL.[ALUNO_CODIGO],
       ETL.[NOTA],
	   ETL.[SINTESE_ID],
	   ETL.[CONCEITO_ID]

  FROM            [Manutencao].[dbo].[ETL_SGP_Fechamento]                             AS ETL (NOLOCK) 
     --INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_turma_grade]                    AS STG (NOLOCK) 
       INNER JOIN [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]           AS STG (NOLOCK) 
               ON STG.cd_turma_escola = ETL.TURMA_ID
              AND STG.dt_fim IS NULL
     --INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                         AS EGR (NOLOCK)
       INNER JOIN [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]                AS EGR (NOLOCK)
               ON EGR.cd_escola_grade = STG.cd_escola_grade
     --INNER JOIN [10.49.16.60].[se1426].[dbo].[grade_componente_curricular]          AS GCC (NOLOCK)
       INNER JOIN [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular] AS GCC (NOLOCK) 
               ON GCC.cd_grade = EGR.cd_grade
              AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

 WHERE ETL.DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
GO

DELETE
  FROM [Manutencao].[dbo].[ETL_SGP_Fechamento]
 WHERE [DISCIPLINA_ID] IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
GO

INSERT INTO [Manutencao].[dbo].[ETL_SGP_Fechamento]
SELECT DISTINCT
       [TURMA_ID],
       [PERIODO_ESCOLAR_ID],
       [CRIADO_EM],
       [ALTERADO_EM],
       [DISCIPLINA_ID],
       [ALUNO_CODIGO],
       [NOTA],
	   [SINTESE_ID],
	   [CONCEITO_ID]
  FROM [Manutencao].[dbo].[ETL_SGP_Fechamento_508]
GO



-- CORRIGINDO DISCIPLINAS DE INGLÊS
UPDATE [Manutencao].[dbo].[ETL_SGP_Fechamento]
   SET [DISCIPLINA_ID] = 9
 WHERE [DISCIPLINA_ID] = 1046
GO



-- Zerando as tabelas do GATO (para não ocupar espaço na base)
TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_serie_turma_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_escola_grade]
GO

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_Fechamento_grade_componente_curricular]
GO
