insert into plano_aula 
(aula_id, descricao, desenvolvimento_aula, recuperacao_aula, licao_casa, 
criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, migrado, excluido)
select distinct
       AUL.id as aula_id, 
       pla_descricao as descricao, 
       pla_desenvolvimento_aula as desenvolvimento_aula, 
       pla_recuperacao_aula as recuperacao_aula, 
       pla_licao_casa as licao_casa,
       aul_criado_em as criado_em, 
       aul_criado_por as criado_por, 
       aul_alterado_em as alterado_em, 
       aul_alterado_por as alterado_por, 
       aul_criado_rf as criado_rf, 
       aul_alterado_rf as alterado_rf,
       true as migrado,
       false as excluido       
  from            etl_sgp_aula_plano_aula_dados_ok as ETL
       inner join aula as AUL 
		       on right('000000' || ue_id,6) = right('000000' || aul_ue_id,6)
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
 group by AUL.id, pla_descricao, pla_desenvolvimento_aula, pla_recuperacao_aula, pla_licao_casa, 
          aul_criado_em, aul_criado_por, aul_alterado_em, aul_alterado_por, aul_criado_rf, aul_alterado_rf
 order by AUL.id
 