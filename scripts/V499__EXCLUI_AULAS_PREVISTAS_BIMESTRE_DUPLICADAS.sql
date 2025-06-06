update aula_prevista_bimestre set 
	excluido = true,
	alterado_em = current_date,
	alterado_por = 0
where id in (
	select 
		apb.id
	from (
		select aula_prevista_id
			, bimestre
			, max(id) as max_id
			, count(id)
		from aula_prevista_bimestre apb
		where not excluido
		group by 
			aula_prevista_id
			, 	bimestre
		having count(id) > 1
		order by 1 desc
	) tb
	inner join aula_prevista_bimestre apb 
		on apb.aula_prevista_id = tb.aula_prevista_id
		and apb.bimestre = tb.bimestre
		and apb.id <> tb.max_id
	order by apb.bimestre
);