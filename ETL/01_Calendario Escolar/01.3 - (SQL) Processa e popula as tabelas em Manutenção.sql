USE GestaoPedagogica
GO

-- 1) Inserindo dados na [ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO]
		INSERT INTO [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO]
		SELECT CAL.cal_id AS [id_LEGADO_TC],
			   CAL.cal_ano AS [ano_letivo],
			   SUBSTRING(RTRIM(LTRIM(CAL.cal_descricao)),1,50) AS [nome],
			   CASE
					WHEN (DATEDIFF("m", CAL.cal_dataInicio, CAL.cal_dataFim)) >= 7 THEN 1   -- Anual
					WHEN (DATEDIFF("m", CAL.cal_dataInicio, CAL.cal_dataFim)) <= 6 THEN 2   -- Semestral
																				   ELSE 0   -- Erro
			   END AS [periodo],
			   CASE
					WHEN CHARINDEX('EJA',CAL.cal_descricao) > 0 THEN 2   -- EJA Regular
																ELSE 1   -- Fundamental/Médio Regular
			   END AS [modalidade],
			   1 AS [situacao],
			   CAL.cal_dataCriacao AS [criado_em],
			   'Migrado - Não informado no legado' AS [criado_por],
			   CAL.cal_dataAlteracao AS [alterado_em],
			   'Migrado - Não informado no legado' AS [alterado_por],
			   0 AS [criado_rf],
			   0 AS [alterado_rf],
			   0 AS [excluido],
			   1 AS [migrado]
		  FROM ACA_CalendarioAnual AS CAL (NOLOCK)
		 WHERE CAL.cal_situacao = 1                    -- Trazendo só os ativos da tabela ACA_CalendarioAnual
		   AND CAL.cal_descricao NOT LIKE '%CIEJA%'    -- Não traz o Calendário Escolar CIEJA
		   AND CAL.cal_descricao NOT LIKE '%Infantil%' -- Não traz o Calendário Escolar Infantil
		 ORDER BY CAL.cal_id, CAL.cal_ano
-- [12 linhas]


-- 2) Inserindo dados na [ETL_SGP_CALENDARIO_ESCOLAR_PERIODO_ESCOLAR]
		INSERT INTO [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_PERIODO_ESCOLAR]
		SELECT CPE.cap_id AS [id_LEGADO_PE],
			   CPE.cal_id AS [tipo_calendario_id],
			   CPE.tpc_id AS [bimestre],
			   CPE.cap_dataInicio AS [periodo_inicio],
			   CPE.cap_dataFim AS [periodo_fim],
			   'Migrado - Não informado no legado' AS [alterado_por],
			   0 AS [alterado_rf],
			   CPE.cap_dataAlteracao AS [alterado_em],
			   'Migrado - Não informado no legado' AS [criado_por],
			   0 AS [criado_rf],
			   CPE.cap_dataCriacao AS [criado_em],
			   1 AS [migrado]
		  FROM ACA_CalendarioPeriodo AS [CPE] (NOLOCK)
		 WHERE CPE.cap_situacao = 1                                                                   -- Trazendo só os ativos da tabela ACA_CalendarioPeriodo
		   AND CPE.cal_id IN (SELECT id_LEGADO_TC 
								FROM [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO]) -- Registros do ACA_CalendarioAnual
		 ORDER BY CPE.cal_id, CPE.tpc_id
-- [36 linhas]


