-- GRUPO MATRIZ x AREA CONHECIMENTO
DROP TABLE if exists componente_curricular_grupo_area_ordenacao;
CREATE TABLE public.componente_curricular_grupo_area_ordenacao (
	grupo_matriz_id int8 NOT NULL,
	area_conhecimento_id int8 not null,
	ordem int null, 
	
    CONSTRAINT componente_curricular_grupo_area_ordenacao_pk PRIMARY KEY (grupo_matriz_id, area_conhecimento_id)
);
CREATE INDEX cc_gp_ac_ordenacao_grupo_idx ON public.componente_curricular_grupo_area_ordenacao USING btree (grupo_matriz_id);
CREATE INDEX cc_gp_ac_ordenacao_area__idx ON public.componente_curricular_grupo_area_ordenacao USING btree (area_conhecimento_id);

ALTER TABLE public.componente_curricular_grupo_area_ordenacao ADD CONSTRAINT cc_gp_ac_ordenacao_grupo_fk FOREIGN KEY (grupo_matriz_id) REFERENCES componente_curricular_grupo_matriz(id);
ALTER TABLE public.componente_curricular_grupo_area_ordenacao ADD CONSTRAINT cc_gp_ac_ordenacao_area_fk FOREIGN KEY (area_conhecimento_id) REFERENCES componente_curricular_area_conhecimento(id);

INSERT INTO public.componente_curricular_grupo_area_ordenacao
	(grupo_matriz_id, area_conhecimento_id, ordem)
VALUES
	(1, 1, 1),
	(1, 2, 2),
	(1, 4, 3),
	(1, 3, 4),
	(5, 7, 1),
	(5, 8, 2),
	(6, 3, 1),
	(6, 4, 2),
	(6, 1, 3);
