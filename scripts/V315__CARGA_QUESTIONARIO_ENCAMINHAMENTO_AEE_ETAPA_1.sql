do $$
declare 
	questionarioId bigint;
	questaoId bigint;
	
begin
	-- Questionário Seção 1 Etapa 1
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 1 Seção 1', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, criado_em, criado_por, criado_rf)
		values(questionarioId, NOW(), 'SISTEMA', '0');


	-- Informações Escolares
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 0, '', '', false, 7, '', NOW(), 'SISTEMA', '0');

	-- 1 - Justificativa de Ausências
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justificativa de ausências', '', false, 2, '', NOW(), 'SISTEMA', '0');
	-- 2 - Estudante está ou esteve matriculado em classe ou escola especializada
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 2, 'Estudante está ou esteve matriculado em classe ou escola especializada', '', true, 3, '', NOW(), 'SISTEMA', '0');
	-- 3 - Qual último período/ano em que o estudante frequentou ​classe ou escola especializada
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 3, 'Qual último período/ano em que o estudante frequentou ​classe ou escola especializada', '', true, 2, '', NOW(), 'SISTEMA', '0');


	-- Questionário Seção 2 Etapa 1
	insert into questionario (nome, excluido, criado_em, criado_por, criado_rf)
		values ('Questionário Encaminhamento AEE Etapa 1 Seção 2', false, NOW(), 'SISTEMA', '0')
		RETURNING id INTO questionarioId;
	insert into secao_encaminhamento_aee (questionario_id, criado_em, criado_por, criado_rf)
		values(questionarioId, NOW(), 'SISTEMA', '0');

	-- 1 - Justificativa de Ausências
	insert into questao(questionario_id, ordem, nome, observacao, obrigatorio, tipo, opcionais, criado_em, criado_por, criado_rf)
		values(questionarioId, 1, 'Justificativa de ausências', '', false, 2, '', NOW(), 'SISTEMA', '0');


	select * from secao_encaminhamento_aee 
end $$;