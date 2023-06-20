DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM parametros_sistema WHERE tipo = 86 AND ano = 2022
    ) THEN
        INSERT INTO parametros_sistema
            (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
        VALUES
            ('Devolutiva', 86, 'Utilizar no Layout Para Relatorio de Devolutiva', '2022', 2022, true, now(), 'SISTEMA', null, null, '0', null);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM parametros_sistema WHERE tipo = 86 AND ano = 2023
    ) THEN
        INSERT INTO parametros_sistema
            (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
        VALUES
            ('Devolutiva', 86, 'Utilizar no Layout Para Relatorio de Devolutiva', '2023', 2023, true, now(), 'SISTEMA', null, null, '0', null);
    END IF;
END $$;