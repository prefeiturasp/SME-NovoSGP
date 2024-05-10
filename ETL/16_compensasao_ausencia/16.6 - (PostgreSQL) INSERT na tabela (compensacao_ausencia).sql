insert into compensacao_ausencia
(bimestre,disciplina_id,turma_id,nome,descricao,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,excluido,migrado,ano_letivo)
select distinct 
       etl.bimestre,
	   etl.disciplina_id,
	   tur.id as turma_id,
	   etl.nome,
	   etl.descricao,
	   etl.criado_em,
	   'Migração/ETL' as criado_por,
	   etl.alterado_em,
	   'Migração/ETL' as alterado_por,
	   '0' as criado_rf,
	   '0' as alterado_rf,
	   false as excluido,
	   true as migrado,
	   etl.ano_letivo
  from etl_sgp_compensacao_ausencia as etl
       inner join turma as tur on tur.turma_id = cast(etl.turma_id as varchar)
       
