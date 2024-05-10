do $$
declare
	_turmaId integer := 0;
	_pendencia_db record;
begin	
	for _pendencia_db in 
		select p.id  
			from pendencia p 
			inner join pendencia_diario_bordo pdb ON pdb.pendencia_id = p.id 
			inner join turma t on t.id = p.turma_id 
			inner join aula a on a.id = pdb.aula_id 
			inner join turma t2 on t2.turma_id  = a.turma_id 
			where p.criado_em >= '2022-03-01' and t.ano_letivo = 2022
			and (t.id <> t2.id or p.descricao not like '%'||t.nome||'%')
			and not p.excluido 
			and p.tipo = 9
			group by p.id

  	loop
  		_turmaId := (select t.id from pendencia_diario_bordo pdb
  							inner join pendencia p on p.id = pdb.pendencia_id 
  							inner join aula a on a.id = pdb.aula_id 
  							inner join turma t on t.turma_id = a.turma_id 
							where pdb.pendencia_id = _pendencia_db.id 
							and p.descricao like '%'||t.nome||'%' limit 1);

	  	if _turmaId <> 0 and _turmaId is not null then		  
			update pendencia set turma_id = _turmaId where id = _pendencia_db.id and turma_id <> _turmaId and _turmaId <> 0; 
			delete 	
				from 
					pendencia_diario_bordo as pdb 
	 	   		using 
		 	   		aula as a,  
		 	   		turma as t
			   	where 
			   		a.id = pdb.aula_id
			   		and t.turma_id  = a.turma_id 
			   		and pdb.pendencia_id = _pendencia_db.id and t.id <> _turmaId and _turmaId <> 0;		 
		else
			delete from pendencia_diario_bordo where pendencia_id = _pendencia_db.id; --nenhuma pendencia db eh da turma da pendencia
			update pendencia set turma_id = null, excluido = true where id = _pendencia_db.id;
	  	end if; 
  	end loop;
end $$ 



