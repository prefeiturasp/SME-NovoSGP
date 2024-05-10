insert into fechamento_aluno (fechamento_turma_disciplina_id,aluno_codigo,anotacao,migrado,excluido,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
select distinct
       ace.id as fechamento_turma_disciplina_id,
       etl.aluno_codigo,
       null as anotacao,
       true as migrado,
       false as excluido,
       min(etl.criado_em) as criado_em,
       'Migração/ETL' as criado_por,
       max(etl.alterado_em) as alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf
  from            etl_sgp_fechamento          as etl
	   inner join turma                       as tur on tur.turma_id = cast(etl.turma_id as varchar)
	   inner join fechamento_turma            as fec on fec.turma_id = tur.id
	 									            and tur.turma_id = cast(etl.turma_id as varchar)
											        and (fec.periodo_escolar_id = etl.periodo_escolar_id 
											         or ((fec.periodo_escolar_id is null) and (etl.periodo_escolar_id is null)))
	   inner join fechamento_turma_disciplina as ftd on ftd.disciplina_id = etl.disciplina_id
													and ftd.fechamento_turma_id = fec.id   
	   inner join etl_sgp_fechamento_acerto   as ace on ace.fechamento_turma_id = ftd.fechamento_turma_id
													and ace.disciplina_id = ftd.disciplina_id
 where fec.migrado = true
--   and etl.turma_id = 1992719
 group by ace.id, etl.aluno_codigo
 order by etl.aluno_codigo, ace.id;

-- select * from fechamento_aluno where migrado = true
-- delete from fechamento_aluno where migrado = true
