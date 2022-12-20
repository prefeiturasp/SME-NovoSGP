do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	secaoId bigint;
	opcaoresposta_outro_procedimento bigint;
	questaoComplementar_descricaoProcedimentoTrabalhoId bigint;
	opcaoresposta_outros_rede_dentro_educacao bigint;
	questaoComplementar_descricaoRedeDentroEducacaoId bigint;
	opcaoresposta_outros_fora_rede_educacao bigint;
	questaoComplementar_descricaoForaRedeEducacaoId bigint;
begin
	/********************************/
	/* Questionário Seção 3 Etapa 1 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
		values ('Questionário Encaminhamento NAAPA Etapa 1 Seção 3 - Itinerância', false, NOW(), 'SISTEMA', '0', 5)
		RETURNING id INTO questionarioId;
		
	insert into secao_encaminhamento_naapa (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf, nome_componente)
		values(questionarioId, 'Itinerância', 1, 3, NOW(), 'SISTEMA', '0', 'QUESTOES_ITINERACIA');	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 0, 'Data do atendimento', '', true, 14, '{"desabilitarDataFutura":true}', NOW(), 'SISTEMA', '0', 6, 'DATA_DO_ATENDIMENTO', 'Data do atendimento');	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 1, 'Tipo de atendimento', '', true, 4, '', NOW(), 'SISTEMA', '0', 6, 'TIPO_DO_ATENDIMENTO', 'Tipo de atendimento')
		RETURNING id INTO questaoId;

	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Atendimento não presencial', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Grupo de Trabalho NAAPA', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Grupo Focal', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Itinerância', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Projeto Tecer', NOW(), 'SISTEMA', '0');			
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Reunião de Rede Macro (formada pelo território)', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Reunião de Rede Micro (formada pelo NAAPA)', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Reunião de Rede Micro na UE', NOW(), 'SISTEMA', '0');		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Reunião em Horários Coletivos', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Reunião Compartilhada', NOW(), 'SISTEMA', '0');		

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 2, 'Procedimento de trabalho', '', true, 4, '', NOW(), 'SISTEMA', '0', 8, 'PROCEDIMENTO_DE_TRABALHO', 'Procedimento de trabalho')
		RETURNING id INTO questaoId;	
		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Ações Lúdicas', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Análise Documental', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Entrevista', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'Grupo Reflexivo Interventivo', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'Observação', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Visita Técnica', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'Atendimento Remoto', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Outro procedimento', NOW(), 'SISTEMA', '0')
	RETURNING id INTO opcaoresposta_outro_procedimento;
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Projeto Tecer', NOW(), 'SISTEMA', '0');
		
	--Questoes complementares descrição do trabalho
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 1, 'Descrição do procedimento de trabalho', '', true, 2, '', NOW(), 'SISTEMA', '0', 12, 'DESCRICAO_PROCEDIMENTO_TRABALHO', 'Descrição do procedimento de trabalho')
		RETURNING id INTO questaoComplementar_descricaoProcedimentoTrabalhoId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_outro_procedimento, questaoComplementar_descricaoProcedimentoTrabalhoId, NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 3, 'Rede de proteção articulada (Dentro da rede de Educação)', '', false, 9, '', NOW(), 'SISTEMA', '0', 8, 'DENTRO_REDE_PROTECAO_EDUCACAO', 'Selecione')
		RETURNING id INTO questaoId;	
		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'Alimentação Escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'CEFAI', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'Comissão de Mediação de Conflitos', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'DEMANDA', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'DIAF', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'DICEU', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'DIPED', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'Gabinete', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'Jurídico', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Supervisão escolar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'Outros', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoresposta_outros_rede_dentro_educacao;
	
	--Questoes complementares outros rede educação
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 1, 'Descrição', '', true, 2, '', NOW(), 'SISTEMA', '0', 12, 'DESCRICAO_OUTROS_DENTRO_REDE_EDUCACAO', 'Descrição')
		RETURNING id INTO questaoComplementar_descricaoRedeDentroEducacaoId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_outros_rede_dentro_educacao, questaoComplementar_descricaoRedeDentroEducacaoId, NOW(), 'SISTEMA', '0');	
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 4, 'Rede de proteção articulada (Fora da rede de educação)', '', false, 9, '', NOW(), 'SISTEMA', '0', 8, 'FORA_REDE_PROTECAO_EDUCACAO', 'Selecione')
		RETURNING id INTO questaoId;	
		
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 1, 'AMA', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 2, 'Atendimentos particulares/Convênios', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 3, 'CAPSIJ', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 4, 'CCA', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 5, 'CDCM', NOW(), 'SISTEMA', '0');	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 6, 'Conselho Tutelar', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 7, 'CRAS', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 8, 'CREAS', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 9, 'DDM', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 10, 'Defensoria Pública', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 11, 'Ministério Público', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 12, 'ONG', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 13, 'SAICA', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 14, 'SASF', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 15, 'SPV V', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 16, 'UBS', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 17, 'Vara de Infância e Juventude', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 18, 'Hospital geral', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 19, 'NPV', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 20, 'CAPS', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 21, 'CRAI', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, 22, 'Outros', NOW(), 'SISTEMA', '0')
		RETURNING id INTO opcaoresposta_outros_fora_rede_educacao;
		
	--Questoes complementares outros fora rede educação
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente, placeholder)
		values(questionarioId, 1, 'Descrição', '', true, 2, '', NOW(), 'SISTEMA', '0', 12, 'DESCRICAO_OUTROS_FORA_REDE_EDUCACAO', 'Descrição')
		RETURNING id INTO questaoComplementar_descricaoForaRedeEducacaoId;
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
		values(opcaoresposta_outros_fora_rede_educacao, questaoComplementar_descricaoForaRedeEducacaoId, NOW(), 'SISTEMA', '0');
		
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
		values(questionarioId, 5, 'Descrição do atendimento', '', true, 18, '', NOW(), 'SISTEMA', '0', 12, 'DESCRICAO_DO_ATENDIMENTO');
end $$;