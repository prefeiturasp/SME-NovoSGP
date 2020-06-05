 USE GestaoPedagogica
 GO


 TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]
 GO

 TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]
 GO

 TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
 GO

 TRUNCATE TABLE [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS] 
 GO


 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]
 SELECT DISTINCT
        TUR.tur_codigoEOL           AS [TURMA_ID],
        TMP.IdDisciplina            AS [DISCIPLINA_ID],
		TUR.tur_id,
		TTD.tud_id,
		ESC.esc_id,
		ESC.esc_codigo,
		DIS.dis_id,
		TMP.NmDisciplina,
		TAU.usu_id,
		TAU.usu_idDocenteAlteracao,
		USU.usu_login,
		TAU.tau_data

   FROM            [GestaoPedagogica].[dbo].[TUR_Turma]                                AS TUR (NOLOCK)
        INNER JOIN [GestaoPedagogica].[dbo].[TUR_TurmaRelTurmaDisciplina]              AS TTD (NOLOCK)
                ON TTD.tur_id = TUR.tur_id
        INNER JOIN [GestaoPedagogica].[dbo].[TUR_TurmaDisciplina]                      AS TDI (NOLOCK)
                ON TDI.tud_id = TTD.tud_id
        INNER JOIN [GestaoPedagogica].[dbo].[CLS_TurmaAula]                            AS TAU (NOLOCK)
		        ON TAU.tud_id = TTD.tud_id
        INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]                               AS ESC (NOLOCK)
                ON ESC.esc_id = TUR.esc_id
        INNER JOIN [GestaoPedagogica].[dbo].[TUR_TurmaDisciplinaRelDisciplina]         AS TDD (NOLOCK)
                ON TDD.tud_id = TAU.tud_id
        INNER JOIN [GestaoPedagogica].[dbo].[ACA_Disciplina]                           AS DIS (NOLOCK)
                ON DIS.dis_id = TDD.dis_id
        INNER JOIN [Manutencao].[dbo].[ETL_SGP_DISCIPLINAS]                            AS TMP (NOLOCK)
                ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS VARCHAR) COLLATE Latin1_General_CI_AS)
        INNER JOIN [CORESSO].[DBO].[SYS_Usuario]                                       AS USU (NOLOCK)
                ON USU.usu_id = TAU.usu_id
			   AND USU.ent_id = ESC.ent_id

  WHERE YEAR(DIS.dis_dataCriacao) BETWEEN 2014 AND 2019
    AND TUR.tur_codigoEOL IS NOT NULL
	AND TAU.tau_situacao <> 3
	AND TAU.usu_id = USU.usu_id
	--AND YEAR(TAU.tau_data) = 2019              -- <= TIRAR
	--AND ESC.esc_codigo IN ('019455','094765')  -- <= TIRAR



 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]
 SELECT DISTINCT
        PT1.[DISCIPLINA_ID]                       AS [DISCIPLINA_ID], 
		EDU.cd_unidade_administrativa_referencia  AS [DRE_ID],
		AUL.cd_unidade_educacao                   AS [UE_ID], 
		SRV.cd_registro_funcional                 AS [PROFESSOR_RF], 
        PT1.[TURMA_ID]                            AS [TURMA_ID], 		
		CASE 
			WHEN ETE.cd_etapa_ensino IN (2,3,7,11)	    THEN 3 -- EJA
			WHEN ETE.cd_etapa_ensino IN (4,5,12,13)	    THEN 5 -- Fundamental
			WHEN ETE.cd_etapa_ensino IN (6,8,9,14,17)   THEN 6 -- Médio
        END                                       AS [MODALIDADE]

   FROM            [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P1]                  AS PT1 (NOLOCK)
        INNER JOIN [10.49.16.60].[se1426].[dbo].[v_servidor_cotic]                 AS SRV (NOLOCK)
                ON SRV.cd_registro_funcional COLLATE Latin1_General_CI_AS = PT1.usu_login
        INNER JOIN [10.49.16.60].[se1426].[dbo].[v_cargo_base_cotic]               AS CRG (NOLOCK)
                ON CRG.cd_servidor = SRV.cd_servidor
		INNER JOIN [10.49.16.60].[se1426].[dbo].[atribuicao_aula]                  AS AUL (NOLOCK)
                ON AUL.cd_cargo_base_servidor = CRG.cd_cargo_base_servidor
			   AND AUL.an_atribuicao = YEAR(PT1.tau_data)
			   AND AUL.cd_serie_grade IS NOT NULL
		INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_turma_grade]			       AS STG (NOLOCK) 
		        ON STG.cd_serie_grade = AUL.cd_serie_grade
			   AND STG.dt_fim IS NULL
			   AND STG.cd_escola COLLATE Latin1_General_CI_AS = PT1.esc_codigo
			   AND STG.cd_turma_escola = PT1.[TURMA_ID]
		INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                     AS EGR (NOLOCK) 
		        ON EGR.cd_grade = AUL.cd_grade
			   AND EGR.cd_escola COLLATE Latin1_General_CI_AS = PT1.esc_codigo
               AND EGR.cd_escola_grade = STG.cd_escola_grade
		INNER JOIN [10.49.16.60].[se1426].[dbo].[grade_componente_curricular]      AS GCC (NOLOCK) 
		        ON GCC.cd_grade = AUL.cd_grade
			   AND GCC.cd_componente_curricular = AUL.cd_componente_curricular
		INNER JOIN [10.49.16.60].[se1426].[dbo].[v_cadastro_unidade_educacao]      AS EDU (NOLOCK)
		        ON EDU.cd_unidade_educacao COLLATE Latin1_General_CI_AS = PT1.esc_codigo
		INNER JOIN [10.49.16.60].[se1426].[dbo].[grade]                            AS GRD (NOLOCK) 
		        ON GRD.cd_grade = AUL.cd_grade
			   AND GRD.cd_tipo_turno = AUL.cd_tipo_turno
			   AND GRD.cd_serie_ensino = STG.cd_serie_ensino
		INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_ensino]                     AS SEN (NOLOCK) 
		        ON SEN.cd_serie_ensino = STG.cd_serie_ensino
		INNER JOIN [10.49.16.60].[se1426].[dbo].[etapa_ensino]                     AS ETE (NOLOCK) 
		        ON ETE.cd_modalidade_ensino = SEN.cd_modalidade_ensino
			   AND ETE.cd_etapa_ensino = SEN.cd_etapa_ensino
        INNER JOIN [CORESSO].[DBO].[SYS_Usuario]                                   AS USU (NOLOCK)
                ON USU.usu_login COLLATE Latin1_General_CI_AS = SRV.cd_registro_funcional
				
  WHERE PT1.tau_data NOT BETWEEN AUL.dt_atribuicao_aula AND AUL.dt_disponibilizacao_aulas
    AND AUL.dt_disponibilizacao_aulas IS NOT NULL
	AND AUL.dt_cancelamento IS NULL
	AND AUL.dt_disponibilizacao_aulas >= AUL.dt_atribuicao_aula
	AND ETE.cd_etapa_ensino IN (2,3,4,5,6,7,8,9,11,12,13,14,17)
	AND SRV.cd_registro_funcional IS NOT NULL
    --AND AUL.an_atribuicao = 2019               -- <= TIRAR
	AND ( (PT1.usu_idDocenteAlteracao = USU.usu_id) OR
	      (PT1.usu_idDocenteAlteracao IS NULL AND PT1.usu_id = USU.usu_id) )


