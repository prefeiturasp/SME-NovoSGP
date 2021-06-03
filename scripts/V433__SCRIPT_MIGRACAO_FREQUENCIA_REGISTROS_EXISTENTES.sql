do $$
declare 
	ausencia record;
	existe_registro_frequencia_aluno boolean;
begin
	
	for ausencia in
		select * from registro_ausencia_aluno raa where not excluido
	loop 
		select 1 into existe_registro_frequencia_aluno
		  from registro_frequencia_aluno rfa 
		 where codigo_aluno = ausencia.codigo_aluno
		   and registro_frequencia_id = ausencia.registro_frequencia_id
		   and numero_aula = ausencia.numero_aula
		   and not excluido;
		if existe_registro_frequencia_aluno is not true then
			insert into registro_frequencia_aluno (valor, registro_frequencia_id, numero_aula, codigo_aluno, alterado_em, alterado_por, alterado_rf, criado_em, criado_por, criado_rf, migrado)
			values(2,ausencia.registro_frequencia_id, ausencia.numero_aula, ausencia.codigo_aluno, ausencia.alterado_em, ausencia.alterado_por, ausencia.alterado_rf, ausencia.criado_em, ausencia.criado_por, ausencia.criado_rf, ausencia.migrado);
		end if;
	end loop;
end $$