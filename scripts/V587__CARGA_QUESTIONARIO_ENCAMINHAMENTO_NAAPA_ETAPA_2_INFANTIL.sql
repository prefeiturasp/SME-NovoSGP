do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	secaoId bigint;
	questaoComplementar_adoececomfrequenciaId bigint;
	questaoComplementar_doencacronicaId bigint;
	opcaoresposta_adoececomfrequencia bigint;
	opcaoresposta_doencacronica bigint;
begin
	/********************************/
	/* Questionário Seção 1 Etapa 1 */
	/********************************/
	
	--tipo questionário 5 - Encaminhamento NAAPA
	update questionario set tipo = 5 where nome = 'Questionário Encaminhamento NAAPA Etapa 1 Seção 1' and tipo = 1;
	
	/********************************/
	/* Questionário Seção 2 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Encaminhamento NAAPA Etapa 1 Seção 2 - Infantil', false, NOW(), 'SISTEMA', '0', 5)
		RETURNING id INTO questionarioId;
	
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, nome_componente, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Questões apresentadas', 1, 2, 'QUESTOES_APRESENTADAS_INFANTIL',NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_encaminhamento_naapa_modalidade (secao_encaminhamento_id, modalidade_codigo, criado_em, criado_por, criado_rf)
		values(secaoId, 1 ,NOW(), 'SISTEMA', '0');

	--Questões no agrupamento desenvolvimento
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 0, 'Questões no agrupamento desenvolvimento', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_DESENVOLVIMENTO')
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

	--Observações Questões no agrupamento desenvolvimento
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 1, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_AGRUPAMENTO_DESENVOLVIMENTO');		

	--Questões no agrupamento proteção
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 2, 'Questões no agrupamento proteção', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_PROTECAO')
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


	--Observações Questões no agrupamento proteção
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 3, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_AGRUPAMENTO_PROTECAO');		


	--Questões no agrupamento promoção de cuidados
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 4, 'Questões no agrupamento promoção de cuidados', '', false, 9, '', NOW(), 'SISTEMA', '0', 6, 'AGRUPAMENTO_PROMOCAO_CUIDADOS')
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

	--Observações Questões no agrupamento promoção de cuidados
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 5, 'Observações', '', false, 2, '', NOW(), 'SISTEMA', '0', 'OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS');		

end $$;