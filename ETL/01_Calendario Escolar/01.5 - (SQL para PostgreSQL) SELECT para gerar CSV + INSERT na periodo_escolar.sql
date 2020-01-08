USE Manutencao
GO

 SELECT --PEE.id,
        TPC.ID AS [tipo_calendario_id],
		PEE.bimestre,
		PEE.periodo_inicio,
		PEE.periodo_fim,
		PEE.alterado_por,
		PEE.alterado_rf,
		PEE.alterado_em,
		PEE.criado_por,
		PEE.criado_rf,
		PEE.criado_em,
		CASE PEE.migrado WHEN 0 THEN 'FALSE'
						 WHEN 1 THEN 'TRUE'
		END AS [migrado]
   FROM            [ETL_SGP_CALENDARIO_ESCOLAR_PERIODO_ESCOLAR] AS PEE 
	    INNER JOIN [ETL_SGP_CALENDARIO_ESCOLAR_TIPO_CALENDARIO] AS TPC 
			                                                    ON TPC.id_LEGADO_TC = PEE.tipo_calendario_id