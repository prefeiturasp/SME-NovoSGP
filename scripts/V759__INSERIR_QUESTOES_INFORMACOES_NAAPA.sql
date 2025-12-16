-- Script de inserção de questões no questionário 87
-- Todas as verificações padronizadas com questionario_id e verificação de exclusão

-- Questão 0: Tabela dinâmica endereço
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
SELECT 87, 0, '', 15, '{
        "titulo": "Contexto social",
        "subtitulo": "Estas são informações familiares e de contexto social do estudante."
    }',
    NOW(), 'SISTEMA', '0', 12, 'TABELA_ENDERECO'
WHERE NOT EXISTS (
    SELECT 1
    FROM questao
    WHERE questionario_id = 87
      AND nome_componente = 'TABELA_ENDERECO'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 1: Tabela dinâmica contexto social
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, dimensao, nome_componente)
SELECT 87, 1, '', 29, NOW(), 'SISTEMA', '0', 12, 'TABELA_CONTEXTO_SOCIAL'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TABELA_CONTEXTO_SOCIAL'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 2: Inscrito no Cad Único
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 2, 'Inscrito no CAD Único?', 4, NOW(), 'SISTEMA', '0', FALSE, 6, 'Selecione', 'INSCRITO_CAD_UNICO'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'INSCRITO_CAD_UNICO'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 3: Nível socioeconômico (INSE)
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 3, 'Nível socioeconômico (INSE)', 4, NOW(), 'SISTEMA', '0', FALSE, 6, 'Selecione', 'NIVEL_SOCIOECONOMICO'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'NIVEL_SOCIOECONOMICO'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 4: CRAS de referência
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 87, 4, 'CRAS de referência', 1, NOW(), 'SISTEMA', '0', FALSE, 4, 100, 'Digite o CRAS de referência...', 'CRAS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'CRAS'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 5: Número do Cartão SUS (CNS)
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, mascara, placeholder, nome_componente)
SELECT 87, 5, 'Número do Cartão SUS (CNS)', 13, NOW(), 'SISTEMA', '0', FALSE, 4, 14, '000 0000 0000 000', '000 0000 0000 000', 'NUMERO_CARTAO_SUS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'NUMERO_CARTAO_SUS'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 6: Número de Identificação Social (NIS)
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, mascara, placeholder, nome_componente)
SELECT 87, 6, 'Número de Identificação Social (NIS)', 13, NOW(), 'SISTEMA', '0', FALSE, 4, 12, '000.00000.00-00', '000.00000.00-00', 'NUMERO_IDENTIFICACAO_SOCIAL'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'NUMERO_IDENTIFICACAO_SOCIAL'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 7: Tabela dinâmica informações pedagógicas
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
SELECT 87, 7, '', 30, 
    '{
        "titulo": "Informações pedagógicas e de acompanhamento escolar",
        "subtitulo": "Abrange tanto os dados de desempenho quanto as ações da unidade educacional e da rede em torno do estudante."
    }',
 NOW(), 'SISTEMA', '0', 12, 'TABELA_INFORMACOES_PEDAGOGICAS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TABELA_INFORMACOES_PEDAGOGICAS'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 8: Tabela frequencia detalhada
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
SELECT 87, 8, '', 30, 
     '{
        "titulo": "Frequência detalhada"
     }',
    NOW(), 'SISTEMA', '0', 12, 'TABELA_FREQUENCIA_DETALHADA'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TABELA_FREQUENCIA_DETALHADA'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 9: Tabela busca ativa escolar
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, dimensao, nome_componente)
SELECT 87, 9, '', 30, 
    '{
        "titulo": "Busca ativa escolar",
        "subtitulo": "Identificação de quantas ligações e visitas foram realizadas à residência do estudante"
     }',
    NOW(), 'SISTEMA', '0', 12, 'TABELA_BUSCA_ATIVA'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TABELA_BUSCA_ATIVA'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 10: TEG
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 10, 'TEG', 4, 
    '{
        "titulo": "Acompanhamento pedagógico",
        "subtitulo": "Identificação do acompanhamento ou atividade pedagógica realizada com o estudante."
     }',
     NOW(), 'SISTEMA', '0', TRUE, 6, 'Sim', 'TEG'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TEG'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 11: PAP
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 11, 'PAP', 25, NOW(), 'SISTEMA', '0', TRUE, 6, 'Recuperação de aprendizagens', 'PAP'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'PAP'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 12: Projeto
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 12, 'Projeto', 25, NOW(), 'SISTEMA', '0', TRUE, 6, 'Futebol', 'PROJETO'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'PROJETO'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 13: Classe Hospitalar
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 13, 'Classe Hospitalar', 4, NOW(), 'SISTEMA', '0', FALSE, 6, 'Sim', 'CLASSE_HOSPITALAR'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'CLASSE_HOSPITALAR'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 14: Acompanhamento - Equipamentos
INSERT INTO questao (questionario_id, ordem, nome, tipo, opcionais, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 14, 'Equipamentos', 4, 
     '{
        "titulo": "Acompanhamento",
        "subtitulo": "Confira quais equipamentos estão oferecendo acompanhamento ao estudante."
      }',
      NOW(), 'SISTEMA', '0', FALSE, 12, 'Selecione', 'EQUIPAMENTOS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'EQUIPAMENTOS'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 15: Protocolo aplicável no caso
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, placeholder, nome_componente)
SELECT 87, 15, 'Protocolo aplicável no caso', 4, NOW(), 'SISTEMA', '0', FALSE, 12, 'Selecione', 'PROTOCOLO_APLICAVEL_NO_CASO'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 16: Observações adicionais
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 87, 16, 'Observações adicionais', 2, NOW(), 'SISTEMA', '0', FALSE, 12, 500, 'Digite observações adicionais...', 'OBSERVACOES_ADICIONAIS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'OBSERVACOES_ADICIONAIS'
      AND (excluido IS NULL OR excluido = FALSE)
);

-- Questão 17: Tabela Avaliações Bimestrais
INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, nome_componente)
SELECT 87, 17, 'Avaliações bimestrais', 28, NOW(), 'SISTEMA', '0', 'TABELA_AVALIACOES_BIMESTRAIS'
WHERE NOT EXISTS (
    SELECT 1 
    FROM questao 
    WHERE questionario_id = 87 
      AND nome_componente = 'TABELA_AVALIACOES_BIMESTRAIS'
      AND (excluido IS NULL OR excluido = FALSE)
);
