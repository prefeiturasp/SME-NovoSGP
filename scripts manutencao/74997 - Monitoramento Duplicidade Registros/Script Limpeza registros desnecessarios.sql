-- Fechamento Aluno
do $$
declare
	curDelete record;
begin
	for curDelete in 
		select fa.id
		  from fechamento_aluno fa 
		  left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id 
		  left join anotacao_fechamento_aluno afa on afa.fechamento_aluno_id = fa.id
		 where fn.id is null
		   and afa.id is null
		   and not fa.excluido
		   and extract(year from fa.criado_em) = 2022
		  limit 100
  	loop
  		delete from fechamento_aluno where id = curDelete.id;
  	end loop;
end $$ 

-- Fechamento Turma Disciplina
do $$
declare
	curDelete record;
begin
	for curDelete in 
		select ftd.id
		  from fechamento_turma_disciplina ftd 
		  left join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
		 where fa.id is null
		   and not ftd.excluido
		   and extract(year from ftd.criado_em) = 2022
--		  limit 100
  	loop
  		delete from pendencia_fechamento where fechamento_turma_disciplina_id = curDelete.id;
  		delete from fechamento_turma_disciplina where id = curDelete.id;
  	end loop;
end $$ 

-- Fechamento Turma 
do $$
declare
	curDelete record;
begin
	for curDelete in 
		select ft.id 
		  from fechamento_turma ft
		  left join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
		  left join conselho_classe cc on cc.fechamento_turma_id = ft.id
		 where not ft.excluido 
		   and ftd.id is null
		   and cc.id is null
		 limit 100
  	loop
  		delete from fechamento_turma where id = curDelete.id;
  	end loop;
end $$ 
