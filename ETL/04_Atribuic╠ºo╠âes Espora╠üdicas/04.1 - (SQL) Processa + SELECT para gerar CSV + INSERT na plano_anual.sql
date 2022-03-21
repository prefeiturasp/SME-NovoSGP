USE GestaoPedagogica
GO

SELECT DISTINCT
       COL.col_id AS [id],
	   RIGHT([dbo].[fn_TiraLetras](RTRIM(LTRIM(CCA.coc_matricula))),7) AS [professor_id],
	   UAD.uad_codigo AS [ue_id],
	   VCE.cd_unidade_administrativa_referencia AS [dre_id],
	   CAST(CCA.coc_vigenciaInicio AS DATETIME) AS [data_inicio],
	   CAST(CCA.coc_vigenciaFim AS DATETIME) AS [data_fim],
	   'Migrado - Não informado no legado.' AS [criado_por],
	   CCA.coc_dataCriacao AS [criado_em],
	   'Migrado - Não informado no legado.' AS [alterado_por],
	   CCA.coc_dataAlteracao AS [alterado_em],
	   0 AS [criado_rf],
	   0 AS [alterado_rf],
	   'FALSE' AS [excluido],
	   'TRUE' AS [migrado]
  FROM            RHU_Colaborador                           AS COL (NOLOCK)
       INNER JOIN RHU_ColaboradorCargo                      AS CCA (NOLOCK) 
	           ON CCA.col_id = COL.col_id
       INNER JOIN [CORESSO].[DBO].[SYS_Usuario]             AS USU 
	           ON USU.pes_id = COL.pes_id
	   INNER JOIN [CORESSO].[DBO].SYS_UnidadeAdministrativa AS UAD 
	           ON UAD.uad_id = CCA.uad_id
	   LEFT  JOIN [DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_cadastro_unidade_educacao] AS VCE (NOLOCK) 
	           ON VCE.cd_unidade_educacao = UAD.uad_codigo COLLATE Latin1_General_CI_AS
 WHERE cca.crg_id = 236  -- 'Atribuição esporádica - EFETIVO' na tabela RHU_Cargo
   AND cca.coc_situacao <> 3
 ORDER BY COL.col_id, CAST(CCA.coc_vigenciaInicio AS DATETIME)
-- [1563 linhas afetadas]




