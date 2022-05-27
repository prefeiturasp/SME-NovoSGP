drop procedure if exists public.CARGA_RELATORIO_BID_CONSELHO;
CREATE PROCEDURE public.CARGA_RELATORIO_BID_CONSELHO(p_AnoLetivo int)
LANGUAGE plpgsql
as $$
declare
	ues record;
begin	
	for ues in
		select id, nome from ue order by id
	loop
		raise notice 'Escola % - %', ues.id, ues.nome;
		call RELATORIO_BID_CONSELHO(p_AnoLetivo, ues.Id);
		commit;
	end loop;
end $$