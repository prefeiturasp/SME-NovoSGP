do $$
declare
	pendencia_db record;
begin	
	raise notice 'Inicio migração id turma pendência diário de bordo'; 
	for pendencia_db in
			select distinct pdb.pendencia_id, t.id as turma_id
	        from pendencia_diario_bordo pdb 
	        inner join aula a on a.id = pdb.aula_id 
	        inner join turma t ON t.turma_id = a.turma_id 
  	loop
  		update pendencia set turma_id = pendencia_db.turma_id where id = pendencia_db.pendencia_id;
  	end loop;

	commit;  
    raise notice 'Final migração id turma pendência diário de bordo'; 
end $$ 