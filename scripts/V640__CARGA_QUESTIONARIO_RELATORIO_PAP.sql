do $$
declare
	configuracaoId bigint;
	periodoRelatorioId bigint;
	periodoDB record;
	secaoId bigint;
	questionarioId bigint;
	questaoId bigint;
begin
	/********************************/
	/* CONFIGURAÇÕES */
	/********************************/
	insert into configuracao_relatorio_pap (inicio_vigencia, fim_vigencia, tipo_periodicidade, criado_em, criado_por, criado_rf)
	values ('2023-01-01', '2023-12-31', 'S', NOW(), 'SISTEMA', '0')
	RETURNING id INTO configuracaoId;
	
	insert into periodo_relatorio_pap (configuracao_relatorio_pap_id, periodo, criado_em, criado_por, criado_rf)
	values (configuracaoId, 2, NOW(), 'SISTEMA', '0')
	RETURNING id INTO periodoRelatorioId;
	
	for periodoDB in
		select pe.id
		from periodo_escolar pe 
		inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
		where ano_letivo = 2023 and modalidade = 1 and bimestre in(3, 4)
	loop
		insert into periodo_escolar_relatorio_pap(periodo_relatorio_pap_id, periodo_escolar_id, criado_em, criado_por, criado_rf)
		values (periodoRelatorioId, periodoDB.id, NOW(), 'SISTEMA', '0');
	end loop;
	
	
	/********************************/
	/* SEÇÃO Frequência */
	/********************************/
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf, tipo)
	values ('Questionário 1', false, NOW(), 'SISTEMA', '0', 6)
	RETURNING id INTO questionarioId;
	
	insert into secao_relatorio_periodico_pap (questionario_id, nome_componente, nome, ordem, etapa, excluido, criado_em, criado_por, criado_rf)
	values (questionarioId, 'SECAO_FREQUENCIA', 'Frequência na turma de PAP', 1, 1, false, NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
	values (secaoId, configuracaoId, NOW(), 'SISTEMA', '0');

	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
	values(questionarioId, 1, '', '', false, 21, '', NOW(), 'SISTEMA', '0', 12, 'INFO_FREQ_TURMA_PAP');

	/********************************/
	/* SEÇÃO Dificuldades apresentadas */
	/********************************/
	
	insert into secao_relatorio_periodico_pap (questionario_id, nome_componente, nome, ordem, etapa, excluido, criado_em, criado_por, criado_rf)
	values (questionarioId, 'SECAO_DIFIC_APRES', 'Dificuldades apresentadas', 2, 1, false, NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
	values (secaoId, configuracaoId, NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
	values(questionarioId, 1, 'Dificuldades apresentadas', '', true, 9, '', NOW(), 'SISTEMA', '0', 6, 'DIFIC_APRESENTADAS')
	RETURNING id INTO questaoId;
	
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values(questaoId, 1, 'Leitura', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values(questaoId, 1, 'Escrita', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values(questaoId, 1, 'Cálculos', NOW(), 'SISTEMA', '0');
	insert into opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
	values(questaoId, 1, 'Interpretação de texto', NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
	values(questionarioId, 2, 'Observações', '', false, 18, '', NOW(), 'SISTEMA', '0', 6, 'OBS_DIFIC_APRESENTADAS');

	/********************************/
	/* SEÇÃO Avanços na aprendizagem */
	/********************************/
	insert into secao_relatorio_periodico_pap (questionario_id, nome_componente, nome, ordem, etapa, excluido, criado_em, criado_por, criado_rf)
	values (questionarioId, 'SECAO_AVANC_APREND_BIMES', 'Avanços na aprendizagem durante o bimestre', 3, 1, false, NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
	values (secaoId, configuracaoId, NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
	values(questionarioId, 1, 'Avanços na aprendizagem durante o bimestre', '', true, 18, '', NOW(), 'SISTEMA', '0', 6, 'AVANC_APREND_BIMES');
	
	/********************************/
	/* SEÇÃO Observações */
	/********************************/
	Insert into secao_relatorio_periodico_pap (questionario_id, nome_componente, nome, ordem, etapa, excluido, criado_em, criado_por, criado_rf)
	values (questionarioId, 'SECAO_OBS', 'Observações', 4, 1, false, NOW(), 'SISTEMA', '0')
	RETURNING id INTO secaoId;
	
	insert into secao_config_relatorio_periodico_pap(secao_relatorio_periodico_pap_id, configuracao_relatorio_pap_id, criado_em, criado_por, criado_rf)
	values (secaoId, configuracaoId, NOW(), 'SISTEMA', '0');
	
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
	values(questionarioId, 1, 'Observações', '', false, 18, '', NOW(), 'SISTEMA', '0', 6, 'OBS_OBS');
end $$;