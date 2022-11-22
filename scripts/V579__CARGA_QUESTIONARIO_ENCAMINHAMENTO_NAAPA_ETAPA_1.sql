do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	
begin
	/********************************/
	/* Questionário Seção 1 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento NAAPA Etapa 1 Seção 1', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Informações do Estudante', 1, 1, NOW(), 'SISTEMA', '0');


	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 0, 'Data de entrada da queixa', 'Data', true, 14, '', NOW(), 'SISTEMA', '0', 3, null, null);

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 1, 'Prioridade', '', true, 4, '', NOW(), 'SISTEMA', '0', 3, null, null)
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Normal', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Prioritária', NOW(), 'SISTEMA', '0');

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 2, 'Porta de entrada', '', true, 4, '', NOW(), 'SISTEMA', '0', 6, null, null)
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Contato Telefônico', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'E-mail', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Família', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Grupo de Trabalho', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Memorando / Relatório Escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Ofício / MP / Vara da Infância', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Rede de Proteção Social', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Supervisão Escolar / Outros Setores DRE', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Busca Ativa Escolar (NAAPA)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Busca Ativa Escolar (Unicef)', NOW(), 'SISTEMA', '0');


	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 3, 'NIS (Número de Identificação Social)', 'Informe o NIS', false, 13, '', NOW(), 'SISTEMA', '0', 4, 11, null);
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 4, 'CNS (Cartão SUS)', 'Informe o CNS', false, 13, '', NOW(), 'SISTEMA', '0', 4, 15, null);

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 5, 'Contato dos responsáveis', '', false, 16, '', NOW(), 'SISTEMA', '0', 12, null, null);

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 6, 'Endereço residencial', '', false, 16, '', NOW(), 'SISTEMA', '0', 12, null, null);

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 7, 'Nome da mãe', 'Informe o nome da mãe', true, 1, '', NOW(), 'SISTEMA', '0', 6, 150, null);
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 8, 'Gênero', '', true, 4, '', NOW(), 'SISTEMA', '0', 6, null, null)
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Masculino', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Feminino', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Outro', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 9, 'Grupo étnico (autodenominação)', 'Grupo étnico', true, 4, '', NOW(), 'SISTEMA', '0', 6, null, null)
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Branco', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Negro', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Pardo', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Amarelo', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Indígena', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Não declarado', NOW(), 'SISTEMA', '0');		
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 10, 'Criança/Estudante é imigrante (autodenominação)', '', true, 3, '', NOW(), 'SISTEMA', '0', 6, null, null)
	RETURNING id INTO questaoId;		

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');			

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 11, 'Responsável/Cuidador é imigrante', '', false, 3, '', NOW(), 'SISTEMA', '0', 12, null, null)
	RETURNING id INTO questaoId;		

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');		

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 12, 'UBS de referência', 'Informe a UBS', false, 1, '', NOW(), 'SISTEMA', '0', 12, 200, null);					
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 13, 'CRAS de referência', 'Informe o CRAS', false, 1, '', NOW(), 'SISTEMA', '0', 12, 200, null);		
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 14, 'Atividades de contraturno', '', false, 17, '', NOW(), 'SISTEMA', '0', 12, null, null);		
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 15, 'Descrição do encaminhamento', '', true, 2, '', NOW(), 'SISTEMA', '0', 12, null, null);		
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara)
		values(questionarioId, 16, 'Anexos', '', false, 6, '', NOW(), 'SISTEMA', '0', 12, null, null);		

end $$;