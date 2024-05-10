do $$
	declare 
		questaoId_justificativa_motivo_falta_outros bigint;
		opcaoresposta_outros_justificativa_falta bigint;
	    opcaoQuestaoComplementarIdExistente bigint;
	begin
		opcaoresposta_outros_justificativa_falta := (select or2.id from opcao_resposta or2 
		inner join questao q on q.id = or2.questao_id 
		where q.questionario_id = (select id from questionario where tipo = 8)
		and q.nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA' 
		and not or2.excluido 
		and not q.excluido
		and or2.nome = 'Outros'); 
	
		questaoId_justificativa_motivo_falta_outros := (select id from questao q 
		where q.questionario_id = (select id from questionario where tipo = 8)
		and q.nome_componente = 'JUSTIFICATIVA_MOTIVO_FALTA_OUTROS' and not q.excluido);
		
		opcaoQuestaoComplementarIdExistente := (select id from opcao_questao_complementar where opcao_resposta_id = questaoId_justificativa_motivo_falta_outros and questao_complementar_id = questaoId_justificativa_motivo_falta_outros);
	
		if (opcaoQuestaoComplementarIdExistente IS NOT NULL) then
		   insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			      values(opcaoresposta_outros_justificativa_falta, questaoId_justificativa_motivo_falta_outros, NOW(), 'SISTEMA', '0');
     	end if;
   end
$$;


