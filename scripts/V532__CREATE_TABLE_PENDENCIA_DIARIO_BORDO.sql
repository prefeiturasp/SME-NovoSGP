DROP TABLE IF EXISTS public.pendencia_diario_bordo;
CREATE TABLE public.pendencia_diario_bordo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	aula_id int8 NOT NULL,
	pendencia_id int8 NOT NULL,
    professor_rf varchar(15) NOT NULL,
    componente_curricular_id int8 NOT NULL,
    criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_diario_bordo_pk PRIMARY KEY (id)
);
CREATE INDEX pendencia_diario_bordo_aula_idx ON public.pendencia_diario_bordo USING btree (aula_id);
CREATE INDEX pendencia_diario_bordo_pendencia_idx ON public.pendencia_diario_bordo USING btree (pendencia_id);
CREATE INDEX pendencia_diario_bordo_componente_idx ON public.pendencia_diario_bordo USING btree (componente_curricular_id);
CREATE INDEX pendencia_diario_bordo_professor_idx ON public.pendencia_diario_bordo USING btree (professor_rf);

ALTER TABLE public.pendencia_diario_bordo ADD CONSTRAINT pendencia_diario_bordo_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES public.pendencia(id);
ALTER TABLE public.pendencia_diario_bordo ADD constraint pendencia_diario_bordo_aula_fk FOREIGN KEY (aula_id) REFERENCES public.aula(id);
ALTER TABLE public.pendencia_diario_bordo ADD constraint pendencia_diario_bordo_componente_fk FOREIGN KEY (componente_curricular_id) REFERENCES public.componente_curricular(id);