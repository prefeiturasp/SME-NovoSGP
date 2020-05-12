insert into compensacao_ausencia_aluno
(compensacao_ausencia_id,codigo_aluno,qtd_faltas_compensadas,notificado,criado_em,criado_por,alterado_em,alterado_por,criado_rf,alterado_rf,excluido)
select distinct 
       cau.id as compensacao_ausencia_id,
       etl.codigo_aluno,
       etl.qtd_faltas_compensadas,
       true as notificado,
	   etl.criado_em,
	   'Migração/ETL' as criado_por,
	   etl.alterado_em,
	   'Migração/ETL' as alterado_por,
	   '0' as criado_rf,
	   '0' as alterado_rf,       
	   false as excluido
  from            etl_sgp_compensacao_ausencia as etl
       inner join compensacao_ausencia         as cau 
		       on cau.bimestre = etl.bimestre
		      and cau.disciplina_id = etl.disciplina_id
		      and cau.nome = etl.nome
		      and cau.descricao = etl.descricao
		      and cau.criado_em = etl.criado_em
		      and cau.alterado_em = etl.alterado_em
		      and cau.ano_letivo = etl.ano_letivo
       inner join turma                        as tur
               on tur.id = cau.turma_id
       