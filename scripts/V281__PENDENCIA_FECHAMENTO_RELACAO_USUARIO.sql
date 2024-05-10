insert into pendencia_usuario (pendencia_id, usuario_id, criado_rf, criado_por, criado_em, alterado_rf, alterado_por, alterado_em)
	select p.id, u.id, p.criado_rf, p.criado_por, p.criado_em, p.alterado_rf, p.alterado_por, p.alterado_em
	  from pendencia p
	 inner join usuario u on u.rf_codigo = p.criado_rf
	 where tipo between 1 and 6

 