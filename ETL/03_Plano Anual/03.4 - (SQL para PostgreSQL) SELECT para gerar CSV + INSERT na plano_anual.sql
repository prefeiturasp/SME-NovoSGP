USE Manutencao
GO

 SELECT escola_id,
		turma_id, 
		ano,
		bimestre,
		descricao,
		criado_em,
		criado_por,
		alterado_em,
		alterado_por,
		criado_rf,
		alterado_rf,
		migrado,
		componente_curricular_eol_id
   FROM [Manutencao].[DBO].[ETL_SGP_PLANO_ANUAL]
  ORDER BY id