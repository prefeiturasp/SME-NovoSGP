insert into fechamento_nota (disciplina_id,nota,conceito_id,migrado,excluido,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,sintese_id,fechamento_aluno_id)
select etl.disciplina_id,
       etl.nota,
       etl.conceito_id,
       true as migrado,
       false as excluido,
       etl.criado_em,
       'Migração/ETL' as criado_por,
       etl.alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf,
       etl.sintese_id,
       fal.id as fechamento_aluno_id
  from            etl_sgp_fechamento          as etl
	   inner join turma                       as tur on tur.turma_id = cast(etl.turma_id as varchar)  
	   inner join fechamento_turma            as fec on fec.turma_id = tur.id
	 									            and tur.turma_id = cast(etl.turma_id as varchar)
											        and (fec.periodo_escolar_id = etl.periodo_escolar_id 
											         or ((fec.periodo_escolar_id is null) and (etl.periodo_escolar_id is null)))
	   inner join fechamento_turma_disciplina as ftd on ftd.fechamento_turma_id = fec.id
													and ftd.disciplina_id = etl.disciplina_id
	   inner join etl_sgp_fechamento_acerto   as ace on ace.fechamento_turma_id = fec.id
												    and ace.disciplina_id = ftd.disciplina_id
	   inner join fechamento_aluno            as fal on fal.aluno_codigo = etl.aluno_codigo
											        and fal.fechamento_turma_disciplina_id = ace.id
 where (((etl.nota is not null) or (etl.sintese_id is not null) or (etl.conceito_id is not null)) -- Garante 1 dos 3 campos preenchidos
    or ((etl.nota is not null) and (etl.nota between 0 and 10))) -- Garante nota entre 0 e 10
   and fec.migrado = true
   and ftd.migrado = true
   and fal.migrado = true
   and etl.disciplina_id not in (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
--   and etl.turma_id = 1992719
 order by fal.id, etl.disciplina_id

-- select * from fechamento_nota where migrado = true
-- delete from fechamento_nota where migrado = true
 
