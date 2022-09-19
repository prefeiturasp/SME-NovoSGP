do $$
declare
	pendencia_db record;
begin	
	raise notice 'Inicio migração id turma pendência outros'; 
	for pendencia_db in 
		select distinct pf.pendencia_id, ft.turma_id  
		from pendencia_fechamento pf
		inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
		inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
		union all 
		select distinct ppf.pendencia_id, ppf.turma_id 
		from pendencia_professor ppf
		inner join pendencia_aula pa ON pa.pendencia_id = ppf.pendencia_id
		union all 
		select distinct pri.pendencia_id, pri.turma_id  
		from pendencia_registro_individual pri  
		union all
		select distinct eaee.pendencia_id, aee.turma_id
		from pendencia_encaminhamento_aee eaee 
		inner join encaminhamento_aee aee on eaee.encaminhamento_aee_id = aee.id     
  	loop
  		update pendencia set turma_id = pendencia_db.turma_id where id = pendencia_db.pendencia_id;
  	end loop;
    commit;  
    raise notice 'Final migração id turma pendência outros'; 
end $$  

