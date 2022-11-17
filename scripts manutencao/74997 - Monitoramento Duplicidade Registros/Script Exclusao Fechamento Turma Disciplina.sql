do $$
declare
	fechamentoTurmaDisciplina record;
	fechamentoAluno record;
begin
	for fechamentoTurmaDisciplina in 
		select ftd.id
		  from monitoramento.fechamento_turma_disciplina_duplicado ftdd 
		  inner join fechamento_turma_disciplina ftd 
		  	on ftd.fechamento_turma_id = ftdd.fechamento_turma_id 
		  	and ftd.disciplina_id = ftdd.disciplina_id 
		  	and ftd.id <> ftdd.ultimo_id 
		  	limit 10
  	loop
  		-- Registros Fechamento Aluno
  		for fechamentoAluno in 
  			select fa.id 
  			  from fechamento_aluno fa
  			 where fa.fechamento_turma_disciplina_id = fechamentoTurmaDisciplina.id
		loop
			delete from fechamento_nota where fechamento_aluno_id = fechamentoAluno.id;
			
			delete from fechamento_aluno where id = fechamentoAluno.id;
		end loop;
	
		delete from fechamento_turma_disciplina where id = fechamentoTurmaDisciplina.id;
  		commit;
  	end loop;
end $$ 