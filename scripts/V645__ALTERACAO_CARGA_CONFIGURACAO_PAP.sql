do $$
declare
	configuracaoId bigint;
	periodoRelatorioId bigint;
	periodoFimSegundoBimestre date;
	periodoDB record;
begin
	
	select periodo_fim INTO periodoFimSegundoBimestre
	from periodo_escolar pe 
	inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
	where ano_letivo = 2023 and modalidade = 1 and bimestre = 2;

	select id into configuracaoId from configuracao_relatorio_pap 
	where not exists(select 1 from configuracao_relatorio_pap where tipo_periodicidade = 'B'); 

	update configuracao_relatorio_pap set fim_vigencia = periodoFimSegundoBimestre
	where id = configuracaoId;

	select id INTO periodoRelatorioId
	from periodo_relatorio_pap
	where configuracao_relatorio_pap_id = configuracaoId;

	update periodo_relatorio_pap set periodo = 1 
	where id = periodoRelatorioId;

	delete from periodo_escolar_relatorio_pap
	where periodo_relatorio_pap_id = periodoRelatorioId;

	for periodoDB in
		select pe.id
		from periodo_escolar pe 
		inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
		where ano_letivo = 2023 and modalidade = 1 and bimestre in(1, 2) and periodoRelatorioId is not null
	loop
		insert into periodo_escolar_relatorio_pap(periodo_relatorio_pap_id, periodo_escolar_id, criado_em, criado_por, criado_rf)
		values (periodoRelatorioId, periodoDB.id, NOW(), 'SISTEMA', '0');
	end loop;

	for periodoDB in
		select pe.id, bimestre, periodo_inicio, periodo_fim
		from periodo_escolar pe 
		inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
		where ano_letivo = 2023 and modalidade = 1 and bimestre in(3, 4)and periodoRelatorioId is not null
	loop
		insert into configuracao_relatorio_pap (inicio_vigencia, fim_vigencia, tipo_periodicidade, criado_em, criado_por, criado_rf)
		values (periodoDB.periodo_inicio, periodoDB.periodo_fim, 'B', NOW(), 'SISTEMA', '0')
		RETURNING id INTO configuracaoId;

		insert into periodo_relatorio_pap (configuracao_relatorio_pap_id, periodo, criado_em, criado_por, criado_rf)
		values (configuracaoId, periodoDB.bimestre, NOW(), 'SISTEMA', '0')
		RETURNING id INTO periodoRelatorioId;

		insert into periodo_escolar_relatorio_pap(periodo_relatorio_pap_id, periodo_escolar_id, criado_em, criado_por, criado_rf)
		values (periodoRelatorioId, periodoDB.id, NOW(), 'SISTEMA', '0');
	end loop;

end $$;