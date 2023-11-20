do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	opcaoresposta_sim_conseguiu_contato bigint;
	opcaoresposta_outros_justificativa_falta bigint;
	opcaoresposta_visita_procedimento_realizado bigint;
begin
	/********************************/
	/* Questionário Seção 1 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Registro Ação Busca Ativa Etapa 1 Seção 1', false, NOW(), 'SISTEMA', '0', 8)
		RETURNING id INTO questionarioId;
	insert into secao_registro_acao_busca_ativa (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 'Registro Ação Busca Ativa Seção 1', 1, 1, NOW(), 'SISTEMA', '0', 'SECAO_1_REGISTRO_ACAO');

	--Data
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 1, 'Data', '', true, 14, '', NOW(), 'SISTEMA', '0', 3, 'Data', 'DATA_REGISTRO_ACAO');
		
	--Conseguiu contato com o responsável?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 2, 'Conseguiu contato com o responsável?', '', true, 5, '', NOW(), 'SISTEMA', '0', 6, '', 'CONSEGUIU_CONTATO_RESP')
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoresposta_sim_conseguiu_contato;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');
	
	--Procedimento realizado	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 3, 'Procedimento realizado:', '', true, 5, '', NOW(), 'SISTEMA', '0', 6, '', 'PROCEDIMENTO_REALIZADO')
		RETURNING id INTO questaoId;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Ligação telefonica', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Visita Domiciliar', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoresposta_visita_procedimento_realizado;	
		
	--O contato ocorreu com o responsável pela criança?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 1, 'O contato ocorreu com o responsável pela criança?', '', true, 5, '', NOW(), 'SISTEMA', '0', 6, '', 'CONTATO_COM_RESPONSAVEL')
		RETURNING id INTO questaoId;
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_sim_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');	
		
	--Após a ligação/visita a criança retornou para escola?	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 2, 'Após a ligação/visita a criança retornou para escola?', '', true, 5, '', NOW(), 'SISTEMA', '0', 6, '', 'APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA')
		RETURNING id INTO questaoId;
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_sim_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Sim', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Não', NOW(), 'SISTEMA', '0');		
	
	--A família/responsável justificou a falta da criança por motivo de
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 3, 'A família/responsável justificou a falta da criança por motivo de:', '', true, 9, '', NOW(), 'SISTEMA', '0', 12, '', 'JUSTIFICATIVA_MOTIVO_FALTA')
		RETURNING id INTO questaoId;
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_sim_conseguiu_contato, questaoId, NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Ausência por gripe ou resfriado (tosse, febre, dor de garganta)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 2, 'Ausência por enjôo, diarreia, vômito', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 3, 'Ausência por doenças crônicas como anemia, diabetes, câncer, problemas cardíacos ou neurológicos, convulsões ou transplantados', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 4, 'Ausência por questões de diagnóstico de transtorno mental ou em sofrimento psíquico (depressão, ansiedade)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 5, 'Ausência por deficiência que impeça ou dificulte o acesso e permanência à Unidade Educacional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 6, 'Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em regime fechado', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 7, 'Ausência do adolescente por motivo de cumprimento de medidas socioeducativas em casa', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 8, 'Ausência por estarem viajando no período', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 9, 'Ausência porque mora distante da escola e apresente dificuldades no deslocamento', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 10, 'Ausência por estarem cuidando de irmãos, pais ou avós', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 11, 'Ausência por motivo de falecimento', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 12, 'Há suspeita de ausência por estar realizando trabalho infantil', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 13, 'Ausência por motivo de gravidez da estudante', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 14, 'Ausência por relato do estudante que não deseja voltar para a escola', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 15, 'Ausência por não ter material escolar/uniforme', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 16, 'Ausência por falta de transporte escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 17, 'Ausência por negligência da família sobre a frequência escolar (não sabe/não se preocupa/não se importa)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 18, 'Ausência por estar em situação de rua ou na rua', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 19, 'Ausência por enfrentar dificuldades financeiras', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 20, 'Ausência por não ter moradia fixa', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 21, 'Ausência por ter sido vitima de preconceito, discriminação ou bullyng na unidade educacional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 22, 'Ausência pelo estudante estar em luto', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 23, 'Ausência por não haver um responsável para levar para a escola', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 24, 'Ausência por ter perdido a vaga', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 25, 'Ausência devido aos seus responsáveis serem pessoas com deficiência e/ou apresentarem problemas de saúde mental ou dependência química (alcoolismo, drogas, medicamentos)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 26, 'Ausência porque os responsáveis não querem levar o bebê/criança/adolescente para a unidade educacional', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 27, 'Ausência por envolvimento do estudante com ácool, drogas ou medicamentos', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 28, 'Ausência devido a violência do território (comunidade, bairro)', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
			values(questaoId, 29, 'Outros', NOW(), 'SISTEMA', '0')
			RETURNING id INTO opcaoresposta_outros_justificativa_falta;

	--Descreva a justificativa da família
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 1, 'Descreva a justificativa da família:', '', true, 2, '', NOW(), 'SISTEMA', '0', 12, '', 'JUSTIFICATIVA_MOTIVO_FALTA_OUTROS')
		RETURNING id INTO questaoId;
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_outros_justificativa_falta, questaoId, NOW(), 'SISTEMA', '0');
		
	--Questões observadas durante a realização das visitas
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 1, 'Questões observadas durante a realização das visitas:', '', true, 9, '', NOW(), 'SISTEMA', '0', 12, '', 'QUESTOES_OBS_DURANTE_VISITA')
		RETURNING id INTO questaoId;
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_visita_procedimento_realizado, questaoId, NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Há suspeita de negligência', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Há suspeita de violência física', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Há suspeita/relato de violência sexual', NOW(), 'SISTEMA', '0');
		
	--Observação
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, placeholder, nome_componente)
		values(questionarioId, 4, 'Observação:', '', false, 2, '', NOW(), 'SISTEMA', '0', 12, '', 'OBS_GERAL');
		
end $$;