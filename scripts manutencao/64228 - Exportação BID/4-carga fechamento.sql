do $$
declare
	ues record;
	anoLetivo int := 2019;
begin	
	for ues in
		select id, nome from ue order by id
	loop
		raise notice 'Escola % - %', ues.id, ues.nome;
		call RELATORIO_BID_FECHAMENTO(anoLetivo, ues.Id);
		commit;
	end loop;
end $$