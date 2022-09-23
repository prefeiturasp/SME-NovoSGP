update pendencia set turma_id = pendencia_db.turma_id 
from(
select distinct pdb.pendencia_id, t.id as turma_id
	        from pendencia_diario_bordo pdb 
	        inner join aula a on a.id = pdb.aula_id 
	        inner join turma t ON t.turma_id = a.turma_id 
) pendencia_db
where id = pendencia_db.pendencia_id;		