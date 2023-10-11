do $$
declare
	configuracaoId bigint;
	periodoRelatorioId bigint;
	configuracaoDB record;
	periodoDB record;
	secaoConfDB record;
begin
	
	for configuracaoDB in
		select 1 semestre, ano_letivo, Min(periodo_inicio) periodo_inicio, Max(periodo_fim) periodo_fim
		from periodo_escolar pe 
		inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
		where ano_letivo in(2021, 2022) and modalidade = 1 and bimestre in (1,2) and not tc.excluido
		group by ano_letivo 
 		union all 
 		select 2 semestre, ano_letivo, Min(periodo_inicio) periodo_inicio, Max(periodo_fim) periodo_fim
		from periodo_escolar pe 
		inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
		where ano_letivo in(2021, 2022, 2023) and modalidade = 1 and bimestre in (3,4) and not tc.excluido
		group by ano_letivo 
		order by ano_letivo, semestre
	loop
		insert into configuracao_relatorio_pap (inicio_vigencia, fim_vigencia, tipo_periodicidade, criado_em, criado_por, criado_rf)
		values (configuracaoDB.periodo_inicio, configuracaoDB.periodo_fim, 'S', NOW(), 'SISTEMA', '0')
		RETURNING id into configuracaoId;
		
		insert into periodo_relatorio_pap (configuracao_relatorio_pap_id, periodo, criado_em, criado_por, criado_rf)
		values (configuracaoId, configuracaoDB.semestre, NOW(), 'SISTEMA', '0')
		RETURNING id into periodoRelatorioId;
		
		for periodoDB in
			select pe.id
			from periodo_escolar pe 
			inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
			where ano_letivo = configuracaoDB.ano_letivo  and modalidade = 1 
			and ((configuracaoDB.semestre = 1 and bimestre in(1,2)) or (configuracaoDB.semestre = 2 and bimestre in(3,4)))
			and not tc.excluido
		loop
			insert into periodo_escolar_relatorio_pap(periodo_relatorio_pap_id, periodo_escolar_id, criado_em, criado_por, criado_rf)
			values (periodoRelatorioId, periodoDB.id, NOW(), 'SISTEMA', '0');
		end loop;
		
		for secaoConfDB in
			select id from secao_relatorio_periodico_pap
		loop
			insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
			values (secaoConfDB.id, configuracaoId, NOW(), 'SISTEMA', '0');
		end loop;
	end loop;
end $$;