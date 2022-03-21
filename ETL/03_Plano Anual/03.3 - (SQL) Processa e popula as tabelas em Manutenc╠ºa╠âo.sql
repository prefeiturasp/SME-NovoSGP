-- #######################################################################
-- ##                                                                   ##
-- ##                       P L A N O   A N U A L                       ##
-- ##                                                                   ##
-- #######################################################################

USE GestaoPedagogica
GO

-- 1) Populando a tabela [ETL_SGP_PLANO_ANUAL]
		 INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
		 SELECT ESC.esc_codigo AS [escola_id],
				TUR.tur_codigoEOL AS [turma_id],
				YEAR(tdp_dataCriacao) AS [ano],
				TDP.tpc_id AS [bimestre],
				CASE WHEN DATALENGTH(TDP.tdp_planejamento) IS NULL THEN 'Migrado - Não informado no legado.'
					 WHEN DATALENGTH(TDP.tdp_planejamento) <= 5    THEN 'Migrado - Não informado no legado.'
																   ELSE TDP.tdp_planejamento
				END AS [descricao],
				TDP.tdp_dataCriacao AS [criado_em],
				'Migração ETL' AS [criado_por],
				TDP.tdp_dataAlteracao AS [alterado_em],
				'Migração ETL' AS [alterado_por],
				'0' AS [criado_rf],
				'0' AS [alterado_rf],
				'TRUE' AS [migrado],
				TMP.IdDisciplina AS [componente_curricular_eol_id]
		   FROM            CLS_TurmaDisciplinaPlanejamento         AS TDP (NOLOCK)
				INNER JOIN TUR_TurmaDisciplina                     AS TDI (NOLOCK)
						ON TDI.tud_id = TDP.tud_id
				INNER JOIN TUR_TurmaRelTurmaDisciplina             AS TTD (NOLOCK)
						ON TTD.tud_id = TDP.tud_id
				INNER JOIN TUR_Turma                               AS TUR (NOLOCK)
						ON TUR.tur_id = TTD.tur_id
				INNER JOIN ESC_Escola                              AS ESC (NOLOCK)
						ON ESC.esc_id = TUR.esc_id
				INNER JOIN TUR_TurmaDisciplinaRelDisciplina        AS TDD (NOLOCK)
						ON TDD.tud_id = TDP.tud_id
				INNER JOIN ACA_Disciplina                          AS DIS (NOLOCK)
						ON DIS.dis_id = TDD.dis_id
				INNER JOIN [Manutencao].[DBO].[ETL_SGP_DISCIPLINAS] AS TMP (NOLOCK)
						ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS varchar) COLLATE Latin1_General_CI_AS)
		  WHERE TDP.tpc_id IN (1,2,3,4)  -- Bimestres
			AND TDP.tdp_situacao = 1     -- Trazendo só os ativos da tabela CLS_TurmaDisciplinaPlanejamento
			AND TUR.tur_situacao = 1     -- Trazendo só os ativos da tabela TUR_Turma
			AND ESC.esc_situacao = 1     -- Trazendo só os ativos da tabela ESC_Escola
			AND DIS.dis_situacao = 1     -- Trazendo só os ativos da tabela ACA_Disciplina
			AND UPPER(DIS.dis_nome) <> 'CONCEITO GLOBAL (INFANTIL I E II)' -- INFANTIL não será migrado
			AND TUR.tur_codigoEOL IS NOT NULL
			and YEAR(tdp_dataCriacao) = 2019
		  ORDER BY ESC.esc_codigo,
				   TUR.tur_codigoEOL,
				   YEAR(tdp_dataCriacao),
				   TMP.IdDisciplina,
				   TDP.tpc_id


-- 2) Busca as Disciplinas de Regência de Classe no EOL e insere na tabela [ETL_SGP_PLANO_ANUAL508]
		 INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL508]
		 SELECT PLA.id, PLA.escola_id, PLA.turma_id, PLA.ano, PLA.bimestre, PLA.descricao, 
				PLA.criado_em, PLA.criado_por, PLA.alterado_em, PLA.alterado_por, PLA.criado_rf, PLA.alterado_rf, 
				PLA.migrado, GCC.cd_componente_curricular AS [componente_curricular_eol_id]
		   FROM            [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]                         AS PLA (NOLOCK)
				INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].serie_turma_grade           AS STG (NOLOCK) 
						ON STG.cd_turma_escola = PLA.turma_id 
					   AND STG.dt_fim IS NULL
				INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].escola_grade                AS EGR (NOLOCK) 
						ON EGR.cd_escola_grade = STG.cd_escola_grade
				INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].grade_componente_curricular AS GCC (NOLOCK) 
						ON GCC.cd_grade = EGR.cd_grade
					   AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
		  WHERE PLA.componente_curricular_eol_id = 508
		  ORDER BY PLA.escola_id, 
				   PLA.turma_ID, 
				   PLA.ano, 
				   PLA.bimestre


-- 3) Apaga as Disciplinas de Regência de Classe (508) da tabela [ETL_SGP_PLANO_ANUAL] (as corretas estão na tabela [ETL_SGP_PLANO_ANUAL508])
		 DELETE 
		   FROM [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
		  WHERE componente_curricular_eol_id = 508


-- 4) Insere as Disciplinas de Regência de Classe na tabela [ETL_SGP_PLANO_ANUAL] à partir da tabela [ETL_SGP_PLANO_ANUAL508]
		 INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
		 SELECT PLA.escola_id, PLA.turma_id, PLA.ano, PLA.bimestre, 
				PLA.descricao, PLA.criado_em, PLA.criado_por, PLA.alterado_em, PLA.alterado_por,
				PLA.criado_rf, PLA.alterado_rf, PLA.migrado, PLA.componente_curricular_eol_id 
		   FROM [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL508] AS PLA


-- 5) Tirando os duplicados
		 SELECT escola_id, turma_id, ano, bimestre, componente_curricular_eol_id
		   INTO #DUPLICADOS
		   FROM [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
		  GROUP BY escola_id, turma_id, ano, bimestre, componente_curricular_eol_id
		 HAVING COUNT(ID) > 1

		 DELETE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
		   FROM            [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL] AS PLA (NOLOCK)
				INNER JOIN #DUPLICADOS                              AS DUP (NOLOCK) ON DUP.escola_id = PLA.escola_id
																				   AND DUP.turma_id = PLA.turma_id
									                             				   AND DUP.ano = PLA.ano
										                             			   AND DUP.bimestre = PLA.bimestre
									                             				   AND DUP.componente_curricular_eol_id = PLA.componente_curricular_eol_id


-- 6) Ajustes
		 UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
 			SET descricao = dbo.fc_LimpaHTML(descricao)

		 UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
			SET descricao = LTRIM(RTRIM(descricao))

		 UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
			SET descricao = NULL
		  WHERE (descricao = '') OR (descricao = ' ') OR (descricao = '  ')

		 UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
			SET descricao = 'Migrado - Não informado no legado.'
		  WHERE descricao IS NULL

		 UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
			SET descricao = LEFT(descricao, 30000)
		  WHERE LEN(descricao) >= 30000

