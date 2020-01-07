-- #######################################################################
-- ##                                                                   ##
-- ##                   P L A N O   D E   C I C L O                     ##
-- ##                                                                   ##
-- #######################################################################

USE GestaoPedagogica
GO


-- CICLOS 1,2,3,4
INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
SELECT CASE WHEN DATALENGTH(PLA.plc_planoCiclo) IS NULL THEN 'Migrado - Não informado no legado.'
            WHEN DATALENGTH(PLA.plc_planoCiclo) <= 3    THEN 'Migrado - Não informado no legado.'
													    ELSE PLA.plc_planoCiclo
       END AS [descricao],
	   PLA.plc_anoLetivo AS [ano],
	   CASE PLA.tci_id WHEN 1 THEN 3
                       WHEN 2 THEN 1
                       WHEN 6 THEN 2
                       WHEN 9 THEN 4
	   END AS [ciclo_id],
	   LEFT(RTRIM(LTRIM(ESC.esc_codigo)),10) AS [escola_id],
	   1 AS [migrado],
	   PLA.plc_dataCriacao AS [criado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [criado_por],
	   PLA.plc_dataAlteracao AS [alterado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [alterado_por],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [criado_rf],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [alterado_rf]
  FROM            [GestaoPedagogica].[dbo].[CLS_PlanejamentoCiclo] AS PLA (NOLOCK)
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_UnidadeEscola]     AS UNI (NOLOCK)
               ON UNI.esc_id = PLA.esc_id
	          AND UNI.uni_id = PLA.uni_id
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]            AS ESC (NOLOCK)
               ON ESC.esc_id = UNI.esc_id
       INNER JOIN [CoreSSO].[dbo].[SYS_Usuario]                    AS USU (NOLOCK)
               ON PLA.usu_id = USU.usu_id
       INNER JOIN [CoreSSO].[dbo].[PES_Pessoa]                     AS PES (NOLOCK)
               ON USU.pes_id = PES.pes_id
 WHERE PLA.tci_id NOT IN (3,4,5,7,8,10,11,12) -- Retirados planos de ciclo inexistentes no SGP Novo
   AND PLA.plc_situacao = 1                   -- Ativos
-- [7638 linhas afetadas]


 -- CICLO 5
INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
SELECT CASE WHEN DATALENGTH(PLA.plc_planoCiclo) IS NULL THEN 'Migrado - Não informado no legado.'
            WHEN DATALENGTH(PLA.plc_planoCiclo) <= 3    THEN 'Migrado - Não informado no legado.'
													    ELSE PLA.plc_planoCiclo
       END AS [descricao],
	   PLA.plc_anoLetivo AS [ano],
	   5 AS [ciclo_id],
	   LEFT(RTRIM(LTRIM(ESC.esc_codigo)),10) AS [escola_id],
	   1 AS [migrado],
	   PLA.plc_dataCriacao AS [criado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [criado_por],
	   PLA.plc_dataAlteracao AS [alterado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [alterado_por],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [criado_rf],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [alterado_rf]
  FROM            [GestaoPedagogica].[dbo].[CLS_PlanejamentoCiclo] AS PLA (NOLOCK)
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_UnidadeEscola]     AS UNI (NOLOCK)
               ON UNI.esc_id = PLA.esc_id
	          AND UNI.uni_id = PLA.uni_id
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]            AS ESC (NOLOCK)
               ON ESC.esc_id = UNI.esc_id
       INNER JOIN [CoreSSO].[dbo].[SYS_Usuario]                    AS USU (NOLOCK)
               ON PLA.usu_id = USU.usu_id
       INNER JOIN [CoreSSO].[dbo].[PES_Pessoa]                     AS PES (NOLOCK)
               ON USU.pes_id = PES.pes_id
 WHERE PLA.tci_id = 10             -- EJA - Educação de Jovens e Adultos serão 4 códigos novos (5,6,7,8)
   AND PLA.plc_situacao = 1        -- Ativos
-- [405 linhas afetadas]


-- CICLO 6
INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
SELECT CASE WHEN DATALENGTH(PLA.plc_planoCiclo) IS NULL THEN 'Migrado - Não informado no legado.'
            WHEN DATALENGTH(PLA.plc_planoCiclo) <= 3    THEN 'Migrado - Não informado no legado.'
													    ELSE PLA.plc_planoCiclo
       END AS [descricao],
	   PLA.plc_anoLetivo AS [ano],
	   6 AS [ciclo_id],
	   LEFT(RTRIM(LTRIM(ESC.esc_codigo)),10) AS [escola_id],
	   1 AS [migrado],
	   PLA.plc_dataCriacao AS [criado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [criado_por],
	   PLA.plc_dataAlteracao AS [alterado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [alterado_por],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [criado_rf],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [alterado_rf]
  FROM            [GestaoPedagogica].[dbo].[CLS_PlanejamentoCiclo] AS PLA (NOLOCK)
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_UnidadeEscola]     AS UNI (NOLOCK)
               ON UNI.esc_id = PLA.esc_id
	          AND UNI.uni_id = PLA.uni_id
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]            AS ESC (NOLOCK)
               ON ESC.esc_id = UNI.esc_id
       INNER JOIN [CoreSSO].[dbo].[SYS_Usuario]                    AS USU (NOLOCK)
               ON PLA.usu_id = USU.usu_id
       INNER JOIN [CoreSSO].[dbo].[PES_Pessoa]                     AS PES (NOLOCK)
               ON USU.pes_id = PES.pes_id
 WHERE PLA.tci_id = 10             -- EJA - Educação de Jovens e Adultos serão 4 códigos novos (5,6,7,8)
   AND PLA.plc_situacao = 1        -- Ativos
