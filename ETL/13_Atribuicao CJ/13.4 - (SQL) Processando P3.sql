 USE GestaoPedagogica
 GO


 TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]
 GO

 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]
 SELECT DISTINCT
        PT1.[DISCIPLINA_ID]                       AS [DISCIPLINA_ID], 
		EDU.cd_unidade_administrativa_referencia  AS [DRE_ID],
		AUL.cd_unidade_educacao                   AS [UE_ID], 
		SRV.cd_registro_funcional                 AS [PROFESSOR_RF], 
        PT1.[TURMA_ID]                            AS [TURMA_ID], 	
		CASE 
			WHEN SEN.cd_etapa_ensino IN (2,3,7,11)	    THEN 3 -- EJA 
			WHEN SEN.cd_etapa_ensino IN (4,5,12,13)	    THEN 5 -- Fundamental 
			WHEN SEN.cd_etapa_ensino IN (6,8,9,14,17)   THEN 6 -- Médio
        END                                       AS [MODALIDADE]
		
   FROM            [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]                   AS PT1 (NOLOCK)
        INNER JOIN [10.49.16.60].[se1426].[dbo].[v_servidor_cotic]                AS SRV (NOLOCK)
                ON SRV.cd_registro_funcional COLLATE Latin1_General_CI_AS = PT1.usu_login
			   AND SRV.cd_registro_funcional IS NOT NULL
        INNER JOIN [10.49.16.60].[se1426].[dbo].[v_cargo_base_cotic]              AS CRG (NOLOCK)
                ON CRG.cd_servidor = SRV.cd_servidor
		INNER JOIN [10.49.16.60].[se1426].[dbo].[atribuicao_aula]                 AS AUL (NOLOCK)
                ON AUL.cd_cargo_base_servidor = CRG.cd_cargo_base_servidor
			   AND AUL.cd_unidade_educacao COLLATE Latin1_General_CI_AS = PT1.ESC_CODIGO
			   AND AUL.an_atribuicao = YEAR(PT1.tau_data)
			   AND AUL.cd_serie_grade IS NULL
			   AND AUL.dt_cancelamento IS NULL
			   AND AUL.dt_disponibilizacao_aulas IS NOT NULL
			   AND AUL.dt_disponibilizacao_aulas >= AUL.dt_atribuicao_aula
			   AND AUL.an_atribuicao = 2019
		--INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                    AS EGR (NOLOCK) 
		--		ON EGR.cd_grade = AUL.cd_grade
		--		AND EGR.cd_escola COLLATE Latin1_General_CI_AS = PT1.esc_codigo
		--INNER JOIN [10.49.16.60].[se1426].[dbo].[turma_escola]                    AS TES (NOLOCK) 
		--		ON TES.cd_escola COLLATE Latin1_General_CI_AS = PT1.esc_codigo
		--		AND TES.an_letivo = AUL.an_atribuicao
		INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_escola]                    AS SES (NOLOCK) 
		        ON SES.cd_escola COLLATE Latin1_General_CI_AS = PT1.esc_codigo
		INNER JOIN [10.49.16.60].[se1426].[dbo].[v_cadastro_unidade_educacao]     AS EDU (NOLOCK)
		        ON EDU.cd_unidade_educacao COLLATE Latin1_General_CI_AS = PT1.esc_codigo
        INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_ensino]                    AS SEN (NOLOCK) 
		        ON SEN.cd_serie_ensino = SES.cd_serie_ensino
			   AND SEN.cd_etapa_ensino IN (2,3,4,5,6,7,8,9,11,12,13,14,17) 
        INNER JOIN [CORESSO].[DBO].[SYS_Usuario]                                  AS USU (NOLOCK)
                ON USU.usu_login COLLATE Latin1_General_CI_AS = SRV.cd_registro_funcional

  WHERE SRV.cd_registro_funcional COLLATE Latin1_General_CI_AS NOT IN (SELECT DISTINCT PROFESSOR_RF FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS] WHERE TURMA_ID =  PT1.[TURMA_ID] AND PROFESSOR_RF = SRV.cd_registro_funcional COLLATE Latin1_General_CI_AS)
	--AND SRV.cd_registro_funcional IN ('8055611') -- <= TIRAR
	AND ( (PT1.usu_idDocenteAlteracao = USU.usu_id) OR
	      (PT1.usu_idDocenteAlteracao IS NULL AND PT1.usu_id = USU.usu_id) )
	--AND AUL.cd_unidade_educacao IN ('019455','094765')  -- <= TIRAR
  GROUP BY PT1.[DISCIPLINA_ID], EDU.cd_unidade_administrativa_referencia, AUL.cd_unidade_educacao, SRV.cd_registro_funcional, PT1.[TURMA_ID], SEN.cd_etapa_ensino


-- Corrigindo disciplinas de Regência de Classe

TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
GO


 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
 SELECT GCC.cd_componente_curricular AS [DISCIPLINA_ID],
		PT3.[DRE_ID],
		PT3.[UE_ID],
		PT3.[PROFESSOR_RF], 
        PT3.[TURMA_ID], 
        PT3.[MODALIDADE]--,
        --PT3.[DATA]

   FROM            [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]               AS PT3 (NOLOCK)
		INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_turma_grade]           AS STG (NOLOCK) 
				ON STG.cd_turma_escola = PT3.TURMA_ID
			   AND STG.dt_fim IS NULL
		INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                AS EGR (NOLOCK) 
				ON EGR.cd_escola_grade = STG.cd_escola_grade
		INNER JOIN [10.49.16.60].[se1426].[dbo].[grade_componente_curricular] AS GCC (NOLOCK) 
				ON GCC.cd_grade = EGR.cd_grade
			   AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

  WHERE PT3.DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)


 DELETE
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3]
  WHERE DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)


 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3] 
 SELECT [DISCIPLINA_ID], [DRE_ID], [UE_ID], [PROFESSOR_RF], [TURMA_ID], [MODALIDADE]
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508] (NOLOCK)


 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2]
 SELECT [DISCIPLINA_ID], [DRE_ID], [UE_ID], [PROFESSOR_RF], [TURMA_ID], [MODALIDADE]
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P3] 


DELETE
  FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2] 
 WHERE DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1115, 1117, 1121, 1124, 1211, 1212, 1213, 1290, 1301)
   AND MODALIDADE = 3

    
DELETE FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
 WHERE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS].TURMA_ID IN
		 (SELECT TURMA_ID
			FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
		   WHERE PROFESSOR_RF = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS].PROFESSOR_RF
			 AND TURMA_ID = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS].TURMA_ID
			 AND DISCIPLINA_ID = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS].DISCIPLINA_ID
		   GROUP BY TURMA_ID
		  HAVING COUNT(MODALIDADE) > 1)
   AND [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS].MODALIDADE = 3


DELETE FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2]
 WHERE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].TURMA_ID IN
		 (SELECT TURMA_ID
			FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2]
		   WHERE PROFESSOR_RF = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].PROFESSOR_RF
			 AND TURMA_ID = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].TURMA_ID
			 AND DISCIPLINA_ID = [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].DISCIPLINA_ID
		   GROUP BY TURMA_ID
		  HAVING COUNT([Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].MODALIDADE) > 1)
   AND [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2].MODALIDADE = 3


 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
 SELECT [DISCIPLINA_ID], [DRE_ID], [UE_ID], [PROFESSOR_RF], [TURMA_ID], [MODALIDADE]
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS2]
   
