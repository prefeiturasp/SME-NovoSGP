  insert into registro_ausencia_aluno
  (codigo_aluno, numero_aula, registro_frequencia_id, migrado, excluido, 
   criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
  select fre.codigo_aluno as codigo_aluno,
         fre.tau_tau_numeroaulas as numero_aula,
         reg.id as registro_frequencia_id,
         true as migrado,
         false as excluido,
         fre.taa_criado_em as criado_em,
         'Migrado' as criado_por,
         0 as criado_rf,
         fre.taa_alterado_em as alterado_em,
         'Migrado' as alterado_por,
         0 as alterado_rf
         from            etl_sgp_frequencia_ausencia as fre
			  inner join etl_sgp_aula_plano_aula_dados_ok as etl
					  on etl.sql_tud_id = fre.taa_tud_id
					 and etl.sql_esc_id = fre.esc_esc_id
					 and etl.sql_tau_id = fre.taa_tau_id
					 and etl.sql_tpc_id = fre.tau_tpc_id
					 and etl.sql_tur_id = fre.tur_tur_id
			  inner join aula as aul
					  on right('000000' || aul.ue_id,6) = right('000000' || etl.aul_ue_id,6)
					 and aul.disciplina_id = etl.aul_disciplina_id
					 and aul.turma_id = etl.aul_turma_id
					 and aul.tipo_calendario_id = etl.aul_tipo_calendario_id
					 and aul.professor_rf = etl.aul_professor_rf
					 and aul.quantidade = etl.aul_quantidade
					 and aul.data_aula = etl.aul_data_aula
					 and aul.recorrencia_aula = etl.aul_recorrencia_aula
					 and aul.tipo_aula = etl.aul_tipo_aula
					 and aul.criado_em = etl.aul_criado_em
					 and aul.criado_por = etl.aul_criado_por
					 and aul.alterado_em = etl.aul_alterado_em
					 and aul.alterado_por = etl.aul_alterado_por
					 and aul.criado_rf = etl.aul_criado_rf
					 and aul.alterado_rf = etl.aul_alterado_rf
			  inner join registro_frequencia as reg
					  on reg.aula_id = aul.id
   order by fre.id
