INSERT INTO opcao_resposta (questao_id, ordem, nome, criado_em, criado_por, criado_rf)
VALUES
-- Inscrito no Cad Único
((SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido), 1, 'Sim',  now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'INSCRITO_CAD_UNICO' AND NOT excluido), 2, 'Não', now(), 'SISTEMA', '0'),
-- Nível socioeconômico                           
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 1, 'Nível 1', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 2, 'Nível 2', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 3, 'Nível 3', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 4, 'Nível 4', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 5, 'Nível 5', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 6, 'Nível 6', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 7, 'Nível 7', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'NIVEL_SOCIOECONOMICO' AND NOT excluido), 8, 'Nível 8', now(), 'SISTEMA', '0'),

--Classe hospitalar
((SELECT id FROM questao WHERE nome_componente = 'CLASSE_HOSPITALAR' AND NOT excluido), 1, 'Sim',  now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'CLASSE_HOSPITALAR' AND NOT excluido), 2, 'Não', now(), 'SISTEMA', '0'),
--Equipamentos
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 1, 'CAPS IJ',  now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 2, 'CCA', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 3, 'CSCM', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 4, 'Conselho Tutelar', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 5, 'CRAS', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 6, 'DDM', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 7, 'NASF', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 8, 'NMSE', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 9, 'ONG', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 10, 'SAICA', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 11, 'SASF', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 12, 'SPVV', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 13, 'UBS', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 14, 'Vara de Infância e Juventude', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'EQUIPAMENTOS' AND NOT excluido), 15, 'Outra articulação', now(), 'SISTEMA', '0'),
--Protocolo aplicável no caso
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 1, 'Fluxo integrado de atenção à criança e ao adolescente vítima de violência', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 2, 'Fluxo integrado de atenção à gravidez na adolescência', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 3, 'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pelo NAAPA', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 4, 'Notificação IN SME 20/2020: Suspeita ou relato de violência contra bebê, criança ou adolescente - pela escola', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 5, 'Fluxo integrado de busca ativa escolar', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 6, 'Protocolo de ameaça', now(), 'SISTEMA', '0'),
((SELECT id FROM questao WHERE nome_componente = 'PROTOCOLO_APLICAVEL_NO_CASO' AND NOT excluido), 7, 'Protocolo Alerta SP', now(), 'SISTEMA', '0')