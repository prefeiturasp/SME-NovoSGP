USE GestaoPedagogica
GO


IF OBJECT_ID('tempdb..#AULA_PREVISTA_GERAL') is not null
	DROP TABLE #AULA_PREVISTA_GERAL
IF OBJECT_ID('tempdb..#AULA_PREVISTA_GERAL508') is not null
	DROP TABLE #AULA_PREVISTA_GERAL508
IF OBJECT_ID('[Manutencao].[dbo].[AULA_PREVISTA]') is not null
	DROP TABLE [Manutencao].[dbo].[AULA_PREVISTA]
IF OBJECT_ID('[Manutencao].[dbo].[AULA_PREVISTA_BIMESTRE]') is not null
	DROP TABLE [Manutencao].[dbo].[AULA_PREVISTA_BIMESTRE]

--01) Criando tabela temporária para Aulas Previstas/Bimestre
		CREATE TABLE #AULA_PREVISTA_GERAL (
			aulas_previstas int NOT NULL,
			bimestre int NOT NULL,
			tipo_calendario_id int NOT NULL,
			disciplina_id varchar(15) NOT NULL,
			turma_id varchar(15) NOT NULL,
			criado_em datetime NOT NULL,
			criado_por varchar(200) NOT NULL,
			alterado_em datetime NULL,
			alterado_por varchar(200) NULL,
			criado_rf varchar(200) NOT NULL,
			alterado_rf varchar(200) NULL,
			excluido bit NOT NULL DEFAULT 0,
			migrado BIT NOT NULL DEFAULT 1
		)

-- 02) Criando tabela temporária para Aulas Previstas/Bimestre para tratamento das Regência de Classe
		CREATE TABLE #AULA_PREVISTA_GERAL508 (
			aulas_previstas int NOT NULL,
			bimestre int NOT NULL,
			tipo_calendario_id int NOT NULL,
			disciplina_id varchar(15) NOT NULL,
			turma_id varchar(15) NOT NULL,
			criado_em datetime NOT NULL,
			criado_por varchar(200) NOT NULL,
			alterado_em datetime NULL,
			alterado_por varchar(200) NULL,
			criado_rf varchar(200) NOT NULL,
			alterado_rf varchar(200) NULL,
			excluido bit NOT NULL DEFAULT 0,
			migrado BIT NOT NULL DEFAULT 1
		)

-- 03) Criando tabela temporária #AULAS_PREVISTAS
		CREATE TABLE [Manutencao].[dbo].[AULA_PREVISTA] (
			id int NOT NULL IDENTITY (1,1),
			tipo_calendario_id int NOT NULL,
			disciplina_id varchar(15) NOT NULL,
			turma_id varchar(15) NOT NULL,
			criado_em datetime NOT NULL,
			criado_por varchar(200) NOT NULL,
			alterado_em datetime NULL,
			alterado_por varchar(200) NULL,
			criado_rf varchar(200) NOT NULL,
			alterado_rf varchar(200) NULL,
			excluido varchar(10) NOT NULL DEFAULT 0,
			migrado varchar(10) NOT NULL,
		)
-- 04) Criando tabela temporária #AULAS_PREVISTAS_BIMESTRE
		CREATE TABLE [Manutencao].[dbo].[AULA_PREVISTA_BIMESTRE] (
			id int NOT NULL IDENTITY (1,1),
			aula_prevista_id int NOT NULL,
			aulas_previstas int NOT NULL,
			bimestre int NOT NULL,
			criado_em datetime NOT NULL,
			criado_por varchar(200) NOT NULL,
			alterado_em datetime NULL,
			alterado_por varchar(200) NULL,
			criado_rf varchar(200) NOT NULL,
			alterado_rf varchar(200) NULL,
			excluido varchar(10) NOT NULL DEFAULT 0,
			migrado varchar(10) NOT NULL DEFAULT 1,
		)


