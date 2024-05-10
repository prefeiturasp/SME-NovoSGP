update pendencia as p 
   set excluido = true
 from pendencia_fechamento pf 
 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
 inner join componente_curricular cc on cc.id = ftd.disciplina_id 
 where pf.pendencia_id = p.id 
   and not p.excluido
   and p.tipo = 4
   and not cc.permite_registro_frequencia 