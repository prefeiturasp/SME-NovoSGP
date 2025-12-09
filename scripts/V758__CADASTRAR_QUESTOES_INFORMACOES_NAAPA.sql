insert into questao (questionario_id, ordem, nome, tipo, criado_em, criado_por, criado_rf, somente_leitura, dimensao, tamanho, placeholder, nome_componente)
values
(8, 0, 'Inscrito no CAD Único?', 4, now(), 'SISTEMA', '0', false, 6, null, 'Selecione', 'INSCRITO_CAD_UNICO'),
(8, 1, 'Nível socioeconômico (INSE)', 4, now(), 'SISTEMA', '0', false, 6, null, 'Selecione', 'NIVEL_SOCIOECONOMICO'),
(8, 2, 'CRAS de referência', 1, now(), 'SISTEMA', '0', false, 4, null, 'Digite o CRAS de referência...', 'CRAS'),
(8, 3, 'Número do Cartão SUS (CNS)', 13, now(), 'SISTEMA', '0', false, 4, 14, '000 0000 0000 000', 'NUMERO_CARTAO_SUS'),
(8, 4, 'Número de Identificação Social (NIS)', 13, now(), 'SISTEMA', '0', false, 4, 12, '000.00000.00-00', 'NUMERO_IDENTIFICACAO_SOCIAL'),
(8, 5, 'TEG', 4, now(), 'SISTEMA', '0', true, 6, null, 'Sim', 'TEG'),
(8, 6, 'PAP', 25, now(), 'SISTEMA', '0', true, 6, null, 'Recuperação de aprendizagens', 'PAP'),
(8, 7, 'Projeto', 25, now(), 'SISTEMA', '0', true, 6, null, 'Futebol', 'PROJETO'),
(8, 8, 'Classe Hospitalar', 4, now(), 'SISTEMA', '0', false, 6, null, 'Sim', 'CLASSE_HOSPITALAR'),
(8, 9, 'Equipamentos', 4, now(), 'SISTEMA', '0', false, 12, null, 'Selecione', 'EQUIPAMENTOS'),
(8, 10, 'Protocolo aplicável no caso', 4, now(), 'SISTEMA', '0', false, 12, null, 'Selecione', 'PROTOCOLO_APLICAVEL_NO_CASO'),
(8, 11, 'Observações adicionais', 2, now(), 'SISTEMA', '0', false, 12, null, 'Digite observações adicionais...', 'OBSERVACOES_ADICIONAIS')