-- CIEJA - Tipo de escola 13
INSERT INTO public.grupo_comunicado
(nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
VALUES('CIEJA', 13, '3,4', current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, '2');

--CMCT - Tipo escola 23
INSERT INTO public.grupo_comunicado
(nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
VALUES('CMCT', 23, null, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, null);

--MOVA - Tipo escola 22
INSERT INTO public.grupo_comunicado
(nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
VALUES('MOVA', 22, null, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, null);

-- ETEC
INSERT INTO public.grupo_comunicado
(nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
VALUES('ETEC', null, null, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, '14');

-- Infantil CEI 
update public.grupo_comunicado set nome = 'Intantil CEI' where id = 2
