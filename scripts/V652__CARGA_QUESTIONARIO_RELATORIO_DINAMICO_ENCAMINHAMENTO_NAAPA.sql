do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	secaoId bigint;
	opcaoresposta_adoececomfrequencia bigint;
	opcaoresposta_doencacronica bigint;
	questaocomplementar_adoececomfrequenciaid bigint;
	questaocomplementar_doencacronicaid bigint;
begin
	/********************************/
	/* Questionário Informações */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Relatório Dinâmico Encaminhamento NAAPA', false, NOW(), 'SISTEMA', '0', 7)
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Informações do Estudante Relatório dinâmico', 1, 1, NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 0, 'Data de entrada da queixa', '', false, 10, '', NOW(), 'SISTEMA', '0', 6, 'Data', 'DATA_ENTRADA_QUEIXA');

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 1, 'Prioridade', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'Prioridade', 'PRIORIDADE')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Normal', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Prioritária', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 3, 'Porta de entrada', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'Porta de entrada', 'PORTA_ENTRADA')
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
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 4, 'Gênero', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'Gênero', 'GENERO')
		RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Masculino', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Feminino', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Outro', NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 5, 'Grupo étnico (autodenominação)', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'Grupo étnico', 'GRUPO_ETNICO')
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
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 6, 'Criança/Estudante é imigrante (autodenominação)', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'ESTUDANTE_MIGRANTE')
	RETURNING id INTO questaoId;		

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');			

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 7, 'Responsável/Cuidador é imigrante', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'RESPONSAVEL_MIGRANTE')
	RETURNING id INTO questaoId;		

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');		
	
	insert into questao(questionario_id, ordem, nome, placeholder, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, tamanho, mascara, nome_componente)
		values(questionarioId, 8, 'Aplicação do fluxo de alerta', '', '', true, 9, '', NOW(), 'SISTEMA', '0', 6, null, null, 'FLUXO_ALERTA')
		RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Fluxo de violência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Notificação feita pelo NAAPA', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Notificação feita pela escola', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Busca ativa escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Busca ativa escolar - Unicef', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Fluxo da Gravidez', NOW(), 'SISTEMA', '0');
		
	/********************************/
	/* Questionário Infantil */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Relatório Dinâmico Encaminhamento NAAPA - Infantil', false, NOW(), 'SISTEMA', '0', 7)
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, nome_componente, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Questões apresentadas infantil Relatório dinâmico', 1, 2, 'QUESTOES_APRESENTADAS_INFANTIL',NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 1 ,NOW(), 'SISTEMA', '0');
		
		--Questões no agrupamento desenvolvimento
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 9, 'Questões no agrupamento desenvolvimento', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_DESENVOLVIMENTO')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Agitação motora', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Aparente sofrimento diante das rotinas propostas para seu agrupamento', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Dificuldades nas habilidades motoras', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Dificuldades nas habilidades de comunicação', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Dificuldades nas interações com os adultos', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Dificuldades nas interações com outras crianças', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Dificuldades no desenvolvimento da comunicação verbal', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Embotamento social', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Isolamento', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Medo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'Não brinca', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 12, 'Tristeza', NOW(), 'SISTEMA', '0');
		
		--Questões no agrupamento proteção
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 10, 'Questões no agrupamento proteção', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_PROTECAO')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Em situação de risco social', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Em situação de rua ou na rua', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Família em situação de extrema pobreza', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Responsáveis com transtornos mentais', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Suspeita de trabalho infantil', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Suspeita de Violência Estrutural', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Suspeita de violência física', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Suspeita de violência institucional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Suspeita de violência negligencial', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Suspeita de violência psicológica', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'Suspeita de Violência química', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 12, 'Suspeita de violência sexual', NOW(), 'SISTEMA', '0');
		
			--Questões no agrupamento promoção de cuidados
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 11, 'Questões no agrupamento promoção de cuidados', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_PROMOCAO_CUIDADOS')
	RETURNING id INTO questaoId;

	--Opções Questões no agrupamento promoção de cuidados
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Carteira de vacinas atrasada', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Adoece com frequência sem receber cuidados médicos', NOW(), 'SISTEMA', '0')
	RETURNING id INTO opcaoresposta_adoececomfrequencia;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Baixo peso', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Excesso de peso', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Rotina de sono alterada', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Doença crônica ou em tratamento de longa duração', NOW(), 'SISTEMA', '0')
	RETURNING id INTO opcaoresposta_doencacronica;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Frequência Irregular/excesso de faltas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Enurese e Encoprese', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Saúde bucal comprometida', NOW(), 'SISTEMA', '0');
		
	--Questoes complementares
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 1, 'Selecione um tipo', 'Adoece com frequência sem receber cuidados médicos', true, 9, '', NOW(), 'SISTEMA', '0', 6, 'TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS')
		RETURNING id INTO questaoComplementar_adoececomfrequenciaId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_adoececomfrequencia, questaoComplementar_adoececomfrequenciaId, NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 1, 'Selecione um tipo', 'Doença crônica ou em tratamento de longa duração', true, 9, '', NOW(), 'SISTEMA', '0', 6, 'TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO')
		RETURNING id INTO questaoComplementar_doencacronicaId;

	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_doencacronica, questaoComplementar_doencacronicaId, NOW(), 'SISTEMA', '0');
		
	--Opções complementares Adoece com frequência sem receber cuidados médicos
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 1, 'Assadura', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 2, 'Bronquite', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 3, 'Coriza', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 4, 'Diarreia', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 5, 'Doenças de pele', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 6, 'Dor de ouvido', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 7, 'Febre', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 8, 'Gripes', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 9, 'Manchas na pele', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 10, 'Piolho', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 11, 'Sarna', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 12, 'Vômito', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_adoececomfrequenciaId, 13, 'Outras', NOW(), 'SISTEMA', '0');
		
	--Opções complementares Doença crônica ou em tratamento de longa duração
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 1, 'Anemia falciforme', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 2, 'Asma', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 3, 'Bronquite', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 4, 'Câncer', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 5, 'Diabetes', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 6, 'Doença hepática', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 7, 'Doença renal', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 8, 'Doenças do aparelho digestivo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 9, 'Epilepsia', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 10, 'Imunossuprimido', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 11, 'Soropositivo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 12, 'Transplantados', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 13, 'Tuberculose', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoComplementar_doencacronicaId, 14, 'Outras', NOW(), 'SISTEMA', '0');
		
	/********************************/
	/* Questionário demais modalidades */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Relatório Dinâmico Encaminhamento NAAPA - Fundamental, Médio, EJA, CIEJA, MOVA, CMCT, ETEC', false, NOW(), 'SISTEMA', '0', 7)
		RETURNING id INTO questionarioId;
	
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, nome_componente, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Questões apresentadas fundamental Relatório dinâmico', 1, 2, 'QUESTOES_APRESENTADAS_FUNDAMENTAL',NOW(), 'SISTEMA', '0')
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
		values(questionarioId, 12, 'Hipótese de escrita', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'HIPOTESE_ESCRITA')
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
		values(questionarioId, 13, 'Ensino e aprendizagem', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'ENSINO_APRENDIZAGEM')
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
		
	--Permanência Escolar
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 14, 'Permanência Escolar', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'PERMANENCIA_ESCOLAR')
	RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Baixa frequência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Reprovação ano anterior', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Evasão escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Defasagem idade escolar', NOW(), 'SISTEMA', '0');
		
	--Saúde/Saúde Mental/Dificuldades nas interações sociais
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 15, 'Saúde/Saúde Mental/Dificuldades nas interações sociais', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS')
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
		
		--Vulnerabilidade Social
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 16, 'Vulnerabilidade Social', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'VULNERABILIDADE_SOCIAL')
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
end $$;