-- 05) Populando a tabela temporária #AULA_PREVISTA_GERAL
		INSERT INTO #AULA_PREVISTA_GERAL
		SELECT 
			   TDAP.tap_aulasPrevitas AS [aulas_previstas],
			   TDAP.tpc_id AS [bimestre],			   
			   CASE YEAR(TDI.tud_dataInicio) 
					WHEN 2014 THEN 1
					WHEN 2015 THEN 2
					WHEN 2016 THEN 3
					WHEN 2017 THEN 4
					WHEN 2018 THEN 7
					WHEN 2019 THEN 10
			   END AS [tipo_calendario_id],
			   TMP.IdDisciplina AS [disciplina_id],
			   TUR.tur_codigoEOL AS [turma_id],
			   TDAP.tap_dataCriacao AS [criado_em],
			   SUBSTRING(RTRIM(LTRIM(ISNULL(PE1.pes_nome,'SISTEMA'))),1,200) AS [criado_por],
			   TDAP.tap_dataAlteracao AS [alterado_em],
			   SUBSTRING(RTRIM(LTRIM(PE2.pes_nome)),1,200) AS [alterado_por],
			   RIGHT([dbo].[fn_TiraLetras](RTRIM(LTRIM(ISNULL(US1.usu_login,'SISTEMA')))),7) AS [criado_rf], 
			   RIGHT([dbo].[fn_TiraLetras](RTRIM(LTRIM(US2.usu_login))),7) AS [alterado_rf], 
			   'FALSE' AS [excluido],
			   'TRUE' AS [migrado]
          FROM TUR_TurmaDisciplinaAulaPrevista	AS TDAP (NOLOCK)           
			INNER JOIN TUR_TurmaDisciplinaRelDisciplina         AS TDD (NOLOCK)
                ON TDD.tud_id = TDAP.tud_id
			INNER JOIN TUR_TurmaDisciplina                      AS TDI (NOLOCK)
                       ON TDI.tud_id = TDD.tud_id
			INNER JOIN TUR_TurmaRelTurmaDisciplina              AS TTD (NOLOCK)
				ON TTD.tud_id = TDD.tud_id
			INNER JOIN TUR_Turma                                AS TUR (NOLOCK)
                ON TUR.tur_id = TTD.tur_id
			INNER JOIN ACA_Disciplina								AS DIS (NOLOCK)
                ON DIS.dis_id = TDD.dis_id
			INNER JOIN ESC_Escola                               AS ESC (NOLOCK)
                ON ESC.esc_id = TUR.esc_id
			INNER JOIN [Manutencao].[dbo].[ETL_SGP_DISCIPLINAS] AS TMP (NOLOCK)
                ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS VARCHAR) COLLATE Latin1_General_CI_AS)
			OUTER APPLY
			(
				SELECT TOP 1 usr.usu_id, usr.usu_login, usr.pes_id, ta.usu_idDocenteAlteracao
				FROM [CoreSSO].[dbo].[SYS_Usuario] usr 
					INNER JOIN  CLS_TurmaAula TA 
						ON usr.usu_id = TA.usu_id
					WHERE TA.tud_id = TDD.tud_id
			) AS US1
			LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]      AS PE1 (NOLOCK)
					ON PE1.pes_id = US1.pes_id
			LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]     AS US2 (NOLOCK)
					ON US2.usu_id = US1.usu_idDocenteAlteracao
			LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]      AS PE2 (NOLOCK)
					ON PE2.pes_id = US2.pes_id
			WHERE  TDI.tud_dataInicio >= '2019-01-01' AND TDI.tud_dataFim <= '2019-12-31'
			AND UPPER(DIS.dis_nome) <> 'CONCEITO GLOBAL (INFANTIL I E II)' -- INFANTIL não será migrado
			--AND ESC.esc_codigo IN ('019455','094765')

