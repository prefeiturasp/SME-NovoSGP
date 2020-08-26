insert into conselho_classe (fechamento_turma_id, migrado, excluido, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
select distinct 
       fec.id as fechamento_turma_id,
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
 where fec.migrado = true
 group by fec.id 
 order by fec.id;

-- select * from conselho_classe where migrado = true
-- delete from conselho_classe where migrado = true
