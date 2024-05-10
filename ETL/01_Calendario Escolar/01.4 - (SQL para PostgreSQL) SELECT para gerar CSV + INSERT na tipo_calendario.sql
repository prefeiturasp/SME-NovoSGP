USE Manutencao
GO

 SELECT --CAL.id,
        CAL.ano_letivo,
		CAL.nome,
		CAL.periodo,
		CAL.modalidade,
		CASE CAL.situacao WHEN 0 THEN 'FALSE'
						  WHEN 1 THEN 'TRUE'
		END AS [situacao],
		CAL.criado_em,
		CAL.criado_por,
		CAL.alterado_em,
		CAL.alterado_por,
		CAL.criado_rf,
		CAL.alterado_rf,
		CASE CAL.excluido WHEN 0 THEN 'FALSE'
						  WHEN 1 THEN 'TRUE'
		END AS [excluido],
		CASE CAL.migrado  WHEN 0 THEN 'FALSE'
						  WHEN 1 THEN 'TRUE'
		END AS [migrado]
   FROM [ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO] AS CAL (NOLOCK)