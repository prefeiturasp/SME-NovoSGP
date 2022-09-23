update pendencia set turma_id = pendencia_db.turma_id 
from(
select distinct pf.pendencia_id, ft.turma_id  
		from pendencia_fechamento pf
		inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
		inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id  
)pendencia_db
where id = pendencia_db.pendencia_id;