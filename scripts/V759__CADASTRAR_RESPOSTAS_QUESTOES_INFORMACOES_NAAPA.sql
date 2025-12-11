
-- ============================================
-- INSCRITO NO CAD ÚNICO
-- ============================================
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido AND questionario_id = 87),
    1, 'Sim', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 1
      AND nome = 'Sim'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido AND questionario_id = 87),
    2, 'Não', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 2
      AND nome = 'Não'
);

-- ============================================
-- NÍVEL SOCIOECONÔMICO
-- ============================================
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       1, 'Nível 1', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 1 AND nome = 'Nível 1'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       2, 'Nível 2', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 2 AND nome = 'Nível 2'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       3, 'Nível 3', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 3 AND nome = 'Nível 3'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       4, 'Nível 4', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 4 AND nome = 'Nível 4'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       5, 'Nível 5', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 5 AND nome = 'Nível 5'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       6, 'Nível 6', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 6 AND nome = 'Nível 6'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       7, 'Nível 7', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 7 AND nome = 'Nível 7'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT (SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87),
       8, 'Nível 8', now(), 'SISTEMA', '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id = (SELECT id FROM questao WHERE nome_componente='NIVEL_SOCIOECONOMICO' AND NOT excluido AND questionario_id = 87)
      AND ordem = 8 AND nome = 'Nível 8'
);

-- ============================================
-- CLASSE HOSPITALAR
-- ============================================
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente='CLASSE_HOSPITALAR' AND NOT excluido AND questionario_id = 87),
1,
'Sim', 
now(), 
'SISTEMA',
'0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id= (SELECT id FROM questao WHERE nome_componente='CLASSE_HOSPITALAR' AND NOT excluido AND questionario_id = 87)
      AND ordem = 1 
      AND nome= 'Sim'
);

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente='CLASSE_HOSPITALAR' AND NOT excluido AND questionario_id = 87),
       2,
       'Não', 
       now(), 
       'SISTEMA',
       '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE questao_id=(SELECT id FROM questao WHERE nome_componente='CLASSE_HOSPITALAR' AND NOT excluido AND questionario_id = 87)
      AND ordem=2 AND nome='Não'
);

-- ============================================
-- EQUIPAMENTOS
-- ============================================
-- EQUIPAMENTOS (1) CAPS IJ
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    1,
    'CAPS IJ',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'CAPS IJ'
      AND ordem = 1
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (2) CCA
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    2,
    'CCA',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'CCA'
      AND ordem = 2
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (3) CSCM
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    3,
    'CSCM',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'CSCM'
      AND ordem = 3
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (4) Conselho Tutelar
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    4,
    'Conselho Tutelar',
    now(),
    'SISTISTA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'Conselho Tutelar'
      AND ordem = 4
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (5) CRAS
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    5,
    'CRAS',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'CRAS'
      AND ordem = 5
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (6) DDM
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    6,
    'DDM',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'DDM'
      AND ordem = 6
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (7) NASF
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    7,
    'NASF',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'NASF'
      AND ordem = 7
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (8) NMSE
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    8,
    'NMSE',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'NMSE'
      AND ordem = 8
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (9) ONG
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    9,
    'ONG',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'ONG'
      AND ordem = 9
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (10) SAICA
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    10,
    'SAICA',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'SAICA'
      AND ordem = 10
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (11) SASF
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    11,
    'SASF',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'SASF'
      AND ordem = 11
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (12) SPVV
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    12,
    'SPVV',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'SPVV'
      AND ordem = 12
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (13) UBS
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    13,
    'UBS',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'UBS'
      AND ordem = 13
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (14) Vara de Infância e Juventude
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    14,
    'Vara de Infância e Juventude',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'Vara de Infância e Juventude'
      AND ordem = 14
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- EQUIPAMENTOS (15) Outra articulação
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
    (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87),
    15,
    'Outra articulação',
    now(),
    'SISTEMA',
    '0'
WHERE NOT EXISTS (
    SELECT 1 FROM opcao_resposta 
    WHERE nome = 'Outra articulação'
      AND ordem = 15
      AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido AND questionario_id = 87)
);

-- ============================================
-- PROTOCOLO APLICÁVEL NO CASO
-- ============================================
INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87),
1, 
'Fluxo integrado de atenção à criança e ao adolescente vítima de violência', 
now(),
'SISTEMA',
'0'
WHERE NOT EXISTS (
      SELECT 1 FROM opcao_resposta
       WHERE nome = 'Fluxo integrado de atenção à criança e ao adolescente vítima de violência'
        AND ordem = 1
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87)
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87),
2,
'Fluxo integrado de atenção à gravidez na adolescência', 
now(),
'SISTEMA',
'0'
WHERE NOT EXISTS (
      SELECT 1 FROM opcao_resposta
      WHERE nome = 'Fluxo integrado de atenção à gravidez na adolescência'
        AND ordem = 2
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87)
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87),
3, 
'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pelo NAAPA',
now(), 
'SISTEMA', 
'0'
WHERE NOT EXISTS (
   SELECT 1 FROM opcao_resposta
      WHERE nome = 'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pelo NAAPA'
        AND ordem = 3
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87)
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87), 
4, 
'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pela escola', 
now(), 
'SISTEMA',
'0'
WHERE NOT EXISTS (
   SELECT 1 FROM opcao_resposta
      WHERE nome = 'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pela escola'
        AND ordem = 4
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87) 
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87),
5,
'Fluxo integrado de busca ativa escolar', 
now(), 
'SISTEMA',
'0'
WHERE NOT EXISTS (
   SELECT 1 FROM opcao_resposta
   WHERE nome = 'Fluxo integrado de busca ativa escolar'
        AND ordem = 5
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87) 
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87), 
6, 
'Protocolo de ameaça',
now(), 
'SISTEMA', 
'0'
WHERE NOT EXISTS (
   SELECT 1 FROM opcao_resposta
    WHERE nome = 'Protocolo de ameaça'
        AND ordem = 6
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87) 
  );

INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
SELECT 
(SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87), 
7,
'Protocolo Alerta SP',
now(),
'SISTEMA',  
'0'
WHERE NOT EXISTS (
   SELECT 1 FROM opcao_resposta
   WHERE nome = 'Protocolo Alerta SP'
        AND ordem = 7
        AND questao_id = (SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido AND questionario_id = 87)
  );
