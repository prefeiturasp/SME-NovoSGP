USE [Manutencao]
GO


-- SELECT DE SAÍDA (PARA EXPORTAÇÃO NO POSTGRESQL)
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
  FROM [Manutencao].[dbo].[ETL_SGP_Compensacao_Ausencia]
GO