-- #######################################################################
-- ##                                                                   ##
-- ##                            A U L A                                ##
-- ##                               +                                   ##
-- ##                   P L A N O   D E   A U L A                       ##
-- ##                                                                   ##
-- #######################################################################

USE GestaoPedagogica
GO


-- 01) Populando a tabela [ETL_SGP_(AULA+PLANO_AULA)_DADO]
        INSERT INTO [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		SELECT -- Aula --
		       ESC.esc_codigo AS [AUL_ue_id],
			   TMP.IdDisciplina AS [AUL_disciplina_id],
			   TUR.tur_codigoEOL AS [AUL_turma_id],
			   CASE YEAR(TAU.tau_data) 
					WHEN 2014 THEN 1
					WHEN 2015 THEN 2
					WHEN 2016 THEN 3
					WHEN 2017 THEN 4
					WHEN 2018 THEN 7
					WHEN 2019 THEN 10
			   END AS [AUL_tipo_calendario_id],
			   CASE WHEN US1.usu_login IS NULL THEN '0'
			                                   ELSE US1.usu_login 
			   END AS [AUL_professor_rf],
			   TAU.tau_numeroaulas AS [AUL_quantidade],
			   TAU.tau_data AS [AUL_data_aula],
			   1 AS [AUL_recorrencia_aula],
			   CASE TAU.tau_reposicao WHEN 0 THEN 1
									  WHEN 1 THEN 2
											 ELSE 0
			   END AS [AUL_tipo_aula],
			   TAU.tau_dataCriacao AS [AUL_criado_em],
			   CASE WHEN PE1.pes_nome IS NULL THEN 'Migrado - Não informado no legado.'
			                                  ELSE PE1.pes_nome 
			   END AS [AUL_criado_por],
			   TAU.tau_dataAlteracao AS [AUL_alterado_em],
			   PE2.pes_nome AS [AUL_alterado_por],
			   CASE WHEN US1.usu_login IS NULL THEN '0'
			                                   ELSE US1.usu_login 
			   END AS [AUL_criado_rf], 
			   US2.usu_login AS [AUL_alterado_rf],
			   -- Plano de Aula --
			   TAU.tau_planoAula AS [PLA_descricao],
			   CASE WHEN TAU.tau_sintese IS NULL THEN 'Migrado - Não informado no legado.'
			                                     ELSE TAU.tau_sintese
			   END AS [PLA_desenvolvimento_aula],
			   NULL AS [PLA_recuperacao_aula],
			   TAU.tau_atividadeCasa AS [PLA_licao_casa],
			   -- SQL IDs/EOL --
			   TAU.tud_id AS [SQL_tud_id],
			   TAU.tpc_id AS [SQL_tpc_id],
			   TAU.tau_id AS [SQL_tau_id],
			   TUR.tur_id AS [SQL_tur_id],
			   TUR.tur_codigoEOL AS [SQL_tur_codigoEOL],
			   ESC.esc_id AS [SQL_esc_id],
			   ESC.esc_codigo AS [SQL_esc_codigoEOL]
		  FROM            CLS_TurmaAula                            AS TAU (NOLOCK)
			   INNER JOIN TUR_TurmaDisciplina                      AS TDI (NOLOCK)
					   ON TDI.tud_id = TAU.tud_id
			   INNER JOIN TUR_TurmaRelTurmaDisciplina              AS TTD (NOLOCK)
					   ON TTD.tud_id = TAU.tud_id
			   INNER JOIN TUR_Turma                                AS TUR (NOLOCK)
					   ON TUR.tur_id = TTD.tur_id
			   INNER JOIN ESC_Escola                               AS ESC (NOLOCK)
					   ON ESC.esc_id = TUR.esc_id
			   INNER JOIN TUR_TurmaDisciplinaRelDisciplina         AS TDD (NOLOCK)
					   ON TDD.tud_id = TAU.tud_id
			   INNER JOIN ACA_Disciplina                           AS DIS (NOLOCK)
					   ON DIS.dis_id = TDD.dis_id
			   INNER JOIN [Manutencao].[dbo].[ETL_SGP_DISCIPLINAS] AS TMP (NOLOCK)
					   ON TMP.NmDisciplina = UPPER(CAST(DIS.dis_nome AS VARCHAR) COLLATE Latin1_General_CI_AS)
			   LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]            AS US1 (NOLOCK)
					   ON US1.usu_id = TAU.usu_id
			   LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]             AS PE1 (NOLOCK)
					   ON PE1.pes_id = US1.pes_id
			   LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]            AS US2 (NOLOCK)
					   ON US2.usu_id = TAU.usu_idDocenteAlteracao
			   LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]             AS PE2 (NOLOCK)
					   ON PE2.pes_id = US2.pes_id
		 WHERE TAU.tau_situacao IN (1,4) -- (1. Aula Prevista, 4. Aula Dada)
		   AND YEAR(TAU.tau_data) = 2015
		   AND UPPER(DIS.dis_nome) <> 'CONCEITO GLOBAL (INFANTIL I E II)' -- INFANTIL não será migrado
		   --AND ESC.esc_codigo IN ('019455','094765')


