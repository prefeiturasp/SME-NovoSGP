USE Manutencao
GO

 SELECT --EVT.id,
        EVT.nome,
		EVT.descricao,
		EVT.data_inicio,
		EVT.data_fim,
		EVT.letivo,
		EVT.feriado_id,
		EVT.tipo_calendario_id,
		EVT.tipo_evento_id,
		EVT.dre_id,
		EVT.ue_id,
		EVT.criado_em,
		EVT.criado_por,
		EVT.alterado_em,
		EVT.alterado_por,
		EVT.criado_rf,
		EVT.alterado_rf,
		CASE EVT.excluido WHEN 0 THEN 'FALSE'
						  WHEN 1 THEN 'TRUE'
		END AS [excluido],
		CASE EVT.migrado  WHEN 0 THEN 'FALSE'
						  WHEN 1 THEN 'TRUE'
		END AS [migrado]
   FROM [ETL_SGP_CALENDARIO_ESCOLAR_EVENTO] AS EVT 
  ORDER BY EVT.id_LEGADO_EV