-- [405 linhas afetadas]


-- CICLO 7
INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
SELECT CASE WHEN DATALENGTH(PLA.plc_planoCiclo) IS NULL THEN 'Migrado - Não informado no legado.'
            WHEN DATALENGTH(PLA.plc_planoCiclo) <= 3    THEN 'Migrado - Não informado no legado.'
													    ELSE PLA.plc_planoCiclo
       END AS [descricao],
	   PLA.plc_anoLetivo AS [ano],
	   7 AS [ciclo_id],
	   LEFT(RTRIM(LTRIM(ESC.esc_codigo)),10) AS [escola_id],
	   1 AS [migrado],
	   PLA.plc_dataCriacao AS [criado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [criado_por],
	   PLA.plc_dataAlteracao AS [alterado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [alterado_por],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [criado_rf],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [alterado_rf]
  FROM            [GestaoPedagogica].[dbo].[CLS_PlanejamentoCiclo] AS PLA (NOLOCK)
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_UnidadeEscola]     AS UNI (NOLOCK)
               ON UNI.esc_id = PLA.esc_id
	          AND UNI.uni_id = PLA.uni_id
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]            AS ESC (NOLOCK)
               ON ESC.esc_id = UNI.esc_id
       INNER JOIN [CoreSSO].[dbo].[SYS_Usuario]                    AS USU (NOLOCK)
               ON PLA.usu_id = USU.usu_id
       INNER JOIN [CoreSSO].[dbo].[PES_Pessoa]                     AS PES (NOLOCK)
               ON USU.pes_id = PES.pes_id
 WHERE PLA.tci_id = 10             -- EJA - Educação de Jovens e Adultos serão 4 códigos novos (5,6,7,8)
   AND PLA.plc_situacao = 1        -- Ativos
-- [405 linhas afetadas]


-- CICLO 8
INSERT INTO [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
SELECT CASE WHEN DATALENGTH(PLA.plc_planoCiclo) IS NULL THEN 'Migrado - Não informado no legado.'
            WHEN DATALENGTH(PLA.plc_planoCiclo) <= 3    THEN 'Migrado - Não informado no legado.'
													    ELSE PLA.plc_planoCiclo
       END AS [descricao],
	   PLA.plc_anoLetivo AS [ano],
	   8 AS [ciclo_id],
	   LEFT(RTRIM(LTRIM(ESC.esc_codigo)),10) AS [escola_id],
	   1 AS [migrado],
	   PLA.plc_dataCriacao AS [criado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [criado_por],
	   PLA.plc_dataAlteracao AS [alterado_em],
	   SUBSTRING(RTRIM(LTRIM(PES.pes_nome)),1,200) AS [alterado_por],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [criado_rf],
	   SUBSTRING(RTRIM(LTRIM(USU.usu_login)),1,7) AS [alterado_rf]
  FROM            [GestaoPedagogica].[dbo].[CLS_PlanejamentoCiclo] AS PLA (NOLOCK)
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_UnidadeEscola]     AS UNI (NOLOCK)
               ON UNI.esc_id = PLA.esc_id
	          AND UNI.uni_id = PLA.uni_id
       INNER JOIN [GestaoPedagogica].[dbo].[ESC_Escola]            AS ESC (NOLOCK)
               ON ESC.esc_id = UNI.esc_id
       INNER JOIN [CoreSSO].[dbo].[SYS_Usuario]                    AS USU (NOLOCK)
               ON PLA.usu_id = USU.usu_id
       INNER JOIN [CoreSSO].[dbo].[PES_Pessoa]                     AS PES (NOLOCK)
               ON USU.pes_id = PES.pes_id
 WHERE PLA.tci_id = 10             -- EJA - Educação de Jovens e Adultos serão 4 códigos novos (5,6,7,8)
   AND PLA.plc_situacao = 1        -- Ativos
-- [405 linhas afetadas]


-- Ajustes
	UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
	   SET descricao = dbo.fc_LimpaHTML(descricao)

	UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
	   SET descricao = LTRIM(RTRIM(descricao))

	UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
	   SET descricao = NULL
	 WHERE (descricao = '') OR (descricao = ' ') OR (descricao = '  ')

	UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
	   SET descricao = 'Migrado - Não informado no legado.'
	 WHERE descricao IS NULL

	UPDATE [Manutencao].[DBO].[ETL_SGP_PLANO_DE_CICLO]
	   SET descricao = LEFT(descricao, 30000)
	 WHERE LEN(descricao) >= 30000

