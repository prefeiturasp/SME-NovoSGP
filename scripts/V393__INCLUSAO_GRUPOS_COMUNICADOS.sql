-- CIEJA - Tipo de escola 13
INSERT INTO public.grupo_comunicado
    (nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
SELECT 'CIEJA', 13, '3,4', current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, '2'
WHERE
    NOT EXISTS (
        SELECT id FROM public.grupo_comunicado WHERE nome = 'CIEJA'
    );
	
--CMCT - Tipo escola 23
INSERT INTO public.grupo_comunicado
    (nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
SELECT 'CMCT', 23, null, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, null
WHERE
    NOT EXISTS (
        SELECT id FROM public.grupo_comunicado WHERE nome = 'CMCT'
    );


--MOVA - Tipo escola 22
INSERT INTO public.grupo_comunicado
    (nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
SELECT 'MOVA', 22, null, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, null
WHERE
    NOT EXISTS (
        SELECT id FROM public.grupo_comunicado WHERE nome = 'MOVA'
    );


-- ETEC
INSERT INTO public.grupo_comunicado
    (nome, tipo_escola_id, tipo_ciclo_id, criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido, etapa_ensino_id)
SELECT 'ETEC', null, 20, current_timestamp, 'Carga inicial', null, null, 'Carga Inicial', null, false, '14'
WHERE
    NOT EXISTS (
        SELECT id FROM public.grupo_comunicado WHERE nome = 'ETEC'
    );

-- Infantil CEI 
update public.grupo_comunicado set nome = 'Infantil CEI' where id = 2;
