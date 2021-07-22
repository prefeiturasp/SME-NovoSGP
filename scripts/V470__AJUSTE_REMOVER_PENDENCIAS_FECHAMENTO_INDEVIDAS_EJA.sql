delete 
  from pendencia where id in (select distinct (p.id)
  							   from pendencia_fechamento pf  
							   inner join pendencia p on p.id = pf.pendencia_id 
							   inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
							   inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
							   inner join turma t on t.id = ft.turma_id 
							   where p.tipo = 4
							     and t.modalidade_codigo = 3
							     and ftd.disciplina_id in (1060,1061));	

delete 
  from pendencia_fechamento where id in (select distinct (pf.id)
										   from pendencia_fechamento pf  
										  inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id 
										  inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id 
										  inner join turma t on t.id = ft.turma_id 
										  where t.modalidade_codigo = 3
											and ftd.disciplina_id in (1060,1061));