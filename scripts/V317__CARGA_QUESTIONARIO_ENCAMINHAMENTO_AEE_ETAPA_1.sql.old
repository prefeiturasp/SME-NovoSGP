do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	questaoComplementar2Id bigint;
	
begin
	/********************************/
	/* Questionário Seção 1 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 1 Seção 1', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Informações escolares', 1, 1, NOW(), 'SISTEMA', '0');


	-- Informações Escolares
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 0, '', '', false, 7, '', NOW(), 'SISTEMA', '0');

	-- 1 - Justificativa de Ausências
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justificativa de ausências', '', false, 2, '', NOW(), 'SISTEMA', '0');

	-- 2 - Estudante está ou esteve matriculado em classe ou escola especializada
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Estudante está ou esteve matriculado em classe ou escola especializada', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	-- 3 - Qual último período/ano em que o estudante frequentou ​classe ou escola especializada (Obrigatório)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Qual último período/ano em que o estudante frequentou ​classe ou escola especializada', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Sim', NOW(), 'SISTEMA', '0');

	-- 3 - Qual último período/ano em que o estudante frequentou ​classe ou escola especializada (Não Obrigatório)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Qual último período/ano em que o estudante frequentou ​classe ou escola especializada', '', false, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 2, 'Não', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 3, 'Não sei', NOW(), 'SISTEMA', '0');


	/********************************/
	/* Questionário Seção 1 Etapa 2 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 1 Seção 2', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Descrição do encaminhamento', 1, 2, NOW(), 'SISTEMA', '0');

	-- 1 - Por qual(is) motivo(s) a sua unidade educacional está encaminhando o estudante ao Atendimento Educacional Especializado (AEE)?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Por qual(is) motivo(s) a sua unidade educacional está encaminhando o estudante ao Atendimento Educacional Especializado (AEE)?', '', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 2 - O estudante tem diagnóstico e/ou laudo? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'O estudante tem diagnóstico e/ou laudo?', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	-- upload
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 0, '', '', true, 6, '.PDF, .PNG, .JPEG', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 2, 'Não', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 3, 'Não sei', NOW(), 'SISTEMA', '0');

	-- 3 - Quais atividades escolares o estudante mais gosta de fazer?  
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Quais atividades escolares o estudante mais gosta de fazer?', '', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 4 - O que o estudante faz que chama a sua atenção?  
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 4, 'O que o estudante faz que chama a sua atenção?', '', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 5 - Para o estudante, quais atividades escolares são mais difíceis? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 5, 'Para o estudante, quais atividades escolares são mais difíceis? ', '', true, 4, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	-- Por quê? Opcional
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Por quê? ', '', false, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Leitura', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 2, 'Escrita', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 3, 'Atividades em grupo', NOW(), 'SISTEMA', '0');

	-- Por quê? Obrigatório
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Por quê?', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 4, 'Outros', NOW(), 'SISTEMA', '0');

	-- 6 - Diante das dificuldades apresentadas acima, quais estratégias pedagógicas foram feitas em sala de aula antes do encaminhamento ao AEE? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 6, 'Diante das dificuldades apresentadas acima, quais estratégias pedagógicas foram feitas em sala de aula antes do encaminhamento ao AEE?? ', 'Quais avanços foram observados? (Descreva detalhadamente as atividades realizadas e seus objetivos)', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 7 - O estudante recebe algum atendimento clínico ou participa de outras atividades além da classe comum? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 7, 'O estudante recebe algum atendimento clínico ou participa de outras atividades além da classe comum?', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	-- 7.1
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Detalhamento de atendimento clínico', '', true, 8, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 2, 'Não', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 3, 'Não Sei', NOW(), 'SISTEMA', '0');

	-- 8 - Você acha que o estudante necessita de algum outro tipo de atendimento? 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 8, 'Você acha que o estudante necessita de algum outro tipo de atendimento?', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

	-- 8.1 - Selecione os tipos de atendimento 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Selecione os tipos de atendimento', '', true, 4, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 2, 'Não', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, null, 3, 'Não Sei', NOW(), 'SISTEMA', '0');

	-- 8.1 opções
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 1, 'PAP', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 2, 'SRM', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 3, 'Mais educação (São Paulo Integral)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 4, 'Imprensa Jovem', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 5, 'Academia Estudantil de Letras (AEL)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 6, 'Xadrez', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, null, 7, 'Robótica', NOW(), 'SISTEMA', '0');

	-- 8.1.1 Descreva
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Descreva o tipo de atendimento', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementar2Id;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementarId, questaoComplementar2Id, 8, 'Outros', NOW(), 'SISTEMA', '0');

	-- 9 - Quais informações relevantes a este encaminhamento foram levantadas junto à família do estudante?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 9, 'Quais informações relevantes a este encaminhamento foram levantadas junto à família do estudante?', '', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 10 - Documentos relevantes a este encaminhamento 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 10, 'Documentos relevantes a este encaminhamento', 'Envie arquivos de texto, imagem, áudio e/ou vídeo que possam mostrar produções do estudante ou sua interação com o grupo de colegas, por exemplo.', false, 6, '', NOW(), 'SISTEMA', '0');

	-- 11 - Observações adicionais (se necessário)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 11, 'Observações adicionais (se necessário)', '', false, 2, '', NOW(), 'SISTEMA', '0');
end $$;