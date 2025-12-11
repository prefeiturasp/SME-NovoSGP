-- Verifica se já existe registro com mesmo nome e tipo
INSERT INTO questionario (nome, criado_em, criado_por, criado_rf, tipo)
SELECT 
    'Questionário Atendimento NAAPA Etapa 1 Seção 1 - Aba Informações',
    now(),
    'SISTEMA',
    '0',
    5
WHERE NOT EXISTS (
    SELECT 1 
    FROM questionario 
    WHERE nome = 'Questionário Atendimento NAAPA Etapa 1 Seção 1 - Aba Informações'
    AND tipo = 5
);