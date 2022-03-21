USE GestaoPedagogica
GO

INSERT INTO [Manutencao].[dbo].[ETL_SGP_FREQUENCIA_AUSENCIA]
SELECT TAA.tud_id AS [TAA_tud_id], 
	   TAA.tau_id AS [TAA_tau_id],
	   TAA.alu_id AS [TAA_alu_id],
	   TAA.mtu_id AS [TAA_mtu_id],
	   TAA.mtd_id AS [TAA_mtd_id],
	   TAA.taa_dataCriacao AS [TAA_criado_em],
	   TAA.taa_dataAlteracao AS [TAA_alterado_em],
	   'Migrado' AS [TAA_alterado_por],
	   TAA.usu_idDocenteAlteracao AS [TAA_usu_idDocenteAlteracao],
	   TAU.tpc_id AS [TAU_tpc_id],
	   TAU.tau_data AS [TAU_tau_data],
	   TAU.tau_numeroAulas AS [TAU_tau_numeroAulas],
	   TAU.tau_dataCriacao AS [TAU_criado_em],
	   TAU.tau_dataAlteracao AS [TAU_alterado_em],
	   CASE WHEN PE1.pes_nome IS NULL THEN 'Migrado'
			                          ELSE PE1.pes_nome 
	   END AS [TAU_criado_por],
	   CASE WHEN PE2.pes_nome IS NULL THEN 'Migrado'
			                          ELSE PE2.pes_nome
	   END AS [TAU_alterado_por],
	   CASE WHEN US1.usu_login IS NULL THEN '0'
		                               ELSE US1.usu_login 
	   END AS [AUL_criado_rf], 
	   US2.usu_login AS [AUL_alterado_rf],
	   TAU.usu_id AS [TAU_usu_id],
	   TAU.usu_idDocenteAlteracao AS [TAU_usu_idDocenteAlteracao],
       ESC.esc_id AS [ESC_esc_id],
       TUR.tur_id AS [TUR_tur_id],
	   AAC.alc_matricula AS [Codigo_Aluno]

  FROM            CLS_TurmaAulaAluno                       AS TAA (NOLOCK)
       INNER JOIN CLS_TurmaAula                            AS TAU (NOLOCK)
	           ON TAU.tud_id = TAA.tud_id
			  AND TAU.tau_id = TAA.tau_id
	   INNER JOIN TUR_TurmaDisciplina                      AS TDI (NOLOCK)
			   ON TDI.tud_id = TAU.tud_id
       INNER JOIN TUR_TurmaRelTurmaDisciplina              AS TTD (NOLOCK)
               ON TTD.tud_id = TAU.tud_id
       INNER JOIN TUR_Turma                                AS TUR (NOLOCK)
               ON TUR.tur_id = TTD.tur_id
       INNER JOIN ESC_Escola                               AS ESC (NOLOCK)
               ON ESC.esc_id = TUR.esc_id
       INNER JOIN ACA_AlunoCurriculo                       AS AAC (NOLOCK)
	           ON AAC.alu_id = TAA.alu_id
			  AND AAC.esc_id = TUR.esc_id
	   LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]            AS US1 (NOLOCK)
		  	   ON US1.usu_id = TAU.usu_id
	   LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]             AS PE1 (NOLOCK)
			   ON PE1.pes_id = US1.pes_id
	   LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]            AS US2 (NOLOCK)
			   ON US2.usu_id = TAU.usu_idDocenteAlteracao
	   LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]             AS PE2 (NOLOCK)
			   ON PE2.pes_id = US2.pes_id
	   LEFT  JOIN [CoreSSO].[dbo].[SYS_Usuario]            AS US3 (NOLOCK)
			   ON US3.usu_id = TAA.usu_idDocenteAlteracao
	   LEFT  JOIN [CoreSSO].[dbo].[PES_Pessoa]             AS PE3 (NOLOCK)
			   ON PE3.pes_id = US3.pes_id

 WHERE TAA.taa_situacao <> 3
   AND YEAR(TAU.tau_data) = 2015

 GROUP BY TAA.tud_id, 
	   TAA.tau_id,
	   TAA.alu_id,
	   TAA.mtu_id,
	   TAA.mtd_id,
	   TAA.taa_dataCriacao,
	   TAA.taa_dataAlteracao,
	   PE3.pes_nome,
	   TAA.usu_idDocenteAlteracao,
	   TAU.tpc_id,
	   TAU.tau_data,
	   TAU.tau_numeroAulas,
	   TAU.tau_dataCriacao,
	   TAU.tau_dataAlteracao,
	   PE1.pes_nome,
	   PE2.pes_nome,
	   US1.usu_login,
	   US2.usu_login,
	   TAU.usu_id,
	   TAU.usu_idDocenteAlteracao,
       ESC.esc_id,
       TUR.tur_id,
	   AAC.alc_matricula


-- SELECT * FROM [Manutencao].[dbo].[ETL_SGP_FREQUENCIA_AUSENCIA] ORDER BY id

