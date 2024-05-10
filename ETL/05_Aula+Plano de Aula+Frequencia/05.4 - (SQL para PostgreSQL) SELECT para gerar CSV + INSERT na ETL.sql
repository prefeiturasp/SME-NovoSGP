USE GestaoPedagogica
GO

 SELECT AUL_ue_id, AUL_disciplina_id, AUL_turma_id, AUL_tipo_calendario_id, 
		AUL_professor_rf, AUL_quantidade, AUL_data_aula, AUL_recorrencia_aula, AUL_tipo_aula,
		AUL_criado_em, AUL_criado_por, AUL_alterado_em, AUL_alterado_por, AUL_criado_rf, AUL_alterado_rf, 
		PLA_descricao, PLA_desenvolvimento_aula, PLA_recuperacao_aula, PLA_licao_casa, 
		SQL_tud_id, SQL_tpc_id, SQL_tau_id, SQL_tur_id, SQL_tur_codigoEOL, SQL_esc_id, SQL_esc_codigoEOL
   FROM [Manutencao].[dbo].[ETL_SGP_(AULA+PLANO_AULA)_DADO_OK] 
  ORDER BY ID
