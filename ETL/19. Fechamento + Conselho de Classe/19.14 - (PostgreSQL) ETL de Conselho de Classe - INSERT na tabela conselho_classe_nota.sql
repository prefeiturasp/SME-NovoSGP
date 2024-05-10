insert into conselho_classe_nota (conselho_classe_aluno_id, componente_curricular_codigo, nota, conceito_id, justificativa, migrado, excluido, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select distinct 
       cca.id as conselho_classe_aluno_id,
       etl.disciplina_id as componente_curricular_codigo,
       etl.notacc as nota,
       etl.conceito_id_cc as conceito_id,
       etl.justificativa, 
       true as migrado,
       false as excluido,
       min(etl.criado_em_cc) as criado_em,
       'Migração/ETL' as criado_por,
       max(etl.alterado_em_CC) as alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf
  from            etl_sgp_fechamento_cc as etl
	   inner join turma                 as tur on tur.turma_id = cast(etl.turma_id as varchar)
	   inner join fechamento_turma      as fec on fec.turma_id = tur.id
	 								          and tur.turma_id = cast(etl.turma_id as varchar)
									          and (fec.periodo_escolar_id = etl.periodo_escolar_id 
									           or ((fec.periodo_escolar_id is null) and (etl.periodo_escolar_id is null)))
	   inner join conselho_classe       as coc on coc.fechamento_turma_id = fec.id
	   inner join conselho_classe_aluno as cca on cca.conselho_classe_id = coc.id
	                                          and cca.aluno_codigo = etl.aluno_codigo
 where fec.migrado = true
   and coc.migrado = true
   and cca.migrado = true
   and not(etl.notacc is null and etl.conceito_id_cc is null)
 group by cca.id, etl.disciplina_id, etl.notacc, etl.conceito_id_cc, etl.justificativa
 order by cca.id, etl.disciplina_id, etl.notacc, etl.conceito_id_cc, etl.justificativa;
 
-- select * from conselho_classe_nota where migrado = true
-- delete from conselho_classe_nota where migrado = true
