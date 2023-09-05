DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM parametros_sistema WHERE tipo = 108 AND ano = 2023
    ) THEN
        INSERT INTO parametros_sistema
            (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
        VALUES
            ('ComponentesParaObjetivosAprendizagemOpcionais', 108, 'Controle de obrigatóriedade de objetivos de aprendizagem para plano aula', '1322, 1770', 2023, true, now(), 'SISTEMA', null, null, '0', null);
    END IF;
END $$;