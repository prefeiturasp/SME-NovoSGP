insert into conselho_classe_aluno (conselho_classe_id, aluno_codigo, recomendacoes_aluno, recomendacoes_familia, anotacoes_pedagogicas, migrado, excluido, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, conselho_classe_parecer_id)
select distinct 
       coc.id as conselho_classe_id,
       etl.aluno_codigo,
       etl.recomendacoes_aluno,
       etl.recomendacoes_familia,
       etl.anotacoes_pedagocidas,
       true as migrado,
       false as excluido,
       min(etl.criado_em_cc) as criado_em,
       'Migração/ETL' as criado_por,
       max(etl.alterado_em_CC) as alterado_em,
       'Migração/ETL' as alterado_por,
       '0' as criado_rf,
       '0' as alterado_rf,
       case when etl.parecer = 1  then case when etl.serie in (1, 2, 4, 5) then 3
                                                                           else 1
                                       end
       		when etl.parecer = 2  then 4
       		when etl.parecer = 8  then 5
       		when etl.parecer = 10 then 2
       		else etl.parecer
       end as conselho_classe_parecer_id
  from            etl_sgp_fechamento_cc as etl
	   inner join turma                 as tur on tur.turma_id = cast(etl.turma_id as varchar)
	   inner join fechamento_turma      as fec on fec.turma_id = tur.id
	 								          and tur.turma_id = cast(etl.turma_id as varchar)
									          and (fec.periodo_escolar_id = etl.periodo_escolar_id 
									           or ((fec.periodo_escolar_id is null) and (etl.periodo_escolar_id is null)))
	   inner join conselho_classe       as coc on coc.fechamento_turma_id = fec.id
 where fec.migrado = true
   and coc.migrado = true
 group by coc.id, etl.aluno_codigo, etl.recomendacoes_aluno, etl.recomendacoes_familia, etl.anotacoes_pedagocidas, etl.serie, etl.parecer
 order by coc.id, etl.aluno_codigo;

-- select * from conselho_classe_aluno where migrado = true
-- delete from conselho_classe_aluno where migrado = true