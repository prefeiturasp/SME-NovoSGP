do $$
declare 
	questionarioId bigint;
	opcaoRespostaId bigint;
	questaoComplementarId bigint;
	
begin
	select questionario_id 
		into questionarioId
	from secao_encaminhamento_aee sea where etapa = 1 and ordem = 2;

	-- 5.2 - Detalhe as atividades
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Detalhe as atividades', '', true, 2, '', NOW(), 'SISTEMA', '0')
		returning id into questaoComplementarId;

	select id 
		into opcaoRespostaId
	from opcao_resposta opc where opc.ordem = 4 and opc.questao_id = (select id from questao where ordem = 5 and tipo = 9);
	
	insert into opcao_questao_complementar (opcao_resposta_id, questao_complementar_id, criado_em, criado_por, criado_rf)
	values(opcaoRespostaId, questaoComplementarId, NOW(), 'SISTEMA', '0');

end $$;
