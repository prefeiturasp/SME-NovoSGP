DO $$
BEGIN
IF (select count(*) from parametros_sistema where tipo =104) = 0 THEN
	INSERT INTO public.parametros_sistema
	(nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
	VALUES('GerarConsolidadoEncaminhamentoNAAPA', 104, 'Controle de geração do consolidado Encaminhamento NAAPA', '', null, true, now(), 'SISTEMA', NULL, NULL, '1', NULL);
ELSE
RAISE NOTICE 'Registro GerarConsolidadoEncaminhamentoNAAPA já existe';
END IF;

IF (select count(*) from parametros_sistema where tipo =105) = 0 THEN
    INSERT INTO public.parametros_sistema
	(nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
	VALUES('GerarConsolidadoAtendimentoNAAPA', 105, 'Controle de geração do consolidado Atendimento Encaminhamento NAAPA', '', null, true, now(), 'SISTEMA', NULL, NULL, '1', NULL);
ELSE
RAISE NOTICE 'Registro GerarConsolidadoAtendimentoNAAPA já existe';
END IF;
END $$;