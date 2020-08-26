insert into fechamento_turma_disciplina (disciplina_id,migrado,excluido,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,situacao,justificativa,fechamento_turma_id)
select distinct
       etl.disciplina_id as disciplina_id,
       true as migrado,
       false as excluido,
       min(etl.criado_em) as criado_em,
       'Migração/ETL' as criado_por,
       max(etl.alterado_em) as alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf,
       3 as situacao,
       null as justificativa,
       fec.id as fechamento_turma_id
  from            etl_sgp_fechamento as etl
	   inner join turma              as tur on tur.turma_id = cast(etl.turma_id as varchar)
	   inner join fechamento_turma   as fec on fec.turma_id = tur.id
	 								       and tur.turma_id = cast(etl.turma_id as varchar)
									       and (fec.periodo_escolar_id = etl.periodo_escolar_id 
									        or ((fec.periodo_escolar_id is null) and (etl.periodo_escolar_id is null)))
 where fec.migrado = true
--   and etl.turma_id = 1992719
 group by etl.disciplina_id, fec.id
 order by fec.id, etl.disciplina_id;

-- select * from fechamento_turma_disciplina where migrado = true
-- delete from fechamento_turma_disciplina where migrado = true
 

-- ACERTO
-- Inserindo Disciplinas de Regência
insert into etl_sgp_fechamento_acerto (id, disciplina_id, fechamento_turma_id)
select fec.id, fec.disciplina_id, fec.fechamento_turma_id 
  from fechamento_turma_disciplina as fec
 where fec.migrado = true
   and fec.disciplina_id in (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
 order by fec.id, fec.disciplina_id, fec.fechamento_turma_id;


-- Inserindo as outras disciplinas (2, 7, 8, 89, 138) com o id da regência
insert into etl_sgp_fechamento_acerto (id, disciplina_id, fechamento_turma_id)
select distinct
       ace.id, fec.disciplina_id, fec.fechamento_turma_id 
  from            fechamento_turma_disciplina as fec
       inner join etl_sgp_fechamento_acerto   as ace 
               on ace.fechamento_turma_id = fec.fechamento_turma_id
 where fec.migrado = true
   and fec.disciplina_id in (2, 7, 8, 89, 138)
 order by ace.id, fec.disciplina_id, fec.fechamento_turma_id;


-- Inserindo Disciplinas Não Regência
insert into etl_sgp_fechamento_acerto (id, disciplina_id, fechamento_turma_id)
select fec.id, fec.disciplina_id, fec.fechamento_turma_id 
  from fechamento_turma_disciplina as fec
 where fec.migrado = true
   and fec.disciplina_id not in (508, 511, 1064, 1065, 1104, 1105, 1112, 1113, 1114, 1115, 1117, 1121, 1124, 1125, 1211, 1212, 1213, 1290, 1301)
   and fec.disciplina_id not in (2, 7, 8, 89, 138)
 order by fec.id, fec.disciplina_id, fec.fechamento_turma_id;
