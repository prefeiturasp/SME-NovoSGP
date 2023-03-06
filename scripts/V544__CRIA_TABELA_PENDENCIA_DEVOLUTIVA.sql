DROP TABLE IF EXISTS pendencia_devolutiva;
CREATE TABLE pendencia_devolutiva (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pendencia_id int8 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	turma_id int8 NOT NULL,
	CONSTRAINT pendencia_devolutiva_pk PRIMARY KEY (id)
); 


CREATE INDEX pendencia_devolutiva_pendencia_idx ON public.pendencia_devolutiva USING btree (pendencia_id);
CREATE INDEX pendencia_devolutiva_componente_idx ON public.pendencia_devolutiva USING btree (componente_curricular_id);
CREATE INDEX pendencia_devolutiva_turma_idx ON public.pendencia_devolutiva USING btree (turma_id);


ALTER TABLE public.pendencia_devolutiva ADD CONSTRAINT pendencia_devolutiva_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
ALTER TABLE public.pendencia_devolutiva ADD CONSTRAINT pendencia_devolutiva_componente_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id);
ALTER TABLE public.pendencia_devolutiva ADD CONSTRAINT pendencia_devolutiva_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id);