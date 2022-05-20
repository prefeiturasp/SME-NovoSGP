do $$
declare
	ues record;
	anoLetivo int := 2021;
begin	
	for ues in
		select id, nome from ue order by id
	loop
		raise notice 'Escola % - %', ues.id, ues.nome;
		raise notice 'Bimestre 1';
		call RELATORIO_BID_FREQUENCIA(anoLetivo, ues.Id, 1);
	
		raise notice 'Bimestre 2';
		call RELATORIO_BID_FREQUENCIA(anoLetivo, ues.Id, 2);
	
		raise notice 'Bimestre 3';
		call RELATORIO_BID_FREQUENCIA(anoLetivo, ues.Id, 3);
	
		raise notice 'Bimestre 4';
		call RELATORIO_BID_FREQUENCIA(anoLetivo, ues.Id, 4);
	
		commit;
	end loop;
end $$