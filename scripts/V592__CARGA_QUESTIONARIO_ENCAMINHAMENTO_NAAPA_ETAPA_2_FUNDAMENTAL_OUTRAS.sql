do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	secaoId bigint;
begin
	/********************************/
	/* Questionário Seção 2 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Encaminhamento NAAPA Etapa 1 Seção 2 - Fundamental, Médio, EJA, CIEJA, MOVA, CMCT, ETEC', false, NOW(), 'SISTEMA', '0', 5)
		RETURNING id INTO questionarioId;
	
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, nome_componente, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Questões apresentadas', 1, 2, 'QUESTOES_APRESENTADAS_FUNDAMENTAL',NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 3 ,NOW(), 'SISTEMA', '0');
		
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 4 ,NOW(), 'SISTEMA', '0');

	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 5 ,NOW(), 'SISTEMA', '0');
		
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 6 ,NOW(), 'SISTEMA', '0');
		
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 7 ,NOW(), 'SISTEMA', '0');
		
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 8 ,NOW(), 'SISTEMA', '0');
		
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 9 ,NOW(), 'SISTEMA', '0');
		

	--Hipótese de escrita
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 0, 'Hipótese de escrita', '', false, 4, '', NOW(), 'SISTEMA', '0', 7, 'HIPOTESE_ESCRITA')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Pré-silábico', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Silábico sem valor sonoro', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Silábico com valor sonoro', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Silábico alfabético', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Alfabético', NOW(), 'SISTEMA', '0');
		
	
	--Ensino e aprendizagem
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 1, 'Ensino e aprendizagem', '', false, 9, '', NOW(), 'SISTEMA', '0', 7, 'ENSINO_APRENDIZAGEM')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Alfabetização', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Suspeita de Dislexia', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Dificuldade de Produção Texto', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Raciocínio Lógico', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Dificuldade leitura/compreensão', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Desatenção', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Desorganização', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Resistência ao registro', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Problemas de memorização', NOW(), 'SISTEMA', '0');

	--Observações Ensino e aprendizagem
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 2, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_ENSINO_APRENDIZAGEM');		

	--Permanência Escolar
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 3, 'Permanência Escolar', '', false, 9, '', NOW(), 'SISTEMA', '0', 7, 'PERMANENCIA_ESCOLAR')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Baixa frequência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Reprovação ano anterior', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Evasão escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Defasagem idade escolar', NOW(), 'SISTEMA', '0');
	
	--Observações Permanência Escolar
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 4, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_PERMANENCIA_ESCOLAR');		
	
	--Saúde/Saúde Mental/Dificuldades nas interações sociais
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 5, 'Saúde/Saúde Mental/Dificuldades nas interações sociais', '', false, 9, '', NOW(), 'SISTEMA', '0', 7, 'SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Doença Crônica', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Enurese', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Encoprese', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Questões fonoaudiológicas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Sonolência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Transtornos alimentares', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Saúde - outras questões', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Auto agressão/Auto mutilação', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Ideação suicida', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Mutismo seletivo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'TDA', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 12, 'TDAH', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 13, 'TOD', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 14, 'Saúde mental - outras questões', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 15, 'Agitação', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 16, 'Agressividade', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 17, 'Apatia', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 18, 'Comportamento Infantilizado', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 19, 'Dificuldade de Interação', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 20, 'Luto', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 21, 'Medo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 22, 'Resistência a regras', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 23, 'Timidez', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 24, 'Gravidez na adolescência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 25, 'Comportamento - outras questões', NOW(), 'SISTEMA', '0');

	--Observações Saúde/Saúde Mental/Dificuldades nas interações sociais
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 6, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS');		
	
	--Vulnerabilidade Social
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 7, 'Vulnerabilidade Social', '', false, 9, '', NOW(), 'SISTEMA', '0', 7, 'VULNERABILIDADE_SOCIAL')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Acolhimento Institucional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Frequenta ambientes de risco social', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Medidas socioeducativas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Pobreza extrema', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Responsável com S. Mental, Drogadição ou Deficiência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Suspeita de negligência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Suspeita de violência física', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Suspeita de violência sexual', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Suspeita de violência psicológica', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Suspeita de violência institucional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'Suspeita/Uso de substâncias psicoativas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 12, 'Trabalho infantil', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 13, 'Exploração sexual', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 14, 'Envolvimento com o tráfico de drogas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 15, 'Responsável com problemas de saúde', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 16, 'Responsável recluso', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 17, 'Em situação de rua ou na rua', NOW(), 'SISTEMA', '0');
		
	--Observações Vulnerabilidade Social 
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 8, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_VULNERABILIDADE_SOCIAL');		
end $$;