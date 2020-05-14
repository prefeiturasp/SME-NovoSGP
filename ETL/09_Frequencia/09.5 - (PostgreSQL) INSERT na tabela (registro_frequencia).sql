insert into registro_frequencia 
(aula_id, migrado, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf, excluido)
select AUL.id as aula_id,
       true as migrado,
       AUL.criado_em,
       AUL.criado_por,
       AUL.criado_rf,
       AUL.alterado_em,
       AUL.alterado_por,
       AUL.alterado_rf,
       false as excluido
  from            etl_sgp_aula_plano_aula_dados_ok as ETL
       inner join aula as AUL 
		       on ue_id = aul_ue_id
		      and disciplina_id = aul_disciplina_id
		      and turma_id = aul_turma_id
		      and tipo_calendario_id = aul_tipo_calendario_id
		      and professor_rf = aul_professor_rf
		      and quantidade = aul_quantidade
		      and data_aula = aul_data_aula
		      and recorrencia_aula = aul_recorrencia_aula
		      and tipo_aula = aul_tipo_aula
		      and criado_em = aul_criado_em
		      and criado_por = aul_criado_por
		      and alterado_em = aul_alterado_em
		      and alterado_por = aul_alterado_por
		      and criado_rf = aul_criado_rf
		      and alterado_rf = aul_alterado_rf
 order by AUL.id