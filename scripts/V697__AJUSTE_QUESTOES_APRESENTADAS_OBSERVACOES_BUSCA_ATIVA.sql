do $$
declare 
	questionarioRegAcaoId bigint;
	questaoId bigint;
	opcaoresposta_sim_conseguiu_contato bigint;
    opcaoresposta_nao_conseguiu_contato bigint;
    registroId bigint;
begin
	questionarioRegAcaoId := (select id from questionario where tipo = 8);
	
	--"QUESTOES APRESENTADAS DURANTE A VISITA" OPCIONAL
	update questao set obrigatorio = false where nome_componente = 'QUESTOES_OBS_DURANTE_VISITA' and questionario_id = questionarioRegAcaoId;
	
	--"OBSERVACOES" OBRIGATORIO EM CASO DE CONTATO COM RESPONSAVEL SER EFETUADO, CASO CONTRARIO OPCIONAL
	opcaoresposta_sim_conseguiu_contato := (select or2.id from opcao_resposta or2 
												inner join questao q on q.id = or2.questao_id 
												where q.nome_componente = 'CONSEGUIU_CONTATO_RESP' and or2.nome = 'Sim' and q.questionario_id = questionarioRegAcaoId);
											
	opcaoresposta_nao_conseguiu_contato := (select or2.id from opcao_resposta or2 
												inner join questao q on q.id = or2.questao_id 
												where q.nome_componente = 'CONSEGUIU_CONTATO_RESP' and or2.nome = 'Não' and q.questionario_id = questionarioRegAcaoId);										

	questaoId := (select id from questao where nome_componente = 'OBS_GERAL' and questionario_id = questionarioRegAcaoId);			
	
	update questao set ordem = 5, obrigatorio = true where id = questaoId;

	SELECT oqc.id INTO registroId
		FROM opcao_questao_complementar oqc
		WHERE oqc.opcao_resposta_id = opcaoresposta_sim_conseguiu_contato
		      AND oqc.questao_complementar_id = questaoId
		LIMIT 1;
	
	IF registroId IS NULL THEN	         
		insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			values(opcaoresposta_sim_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	END IF;

	--OBS_GERAL QUANDO NAO CONSEGUIU CONTATO RESPONSAVEL
	SELECT oqc.id INTO registroId
			FROM questao oqc
			WHERE oqc.questionario_id = questionarioRegAcaoId
		          and oqc.nome_componente = 'OBS_GERAL_NAO_CONTATOU_RESP'
	LIMIT 1;		

    IF registroId IS NULL THEN	 
	  --Observação
	    insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		       values(questionarioRegAcaoId, 2, 'Observação:', '', false, 2, '', NOW(), 'SISTEMA', '0', 12, '', 'OBS_GERAL_NAO_CONTATOU_RESP')
		RETURNING id INTO questaoId;
		
		insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			values(opcaoresposta_nao_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	
	END IF;
				         
end $$;