INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 0, 'Inscrito no CAD Único?', 4, now(), 'SISTEMA', '0', false, 6, null, 'Selecione', 'INSCRITO_CAD_UNICO'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'INSCRITO_CAD_UNICO'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 1, 'Nível socioeconômico (INSE)', 4, now(), 'SISTEMA', '0', false, 6, null, 'Selecione', 'NIVEL_SOCIOECONOMICO'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'NIVEL_SOCIOECONOMICO'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 2, 'CRAS de referência', 1, now(), 'SISTEMA', '0', false, 4, null, 'Digite o CRAS de referência...', 'CRAS'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'CRAS'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 3, 'Número do Cartão SUS (CNS)', 13, now(), 'SISTEMA', '0', false, 4, 14, '000 0000 0000 000', 'NUMERO_CARTAO_SUS'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'NUMERO_CARTAO_SUS'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 4, 'Número de Identificação Social (NIS)', 13, now(), 'SISTEMA', '0', false, 4, 12, '000.00000.00-00', 'NUMERO_IDENTIFICACAO_SOCIAL'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'NUMERO_IDENTIFICACAO_SOCIAL'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 5, 'TEG', 4, now(), 'SISTEMA', '0', true, 6, null, 'Sim', 'TEG'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'TEG'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 6, 'PAP', 25, now(), 'SISTEMA', '0', true, 6, null, 'Recuperação de aprendizagens', 'PAP'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'PAP'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 7, 'Projeto', 25, now(), 'SISTEMA', '0', true, 6, null, 'Futebol', 'PROJETO'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'PROJETO'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 8, 'Classe Hospitalar', 4, now(), 'SISTEMA', '0', false, 6, null, 'Sim', 'CLASSE_HOSPITALAR'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'CLASSE_HOSPITALAR'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 9, 'Equipamentos', 4, now(), 'SISTEMA', '0', false, 12, null, 'Selecione', 'EQUIPAMENTOS'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'EQUIPAMENTOS'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 10, 'Protocolo aplicável no caso', 4, now(), 'SISTEMA', '0', false, 12, null, 'Selecione', 'PROTOCOLO_APLICAVEL_NO_CASO'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO'
);

INSERT INTO questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
SELECT 8, 11, 'Observações adicionais', 2, now(), 'SISTEMA', '0', false, 12, null, 'Digite observações adicionais...', 'OBSERVACOES_ADICIONAIS'
WHERE NOT EXISTS (
    SELECT 1 FROM questao WHERE questionario_id = 8 AND nome_componente = 'OBSERVACOES_ADICIONAIS'
);
