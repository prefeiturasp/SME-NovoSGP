delete from pendencia_usuario 
where pendencia_id in (select id from pendencia where tipo between 1 and 6);

insert into pendencia_usuario (pendencia_id, usuario_id, criado_rf, criado_por, criado_em, alterado_rf, alterado_por, alterado_em)
	select p.id, u.id, p.criado_rf, p.criado_por, p.criado_em, p.alterado_rf, p.alterado_por, p.alterado_em
	  from pendencia p
	 inner join pendencia_fechamento pf on pf.pendencia_id = p.id
	 inner join fechamento_turma_disciplina ftd on ftd.id = pf.fechamento_turma_disciplina_id
	 inner join usuario u on u.rf_codigo = ftd.criado_rf
	 where tipo between 1 and 6;
