USE GestaoPedagogica
GO

 SELECT taa_tud_id, taa_tau_id, taa_alu_id, taa_mtu_id, taa_mtd_id, 
		taa_criado_em, taa_alterado_em, taa_alterado_por, taa_usu_idDocenteAlteracao, 
		tau_tpc_id, tau_tau_data, TAU_tau_numeroAulas, 
		tau_criado_em, tau_alterado_em, tau_criado_por, tau_alterado_por, tau_criado_rf, tau_alterado_rf, 
		tau_usu_id, tau_usu_iddocentealteracao, esc_esc_id, tur_tur_id, Codigo_Aluno
   FROM [Manutencao].[dbo].[ETL_SGP_FREQUENCIA_AUSENCIA]
  WHERE YEAR(tau_tau_data) = 2019
  ORDER BY ID



