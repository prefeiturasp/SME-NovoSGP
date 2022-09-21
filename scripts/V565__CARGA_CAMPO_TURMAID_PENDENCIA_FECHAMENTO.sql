do $$
declare
	pendencia_db record;
begin	
	raise notice 'Inicio migração id turma pendência fechamento'; 
	for pendencia_db in 
		select distinct pf.pendencia_id, ft.turma_id  
		from pendencia_fechamento pf
		inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
		inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id  
  	loop
  		update pendencia set turma_id = pendencia_db.turma_id where id = pendencia_db.pendencia_id;
  	end loop;
    commit;  
    raise notice 'Final migração id turma pendência fechamento'; 
end $$  