-- 02) Busca as Disciplinas de Regência de Classe no EOL e insere na tabela [ETL_SGP_(AULA+PLANO_AULA)_DADO508]
		INSERT INTO [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO508]
		SELECT AUL.id, AUL.AUL_ue_id, GCC.cd_componente_curricular AS [AUL_disciplina_id], AUL.AUL_turma_id, AUL.AUL_tipo_calendario_id, 
		       AUL.AUL_professor_rf, AUL.AUL_quantidade, AUL.AUL_data_aula, AUL.AUL_recorrencia_aula, AUL.AUL_tipo_aula,
			   AUL.AUL_criado_em, AUL.AUL_criado_por, AUL.AUL_alterado_em, AUL.AUL_alterado_por, AUL.AUL_criado_rf, AUL.AUL_alterado_rf, 
			   AUL.PLA_descricao, AUL.PLA_desenvolvimento_aula, AUL.PLA_recuperacao_aula, AUL.PLA_licao_casa, 
			   AUL.SQL_tud_id, AUL.SQL_tpc_id, AUL.SQL_tau_id, AUL.SQL_tur_id, AUL.SQL_tur_codigoEOL, AUL.SQL_esc_id, AUL.SQL_esc_codigoEOL
		  FROM            [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]  AS AUL (NOLOCK)
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].serie_turma_grade           AS STG (NOLOCK) 
					   ON STG.cd_turma_escola = AUL.AUL_turma_id 
					  AND STG.dt_fim IS NULL
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].escola_grade                AS EGR (NOLOCK) 
					   ON EGR.cd_escola_grade = STG.cd_escola_grade
			   INNER JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].grade_componente_curricular AS GCC (NOLOCK) 
					   ON GCC.cd_grade = EGR.cd_grade
					  AND GCC.cd_componente_curricular IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
		 WHERE AUL.AUL_disciplina_id IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
		   AND YEAR(AUL.AUL_data_aula) = 2015


-- 03) Apaga as Disciplinas de Regência de Classe (508) da tabela [ETL_SGP_(AULA+PLANO_AULA)_DADO] (as corretas estão na [ETL_SGP_(AULA+PLANO_AULA)_DADO508])
		 DELETE
		   FROM [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		  WHERE AUL_disciplina_id IN (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
		    AND YEAR(AUL_data_aula) = 2015


-- 04) Insere as Disciplinas de Regência de Classe corrigidas de volta na tabela [ETL_SGP_(AULA+PLANO_AULA)_DADO]
		 INSERT INTO [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		 SELECT AUL_ue_id, AUL_disciplina_id, AUL_turma_id, AUL_tipo_calendario_id, 
		        AUL_professor_rf, AUL_quantidade, AUL_data_aula, AUL_recorrencia_aula, AUL_tipo_aula,
			    AUL_criado_em, AUL_criado_por, AUL_alterado_em, AUL_alterado_por, AUL_criado_rf, AUL_alterado_rf, 
			    PLA_descricao, PLA_desenvolvimento_aula, PLA_recuperacao_aula, PLA_licao_casa, 
			    SQL_tud_id, SQL_tpc_id, SQL_tau_id, SQL_tur_id, SQL_tur_codigoEOL, SQL_esc_id, SQL_esc_codigoEOL
		   FROM [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO508]
		  WHERE YEAR(AUL_data_aula) = 2015


-- 05) Ajustes nos campos da Aula
		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_alterado_por = AUL_criado_por
		  WHERE AUL_alterado_em IS NOT NULL
		    AND AUL_alterado_por IS NULL
			AND YEAR(AUL_data_aula) = 2015

		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_alterado_rf = AUL_criado_rf
		  WHERE AUL_alterado_por IS NOT NULL
		    AND AUL_alterado_rf IS NULL
			AND YEAR(AUL_data_aula) = 2015

	 	 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_professor_rf = RIGHT('0000000' + [dbo].[fn_TiraLetras](AUL_professor_rf),7)
		  WHERE YEAR(AUL_data_aula) = 2015

		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_criado_rf = RIGHT('0000000' + [dbo].[fn_TiraLetras](AUL_criado_rf),7)
		  WHERE YEAR(AUL_data_aula) = 2015

	  	 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_alterado_rf = RIGHT('0000000' + [dbo].[fn_TiraLetras](AUL_alterado_rf),7)
		  WHERE YEAR(AUL_data_aula) = 2015

		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_criado_por = SUBSTRING(AUL_criado_por,1,200)
		  WHERE LEN(AUL_criado_por) > 200
		    AND YEAR(AUL_data_aula) = 2015

		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET AUL_alterado_por = SUBSTRING(AUL_alterado_por,1,200)
		  WHERE LEN(AUL_alterado_por) > 200
		    AND YEAR(AUL_data_aula) = 2015


-- 06) Ajustes no campo descricao (Plano de Aula)
		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
 		   SET PLA_descricao = dbo.fc_LimpaHTML(PLA_descricao)
		 WHERE PLA_descricao IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_descricao = RTRIM(LTRIM(PLA_descricao))
		 WHERE PLA_descricao IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_descricao = NULL
		 WHERE ((PLA_descricao = '') 
		    OR  (PLA_descricao = ' ') 
			OR  (PLA_descricao = '  '))
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_descricao = NULL
		 WHERE LEN(PLA_descricao) <= 2
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_descricao = RTRIM(LTRIM(PLA_descricao))
		 WHERE PLA_descricao IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_descricao = LEFT(LTRIM(PLA_descricao), 30000)
		 WHERE LEN(PLA_descricao) > 30000
		   AND YEAR(AUL_data_aula) = 2015 


