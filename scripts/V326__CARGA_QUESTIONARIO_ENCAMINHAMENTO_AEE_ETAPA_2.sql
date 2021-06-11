do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	questaoComplementarId bigint;
	questaoComplementar2Id bigint;
	
begin
	/********************************/
	/* Questionário Seção 3 Etapa 2 */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 2 Seção 3', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, nome, etapa, ordem, criado_em, criado_por, criado_rf)
		values(questionarioId, 'Parecer Coordenação', 2, 1, NOW(), 'SISTEMA', '0');	

	-- 1 - Quais mediações pedagógicas você realizou junto ao professor de classe comum antes de encaminhar o estudante ao AEE?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Quais mediações pedagógicas você realizou junto ao professor de classe comum antes de encaminhar o estudante ao AEE?', '', true, 2, '', NOW(), 'SISTEMA', '0');

	-- 2 - Além do professor de classe comum, o encaminhamento do estudante ao AEE foi discutido com outros profissionais da unidade educacional?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Além do professor de classe comum, o encaminhamento do estudante ao AEE foi discutido com outros profissionais da unidade educacional?', '', true, 3, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoId;

    -- 2.1 - Quais profissionais participaram desta discussão e quais as contribuições de cada um?
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Quais profissionais participaram desta discussão e quais as contribuições de cada um?', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;	

    insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 1, 'Sim', NOW(), 'SISTEMA', '0');
    
    -- 2.1  - Justifique o motivo de não haver envolvimento de outros profissionais
    insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justifique o motivo de não haver envolvimento de outros profissionais', '', true, 2, '', NOW(), 'SISTEMA', '0')
		RETURNING id INTO questaoComplementarId;

	insert into opcao_resposta (questao_id, questao_complementar_id, ordem, nome, criado_em, criado_por, criado_rf)
		values(questaoId, questaoComplementarId, 2, 'Não', NOW(), 'SISTEMA', '0');	

    -- 3 - Observações adicionais (se necessário)
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Observações adicionais (se necessário)', '', false, 2, '', NOW(), 'SISTEMA', '0');   
    
end $$;
