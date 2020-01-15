USE GestaoPedagogica
GO
SELECT 
	tipo_calendario_id, 
	disciplina_id, 
	turma_id,
	criado_em, 
	criado_por, 
	alterado_em, 
	alterado_por, 
	criado_rf,
	alterado_rf, 
	migrado 
FROM [Manutencao].[dbo].[AULA_PREVISTA]
GO

SELECT 
	aula_prevista_id, 
	aulas_previstas, 
	bimestre,
	criado_em, 
	criado_por, 
	alterado_em, 
	alterado_por, 
	criado_rf, 
	alterado_rf, 
	excluido, 
	migrado
FROM [Manutencao].[dbo].[AULA_PREVISTA_BIMESTRE]
GO