DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM parametros_sistema WHERE tipo = 106 AND ano = 2023
    ) THEN
        INSERT INTO parametros_sistema
            (nome, tipo, descricao, valor, ano, ativo, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf)
        VALUES
            ('DiasAposInicioPeriodoLetivoComponenteSemAula', 106, 'Dias Após Início Período Letivo Pendência para Turma/Componente Sem Aula', '15', 2023, true, now(), 'SISTEMA', null, null, '0', null);
    END IF;
END $$;