do $$
declare
	fechamentoTurma record;
begin
	for fechamentoTurma in 
		select ftd.turma_id
			, ftd.periodo_escolar_id 
			, ftd.ultimo_id 
		  from monitoramento.fechamento_turma_duplicado ftd 
  	loop
  		delete from fechamento_turma 
  		where turma_id = fechamentoTurma.turma_id
  		  and periodo_escolar_id = fechamentoTurma.periodo_escolar_id
  		  and id <> fechamentoTurma.ultimo_id;

  		commit;
  	end loop;
end $$ 