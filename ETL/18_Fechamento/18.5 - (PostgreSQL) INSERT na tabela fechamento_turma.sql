insert into fechamento_turma (turma_id,periodo_escolar_id,migrado,excluido,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf)
select distinct
       tur.id as turma_id,
       etl.periodo_escolar_id as periodo_escolar_id,
       true as migrado,
       false as excluido,
       min(etl.criado_em) as criado_em,
       'Migração/ETL' as criado_por,
       max(etl.alterado_em) as alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf
  from            etl_sgp_fechamento as etl
       inner join turma              as tur on tur.turma_id = cast(etl.turma_id as varchar)
-- where etl.turma_id = 1992719
 group by tur.id, etl.periodo_escolar_id
 order by tur.id, etl.periodo_escolar_id;

-- select * from fechamento_turma where migrado = true
-- delete from fechamento_turma where migrado = true
