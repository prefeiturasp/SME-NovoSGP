drop procedure if exists public.CARGA_RELATORIO_BID_FREQUENCIA;
CREATE PROCEDURE public.CARGA_RELATORIO_BID_FREQUENCIA(p_AnoLetivo int)
LANGUAGE plpgsql
as $$
declare
	ues record;
begin	
	for ues in
		select id, nome from ue order by id
	loop
		raise notice 'Escola % - %', ues.id, ues.nome;
		raise notice 'Bimestre 1';
		call RELATORIO_BID_FREQUENCIA(p_AnoLetivo, ues.Id, 1);
	
		raise notice 'Bimestre 2';
		call RELATORIO_BID_FREQUENCIA(p_AnoLetivo, ues.Id, 2);
	
		raise notice 'Bimestre 3';
		call RELATORIO_BID_FREQUENCIA(p_AnoLetivo, ues.Id, 3);
	
		raise notice 'Bimestre 4';
		call RELATORIO_BID_FREQUENCIA(p_AnoLetivo, ues.Id, 4);
	
		commit;
	end loop;
end $$