do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	opcaoRespostaId bigint;
	
begin
	/************************/
	/* Questionário Etapa 3 */
	/************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 3', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Parecer AEE', 3, 1, NOW(), 'SISTEMA', '0');	

	-- 1 - Quais barreiras foram identificadas no contexto escolar que justificam a necessidade da oferta do AEE? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Quais barreiras foram identificadas no contexto escolar que justificam a necessidade da oferta do AEE?', '', true, 5, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

    -- 1.1 - Barreiras arquitetônicas (Exemplifique)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Barreiras arquitetônicas (Exemplifique)', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

    insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Barreiras arquitetônicas', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
    -- 1.2 - Barreiras nas comunicações e na informação (Exemplifique)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Barreiras nas comunicações e na informação (Exemplifique)', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

    insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Barreiras nas comunicações e na informação', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
    -- 1.3 - Barreiras atitudinais (Exemplifique)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Barreiras atitudinais (Exemplifique)', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

    insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Barreiras atitudinais', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');

	
	-- 2 - A criança necessita do Atendimento Educacional Especializado?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'A criança necessita do Atendimento Educacional Especializado?', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;
	
    -- 2.1 - Justifique a partir do estudo de caso quais critérios são elegíveis para o atendimento educacional especializado para este estudante.
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique a partir do estudo de caso quais critérios são elegíveis para o atendimento educacional especializado para este estudante.', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	-- 2.1 - Uma vez que não foram identificadas barreiras no contexto escolar do estudante, quais sugestões podem contribuir para a prática pedagógica?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Uma vez que não foram identificadas barreiras no contexto escolar do estudante, quais sugestões podem contribuir para a prática pedagógica?', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	-- 2.2 - Que sugestões podem ser dadas à unidade educacional para orientar a família?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Que sugestões podem ser dadas à unidade educacional para orientar a família?', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

	insert into opcao_questao_complementar(opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');

end $$;
