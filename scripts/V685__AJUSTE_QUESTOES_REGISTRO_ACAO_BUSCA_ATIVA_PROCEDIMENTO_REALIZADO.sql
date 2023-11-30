do $$
declare 
	questionarioRegAcaoId bigint;
	questaoId bigint;
	opcaoresposta_sim_conseguiu_contato bigint;
    opcaoresposta_nao_conseguiu_contato bigint;
    registroId bigint;
begin
	questionarioRegAcaoId := (select id from questionario where tipo = 8);
	
	opcaoresposta_sim_conseguiu_contato := (select or2.id from opcao_resposta or2 
												inner join questao q on q.id = or2.questao_id 
												where q.nome_componente = 'CONSEGUIU_CONTATO_RESP' and or2.nome = 'Sim' and q.questionario_id = questionarioRegAcaoId);
											
	opcaoresposta_nao_conseguiu_contato := (select or2.id from opcao_resposta or2 
												inner join questao q on q.id = or2.questao_id 
												where q.nome_componente = 'CONSEGUIU_CONTATO_RESP' and or2.nome = 'Não' and q.questionario_id = questionarioRegAcaoId);										

	questaoId := (select id from questao where nome_componente = 'PROCEDIMENTO_REALIZADO' and questionario_id = questionarioRegAcaoId);			
	
	update questao set ordem = 3 where nome_componente = 'OBS_GERAL' and questionario_id = questionarioRegAcaoId;--ordem 3 questao geral
	update questao set ordem = 4 where nome_componente = 'PROCEDIMENTO_REALIZADO' and questionario_id = questionarioRegAcaoId;--ordem 4 questao complementar

	SELECT oqc.id INTO registroId
		FROM opcao_questao_complementar oqc
		WHERE oqc.opcao_resposta_id = opcaoresposta_sim_conseguiu_contato
		      AND oqc.questao_complementar_id = questaoId
		LIMIT 1;
	
	IF registroId IS NULL THEN	         
		insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			values(opcaoresposta_sim_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	END IF;

	--Procedimento realizado QUANDO NAO CONSEGUIU CONTATO RESPONSAVEL
	SELECT oqc.id INTO registroId
			FROM questao oqc
			WHERE oqc.questionario_id = questionarioRegAcaoId
		          and oqc.nome_componente = 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP'
	LIMIT 1;		

    IF registroId IS NULL THEN	 
		insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
			values(questionarioRegAcaoId, 1, 'Procedimento realizado:', '', true, 3, '', NOW(), 'SISTEMA', '0', 6, '', 'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP')
			RETURNING id INTO questaoId;
		
		insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
			values(opcaoresposta_nao_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	
		insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 1, 'Ligação telefonica', NOW(), 'SISTEMA', '0');
		insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 2, 'Visita Domiciliar', NOW(), 'SISTEMA', '0');
	END IF;
				         
end $$;