do $$
declare
	configuracaoDB record;
	secaoDB record;
begin
	for configuracaoDB in
		select conf.id
		from configuracao_relatorio_pap conf 
		where tipo_periodicidade = 'B'
	loop
		for secaoDB in
			select id from secao_relatorio_periodico_pap 
		loop	
			insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
			values (secaoDB.id, configuracaoDB.id, NOW(), 'SISTEMA', '0');
		end loop;
	end loop;
end $$;