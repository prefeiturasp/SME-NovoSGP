do $$
declare 
	questaoComplementarId bigint;
	opcaoRespostaId bigint;
	
begin
	-- Questão 7
	select questao_complementar_id
		into questaoComplementarId
	  from opcao_questao_complementar oqc 
	 where opcao_resposta_id in (
	 select id 
	   from opcao_resposta 
	  where ordem = 1 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 7
		 ));
	
	select id 
		into opcaoRespostaId
	   from opcao_resposta 
	  where ordem = 2 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 7
		);

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	-- Questão 8
	select questao_complementar_id
		into questaoComplementarId
	  from opcao_questao_complementar oqc 
	 where opcao_resposta_id in (
	 select id 
	   from opcao_resposta 
	  where ordem = 1 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 8
		 ));
	
	select id 
		into opcaoRespostaId
	   from opcao_resposta 
	  where ordem = 2 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 8
		);

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	-- Questão 9
	select questao_complementar_id
		into questaoComplementarId
	  from opcao_questao_complementar oqc 
	 where opcao_resposta_id in (
	 select id 
	   from opcao_resposta 
	  where ordem = 1 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 9
		 ));
	
	select id 
		into opcaoRespostaId
	   from opcao_resposta 
	  where ordem = 2 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 9
		);

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	-- Questão 10
	select questao_complementar_id
		into questaoComplementarId
	  from opcao_questao_complementar oqc 
	 where opcao_resposta_id in (
	 select id 
	   from opcao_resposta 
	  where ordem = 1 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 10
		 ));
	
	select id 
		into opcaoRespostaId
	   from opcao_resposta 
	  where ordem = 2 
	    and questao_id in (
		 	select id 
		  from questao q 
		 where questionario_id in (select id from questionario q where tipo = 2) 
		   and ordem = 10
		);

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
end $$;
