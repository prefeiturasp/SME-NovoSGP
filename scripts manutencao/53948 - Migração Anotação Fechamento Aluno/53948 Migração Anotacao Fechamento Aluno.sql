do $$
declare
	anotacoes record;

begin
	for anotacoes in 
		select fa.id, fa.anotacao
			, fa.criado_em, fa.criado_por, fa.criado_rf
			, fa.alterado_em, fa.alterado_por, fa.alterado_rf
		  from fechamento_aluno fa 
		  left join anotacao_fechamento_aluno afa on afa.fechamento_aluno_id = fa.id
		 where not fa.excluido 
		   and not fa.anotacao is null
		   and afa.id is null
  	loop
  		insert into anotacao_fechamento_aluno (fechamento_aluno_id, anotacao, criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
  		values (anotacoes.id, anotacoes.anotacao, anotacoes.criado_em, anotacoes.criado_por, anotacoes.criado_rf, anotacoes.alterado_em, anotacoes.alterado_por, anotacoes.alterado_rf);
  	
  		commit;
  	end loop;
end $$ 