-- Corrigindo disciplinas de Regência de Classe

 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508]
 SELECT GCC.cd_componente_curricular AS [DISCIPLINA_ID],
		PT2.[DRE_ID],
		PT2.[UE_ID],
		PT2.[PROFESSOR_RF], 
        PT2.[TURMA_ID], 
        PT2.[MODALIDADE]

   FROM            [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]              AS PT2 (NOLOCK)
		INNER JOIN [10.49.16.60].[se1426].[dbo].[serie_turma_grade]           AS STG (NOLOCK) 
				ON STG.cd_turma_escola = PT2.TURMA_ID
			   AND STG.dt_fim IS NULL
		INNER JOIN [10.49.16.60].[se1426].[dbo].[escola_grade]                AS EGR (NOLOCK) 
				ON EGR.cd_escola_grade = STG.cd_escola_grade
		INNER JOIN [10.49.16.60].[se1426].[dbo].[grade_componente_curricular] AS GCC (NOLOCK) 
				ON GCC.cd_grade = EGR.cd_grade
			   AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

  WHERE PT2.DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)



 DELETE
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2]
  WHERE DISCIPLINA_ID IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)



 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2] 
 SELECT [DISCIPLINA_ID], [DRE_ID], [UE_ID], [PROFESSOR_RF], [TURMA_ID], [MODALIDADE]
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_508] (NOLOCK)



 INSERT INTO [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_DADOS]
 SELECT [DISCIPLINA_ID], [DRE_ID], [UE_ID], [PROFESSOR_RF], [TURMA_ID], [MODALIDADE]
   FROM [Manutencao].[dbo].[ETL_SGP_ATRIBUICAOCJ_P2] 