-- 3) Inserindo dados na [ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
		INSERT INTO [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
		SELECT EVT.evt_id AS [id_LEGADO_EV],
			   LEFT(RTRIM(LTRIM(EVT.evt_nome)),200) AS [nome],
			   CASE WHEN DATALENGTH(EVT.evt_descricao) IS NULL THEN ''
					WHEN DATALENGTH(EVT.evt_descricao) = 0     THEN ''
															   ELSE SUBSTRING(EVT.evt_descricao,1,500)
			   END AS [descricao],
			   EVT.evt_dataInicio AS [data_inicio],
			   EVT.evt_dataFim AS [data_fim],
			   CASE WHEN EVT.evt_semAtividadeDiscente = 1 THEN 2
					WHEN EVT.evt_semAtividadeDiscente = 0 THEN 1
			   END AS [letivo],
			   NULL AS [feriado_id],
			   CASE YEAR(EVT.evt_datainicio) 
					WHEN 2014 THEN 1
					WHEN 2015 THEN 2
					WHEN 2016 THEN 3
					WHEN 2017 THEN 4
					WHEN 2018 THEN 7
					WHEN 2019 THEN 10
			   END AS [tipo_calendario_id],
			   CASE EVT.tev_id 
					WHEN 1    THEN  3 -- Fechamento de Bimestre
					WHEN 2    THEN  9 -- Efetivação da recuperação / Outros
					WHEN 3    THEN  9 -- Fechamento final / Outros
					WHEN 4    THEN  9 -- Efetivação da recuperação final / Outros
					WHEN 5    THEN  9 -- Férias
					WHEN 6    THEN  4 -- Feriado
					WHEN 8    THEN 11 -- Recesso
					WHEN 9    THEN  8 -- Organização Escolar / Organização SME
					WHEN 11   THEN 13 -- Reposição de Aula
					WHEN 12   THEN 19 -- Reunião de Responsáveis
					WHEN 13   THEN  1 -- Conselho de Classe
					WHEN 15   THEN  9 -- Outros
					WHEN 17   THEN 10 -- Projeto Político Pedagógico
					WHEN 19   THEN 12 -- Recreio nas Férias
					WHEN 20   THEN 16 -- Reunião Pedagógica
					WHEN 21   THEN 17 -- Reunião de APM
					WHEN 22   THEN  5 -- Férias docentes
					WHEN 23   THEN  7 -- Liberação do Boletim
					WHEN 24   THEN  9 -- Atividade Diversificada / Outros
					WHEN 25   THEN 18 -- Reunião de Conselho de Escola
					WHEN 26   THEN 21 -- Suspensão de Atividades
					WHEN 27   THEN  9 -- Abertura de sugestões de currículos / Outros
					WHEN 28   THEN  9 -- Cadastro de preferência de horário / Outros
			   END AS [tipo_evento_id],
			   VCE.cd_unidade_administrativa_referencia AS [dre_id],
			   UNI.uni_codigo AS [ue_id],    -- Vem do EOL
			   EVT.evt_dataCriacao AS [criado_em],
			   'Migrado - Não informado no legado' AS [criado_por],
			   EVT.evt_dataAlteracao AS [alterado_em],
			   'Migrado - Não informado no legado' AS [alterado_por],
			   0 AS [criado_rf],
			   0 AS [alterado_rf],
			   0 AS [excluido],
			   1 AS [migrado]
		  FROM           ACA_Evento        AS EVT (NOLOCK)
			   LEFT JOIN ESC_UnidadeEscola AS UNI (NOLOCK) 
					  ON UNI.uni_id = EVT.uni_id
					 AND UNI.esc_id = EVT.esc_id
			   LEFT JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] AS VCE (NOLOCK) 
					  ON VCE.cd_unidade_educacao = UNI.uni_codigo COLLATE Latin1_General_CI_AS
		 WHERE EVT.evt_situacao = 1  -- Trazendo só os ativos da tabela ACA_Evento
		   AND YEAR(EVT.evt_datainicio) BETWEEN 2014 AND 2019
		   AND YEAR(EVT.evt_datafim) BETWEEN 2014 AND 2019
		 ORDER BY EVT.evt_id
-- [143.566 linhas]


-- 4) Ajustes
		UPDATE [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
		   SET descricao = ''
		 WHERE descricao IS NULL
-- [0 linhas]

		UPDATE [Manutencao].[dbo].[ETL_SGP_CALENDARIO_ESCOLAR_EVENTO]
		   SET descricao = LTRIM(RTRIM(descricao))
-- [143.566 linhas]

