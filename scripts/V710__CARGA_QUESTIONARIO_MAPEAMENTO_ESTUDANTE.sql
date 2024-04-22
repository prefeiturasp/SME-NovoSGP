 do $$
declare 
	questionarioId bigint;
	questaoId bigint;
begin
	/********************************/
	/* Questionário Seção 1 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Mapeamento Estudante Etapa 1 Seção 1', false, NOW(), 'SISTEMA', '0', 9)
		RETURNING id INTO questionarioId;
	insert into secao_mapeamento_estudante (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 'Mapeamento Estudante Seção 1', 1, 1, NOW(), 'SISTEMA', '0', 'SECAO_1_MAPEAMENTO_ESTUDANTE');

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 1, 'Parecer conclusivo do ano anterior', '', true, 24, '', NOW(), 'SISTEMA', '0', 6, '', 'PARECER_CONCLUSIVO_ANO_ANTERIOR', true);
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 2, 'Turma do ano anterior', '', true, 1, '', NOW(), 'SISTEMA', '0', 6, '', 'TURMA_ANO_ANTERIOR', true);

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 3, 'Anotações pedagógicas do bimestre anterior', '', false, 2, '', NOW(), 'SISTEMA', '0', 12, '', 'ANOTACOES_PEDAG_BIMESTRE_ANTERIOR', true);
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 4, 'Classificado', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'CLASSIFICADO', false)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 5, 'Reclassificado', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'RECLASSIFICADO', false)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 6, 'Migrante', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'MIGRANTE', false)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 7, 'Acompanhado pelo SRM/CEFAI', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'ACOMPANHADO_SRM_CEFAI', true)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 8, 'Possui plano AEE', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'POSSUI_PLANO_AEE', true)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 9, 'Acompanhado pelo NAAPA', '', true, 3, '', NOW(), 'SISTEMA', '0', 4, '', 'ACOMPANHADO_NAAPA', true)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 10, 'Ações da rede de apoio', '', false, 18, '', NOW(), 'SISTEMA', '0', 12, '', 'ACOES_REDE_APOIO', false);	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 11, 'Ações de recuperação contínua', '', false, 18, '', NOW(), 'SISTEMA', '0', 12, '', 'ACOES_RECUPERACAO_CONTINUA', false);
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 12, 'Participa do PAP', '', true, 25, '', NOW(), 'SISTEMA', '0', 6, '', 'PARTICIPA_PAP', true);
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 13, 'Participa de projetos do Mais Educação', '', true, 25, '', NOW(), 'SISTEMA', '0', 6, '', 'PARTICIPA_MAIS_EDUCACAO', true);	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 14, 'Projeto de fortalecimento das aprendizagens', '', true, 25, '', NOW(), 'SISTEMA', '0', 12, '', 'PROJETO_FORTALECIMENTO_APRENDIZAGENS', true);		
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 15, 'Programa São Paulo Integral', '', true, 3, '', NOW(), 'SISTEMA', '0', 6, '', 'PROGRAMA_SAO_PAULO_INTEGRAL', true)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');		
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 16, 'Qual a hipótese de escrita do estudante', '', false, 1, '', NOW(), 'SISTEMA', '0', 6, '', 'HIPOTESE_ESCRITA', true);	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 17, 'Avaliações externas (Prova São Paulo)', '', false, 26, '', NOW(), 'SISTEMA', '0', 12, '', 'AVALIACOES_EXTERNAS_PROVA_SP', true);		
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 18, 'Observações sobre a avaliação processual do estudante', '', true, 18, '', NOW(), 'SISTEMA', '0', 12, '', 'OBS_AVALIACAO_PROCESSUAL', false);			
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 19, 'Frequência', '', true, 4, '', NOW(), 'SISTEMA', '0', 6, '', 'FREQUENCIA', true)
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Frequente', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não Frequente', NOW(), 'SISTEMA', '0');	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente, somente_leitura)
		values(questionarioId, 20, 'Quantidade de registros de busca ativa', '', true, 13, '', NOW(), 'SISTEMA', '0', 6, '', 'QDADE_REGISTROS_BUSCA_ATIVA', true);			

end $$;