-- 07) Ajustes no campo desenvolvimento_aula (Plano de Aula)
		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
 		   SET PLA_desenvolvimento_aula = dbo.fc_LimpaHTML(PLA_desenvolvimento_aula)
		 WHERE PLA_desenvolvimento_aula IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_desenvolvimento_aula = LTRIM(RTRIM(PLA_desenvolvimento_aula))
		 WHERE YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_desenvolvimento_aula = 'Migrado - Não informado no legado.'
		 WHERE ((PLA_desenvolvimento_aula = '') 
		    OR  (PLA_desenvolvimento_aula = ' ') 
			OR  (PLA_desenvolvimento_aula = '  '))
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_desenvolvimento_aula = 'Migrado - Não informado no legado.'
		 WHERE LEN(PLA_desenvolvimento_aula) <= 2
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_desenvolvimento_aula = 'Migrado - Não informado no legado.'
		 WHERE PLA_desenvolvimento_aula IS NULL
		   AND YEAR(AUL_data_aula) = 2015 

		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET PLA_desenvolvimento_aula = LEFT(LTRIM(PLA_desenvolvimento_aula), 30000)
		  WHERE LEN(PLA_desenvolvimento_aula) > 30000
		   AND YEAR(AUL_data_aula) = 2015 


-- 08) Ajustes no campo recuperacao_aula (Plano de Aula)
		 UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		    SET PLA_recuperacao_aula = LEFT(LTRIM(PLA_recuperacao_aula), 30000)
		  WHERE LEN(PLA_recuperacao_aula) > 30000
		    AND YEAR(AUL_data_aula) = 2015 


-- 09) Ajustes no campo licao_casa (Plano de Aula)
		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
 		   SET PLA_licao_casa = dbo.fc_LimpaHTML(PLA_licao_casa)
		 WHERE PLA_licao_casa IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_licao_casa = RTRIM(LTRIM(PLA_licao_casa))
		 WHERE PLA_licao_casa IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_licao_casa = NULL
		 WHERE ((PLA_licao_casa = '') 
		    OR  (PLA_licao_casa = ' ') 
			OR  (PLA_licao_casa = '  '))
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_licao_casa = NULL
		 WHERE LEN(PLA_licao_casa) <= 2
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_licao_casa = RTRIM(LTRIM(PLA_licao_casa))
		 WHERE PLA_licao_casa IS NOT NULL
		   AND YEAR(AUL_data_aula) = 2015 

		UPDATE [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		   SET PLA_licao_casa = LEFT(LTRIM(PLA_licao_casa), 30000)
		 WHERE LEN(PLA_licao_casa) > 30000
		   AND YEAR(AUL_data_aula) = 2015 


-- 10) Insere os dados na tabela [ETL_SGP_(AULA+PLANO_AULA)_DADO_OK]
		INSERT INTO [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO_OK]
		SELECT AUL_ue_id, AUL_disciplina_id, AUL_turma_id, AUL_tipo_calendario_id, 
		       AUL_professor_rf, AUL_quantidade, AUL_data_aula, AUL_recorrencia_aula, AUL_tipo_aula,
		       AUL_criado_em, AUL_criado_por, AUL_alterado_em, AUL_alterado_por, AUL_criado_rf, AUL_alterado_rf, 
		       PLA_descricao, PLA_desenvolvimento_aula, PLA_recuperacao_aula, PLA_licao_casa, 
		       SQL_tud_id, SQL_tpc_id, SQL_tau_id, SQL_tur_id, SQL_tur_codigoEOL, SQL_esc_id, SQL_esc_codigoEOL
		  FROM [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO]
		 WHERE YEAR(AUL_data_aula) = 2015 


-- SELECT * FROM [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO_OK] ORDER BY ID

