do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	opcaoRespostaId bigint;
	
begin
	/**************************/
	/* Questionário Plano AEE */
	/**************************/
	insert into questionario (nome, tipo, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Plano AEE', 2, false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;

	-- 1 - Qual o período de vigência deste planejamento (O plano AEE não deve ser maior que 3 meses)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Qual o período de vigência deste planejamento', 'O plano AEE não deve ser maior que 3 meses', true, 10, '', NOW(), 'SISTEMA', '0');

	-- 2 - Organização do AEE
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Organização do AEE', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Colaborativo', 'somente para as unidades educacionais da RME', NOW(), 'SISTEMA', '0');	
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Contraturno', '', NOW(), 'SISTEMA', '0');	

	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Itinerante', 'somente para as unidades educacionais da RME', NOW(), 'SISTEMA', '0');	
	
    -- 3 - Dias e horários de frequência do estudante no AEE
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Dias e horários de frequência do estudante no AEE', '', true, 11, '', NOW(), 'SISTEMA', '0');

	
    -- 4 - Forma de atendimento educacional especializado do estudante
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 4, 'Forma de atendimento educacional especializado do estudante', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Individual', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Em grupo', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');

	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Misto', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
    
    -- 5 - Objetivos do AEE
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 5, 'Objetivos do AEE', '', true, 2, '', NOW(), 'SISTEMA', '0');
    
    -- 6 - Orientações e ações para o desenvolvimento/atividades do AEE
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 6, 'Orientações e ações para o desenvolvimento/atividades do AEE', '', true, 2, '', NOW(), 'SISTEMA', '0');
	
    -- 7 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a unidade educacional de origem
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 7, 'Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras', 'Seleção de materiais, equipamentos e mobiliário para a unidade educacional de origem', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');

    -- 8 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 8, 'Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras', 'Seleção de materiais, equipamentos e mobiliário para a Sala de Recursos Multifuncionais', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');
	
	
    -- 9 - Mobilização dos Recursos Humanos da U.E./outras parcerias
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 9, 'Mobilização dos Recursos Humanos da U.E./outras parcerias', 'Interface / orientações na unidade educacional', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');
	
    -- 10 - Mobilização dos Recursos Humanos da U.E./outras parcerias
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 10, 'Mobilização dos Recursos Humanos da U.E./outras parcerias', 'Parcerias com profissionais externos à unidade educacional', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoRespostaId;

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values (opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');
	
	insert into opcao_resposta (questao_id, ordem, nome, observacao, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', '', NOW(), 'SISTEMA', '0');
	
end $$;
