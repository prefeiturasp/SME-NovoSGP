DELETE FROM tipo_ciclo_ano WHERE etapa_id = 3;
DELETE FROM tipo_ciclo WHERE id IN (5,6,7);

INSERT INTO tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) VALUES (5,'EJA - Alfabetização',now(),'Carga inicial','Carga inicial');
INSERT INTO tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) VALUES (6,'EJA - Básica',now(),'Carga inicial','Carga inicial');
INSERT INTO tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) VALUES (7,'EJA - Complementar',now(),'Carga inicial','Carga inicial');
INSERT INTO tipo_ciclo (id,descricao,criado_em,criado_por, criado_rf) VALUES (8,'EJA - Final',now(),'Carga inicial','Carga inicial');

INSERT INTO tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) VALUES (5,3,1);
INSERT INTO tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) VALUES (5,3,2);
INSERT INTO tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) VALUES (5,3,3);
INSERT INTO tipo_ciclo_ano (tipo_ciclo_id,etapa_id,ano) VALUES (5,3,4);