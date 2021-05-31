do $$
declare 
	ausencia record;
	existe_registro boolean;
begin
	for ausencia in
		select * from registro_ausencia_aluno raa where not excluido
	loop 
		select 1 into existe_registro
		  from registro_frequencia_aluno rfa 
		 where codigo_aluno = ausencia.codigo_aluno
		   and registro_frequencia_id = ausencia.registro_frequencia_id
		   and numero_aula = ausencia.numero_aula
		   and not excluido;
		if existe_registro is not true then
			insert into registro_frequencia_aluno (valor, registro_frequencia_id, numero_aula, codigo_aluno, alterado_em, alterado_por, alterado_rf, criado_em, criado_por, criado_rf)
			values(2,ausencia.registro_frequencia_id, ausencia.numero_aula, ausencia.codigo_aluno, ausencia.alterado_em, ausencia.alterado_por, ausencia.alterado_rf, ausencia.criado_em, ausencia.criado_por, ausencia.criado_rf);
		end if;
	end loop;
end $$;

do $$
declare 
	aulas record;
	count_aulas int := 0;
	existe_registro boolean;
	alunos_migracao record;	
begin
	for aulas in	
		select distinct(a.id) aulaId, 
		       a.quantidade quantidadeAulas, 
		       rf.id registroFrequenciaId,
		       t.turma_id as codigoTurma
		  from registro_frequencia_aluno rfa 
		 inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id
		 inner join aula a on a.id = rf.aula_id 
		 inner join turma t on t.turma_id = a.turma_id 
		 where t.ano_letivo = 2021
	loop 
		count_aulas := 0;
		while count_aulas < aulas.quantidadeAulas 
		loop 
			count_aulas := count_aulas + 1;
			for alunos_migracao in
				select mf.codigo_aluno 
			 	  from migracao_frequencia mf
				 where mf.codigo_turma = aulas.codigoTurma
				   and codigo_situacao in ('1', '10', '6', '13')
			loop			
				select 1 into existe_registro
				  from registro_frequencia_aluno
		  		 where registro_frequencia_id = aulas.registroFrequenciaId
		  		   and numero_aula = count_aulas
		  		   and codigo_aluno = alunos_migracao.codigo_aluno;
		  		  
		  		 if existe_registro is not true then
		  		 	insert into registro_frequencia_aluno (valor, registro_frequencia_id, numero_aula, codigo_aluno, criado_em, criado_por, criado_rf) 
		  		 	values(1, aulas.registroFrequenciaId, count_aulas, alunos_migracao.codigo_aluno, now(), 'Sistema', 'Sistema');
		  		 end if;
			end loop;
			
		end loop;
	end loop;
end $$;
 