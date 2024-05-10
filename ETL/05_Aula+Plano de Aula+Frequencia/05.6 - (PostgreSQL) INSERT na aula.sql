insert into aula 
(ue_id, disciplina_id, turma_id, tipo_calendario_id, professor_rf, quantidade, data_aula, recorrencia_aula,
tipo_aula, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, migrado)
select right('000000' || aul_ue_id,6) as ue_id, 
       aul_disciplina_id as disciplina_id, 
       aul_turma_id as turma_id, 
       aul_tipo_calendario_id as tipo_calendario_id, 
       aul_professor_rf as professor_rf,
       aul_quantidade as quantidade, 
       aul_data_aula as data_aula, 
       aul_recorrencia_aula as recorrencia_aula, 
       aul_tipo_aula as tipo_aula, 
       aul_criado_em as criado_em, 
       aul_criado_por as criado_por, 
       aul_alterado_em as alterado_em, 
       aul_alterado_por as alterado_por, 
       aul_criado_rf as criado_rf, 
       aul_alterado_rf as alterado_rf,
       false as excluido,
       true as migrado
  from etl_sgp_aula_plano_aula_dados_ok as ETL
 order by id;
 