-- 06) Busca as Disciplinas de Regência de Classe no EOL, e insere na tabela temporária #AULA_PREVISTA_GERAL508

		INSERT INTO #AULA_PREVISTA_GERAL508
		SELECT AUL.aulas_previstas, AUL.bimestre,AUL.tipo_calendario_id, GCC.cd_componente_curricular AS [disciplina_id], AUL.turma_id, 
			   AUL.criado_em, AUL.criado_por, AUL.alterado_em, AUL.alterado_por, 
			   AUL.criado_rf, AUL.alterado_rf, AUL.excluido, AUL.migrado

		  FROM            #AULA_PREVISTA_GERAL AS AUL (NOLOCK)
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].serie_turma_grade AS STG (NOLOCK) 
					   ON STG.cd_turma_escola = AUL.turma_id 
					  AND STG.dt_fim IS NULL
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].escola_grade AS EGR (NOLOCK) 
					   ON EGR.cd_escola_grade = STG.cd_escola_grade
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].grade_componente_curricular AS GCC (NOLOCK) 
					   ON GCC.cd_grade = EGR.cd_grade
					  AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

		 WHERE AUL.disciplina_id IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)

		 ORDER BY  
				  AUL.turma_id, 
				  GCC.cd_componente_curricular



-- 07) Apaga as Disciplinas de Regência de Classe (508) da tabela temporária ##AULA_PREVISTA_GERAL (as corretas estão na ##AULA_PREVISTA_GERAL508)
		 DELETE FROM #AULA_PREVISTA_GERAL
		  WHERE disciplina_id IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)


-- 08) Insere as Disciplinas de Regência de Classe na tabela temporária #PLANO_ANUAL à partir da #AULA508

		 INSERT INTO #AULA_PREVISTA_GERAL
		 SELECT AUL.aulas_previstas, AUL.bimestre, AUL.tipo_calendario_id, AUL.disciplina_id, AUL.turma_id, 
			    AUL.criado_em, AUL.criado_por, AUL.alterado_em, AUL.alterado_por, 
			    AUL.criado_rf, AUL.alterado_rf, AUL.excluido, AUL.migrado
		   FROM #AULA_PREVISTA_GERAL508 AS AUL


---- 09) Ajustes

	UPDATE #AULA_PREVISTA_GERAL
	   SET alterado_por = criado_por
	 WHERE alterado_em IS NOT NULL
	   AND alterado_por IS NULL

	UPDATE #AULA_PREVISTA_GERAL
	   SET alterado_rf = criado_rf
	 WHERE alterado_por IS NOT NULL
	   AND alterado_rf IS NULL

------ 10) Montando os CSVs
------- AULAS_PREVISTAS
		INSERT INTO [Manutencao].[dbo].[AULA_PREVISTA]
		SELECT 
			   AUL.tipo_calendario_id, AUL.disciplina_id, AUL.turma_id,
		       MAX(AUL.criado_em) AS criado_em, AUL.criado_por, MAX(AUL.alterado_em) AS alterado_em, AUL.alterado_por, AUL.criado_rf, AUL.alterado_rf, 
			   CASE WHEN AUL.excluido = 0 THEN 'FALSE'
			                              ELSE 'TRUE'
			   END AS excluido, 
			   CASE WHEN AUL.migrado = 0 THEN 'FALSE'
			                             ELSE 'TRUE'
			   END AS migrado
		  FROM #AULA_PREVISTA_GERAL AS AUL
		  GROUP BY  AUL.tipo_calendario_id, AUL.disciplina_id, AUL.turma_id,
		       AUL.criado_por, AUL.alterado_por, AUL.criado_rf, AUL.alterado_rf,AUL.excluido,AUL.migrado

------- AULAS_PREVISTAS BIMESTRE
		INSERT INTO [Manutencao].[dbo].[AULA_PREVISTA_BIMESTRE]
		SELECT 
			AP.id AS aula_prevista_id, AUL.aulas_previstas, AUL.bimestre,
			AUL.criado_em, AUL.criado_por, AUL.alterado_em, AUL.alterado_por, AUL.criado_rf, AUL.alterado_rf, 
			CASE WHEN AUL.excluido = 0 THEN 'FALSE'
										ELSE 'TRUE'
			END AS excluido, 
			CASE WHEN AUL.migrado = 0 THEN 'FALSE'
										ELSE 'TRUE'
			END AS migrado
		FROM #AULA_PREVISTA_GERAL AS AUL
		INNER JOIN [Manutencao].[dbo].[AULA_PREVISTA] AP 
			ON AUL.turma_id = AP.turma_id 
				AND AUL.disciplina_id = AP.disciplina_id
				AND AUL.tipo_calendario_id = AP.tipo_calendario_id
		ORDER BY aula_prevista_id
