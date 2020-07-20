update aula as a
  set ue_id = ue.ue_id
 from turma t 
 inner join ue on ue.id = t.ue_id
 where t.turma_id = a.turma_id
   and a.turma_id = a.ue_id
   and not a.excluido
