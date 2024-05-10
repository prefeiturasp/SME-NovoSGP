drop table if exists pendencia_professor;

CREATE TABLE public.pendencia_professor (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	pendencia_id int8 not null,
	componente_curricular_id int8 not null,
	turma_id int8 not null,
	professor_rf varchar(15) not null,
	CONSTRAINT pendencia_professor_pk PRIMARY KEY (id)
);

ALTER TABLE public.pendencia_professor ADD CONSTRAINT pendencia_professor_componente_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id);
ALTER TABLE public.pendencia_professor ADD CONSTRAINT pendencia_professor_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

CREATE INDEX pendencia_professor_componente_curricular_idx ON public.pendencia_professor USING btree (componente_curricular_id);
CREATE INDEX pendencia_professor_turma_idx ON public.pendencia_professor USING btree (turma_id);
CREATE INDEX pendencia_professor_professor_idx ON public.pendencia_professor USING btree (professor_rf);
