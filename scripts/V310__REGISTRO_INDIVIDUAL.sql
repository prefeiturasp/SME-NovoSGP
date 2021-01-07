CREATE TABLE public.registro_individual (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo int8 NULL,
	componente_curricular_id int8 NOT NULL,
	data_registro timestamp NOT NULL,
	registro varchar NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	migrado bool NOT NULL DEFAULT false,
	CONSTRAINT registro_individual_pk PRIMARY KEY (id)
);
CREATE INDEX registro_individual_turma_idx ON public.registro_individual USING btree (turma_id);
CREATE INDEX registro_individual_aluno_codigo_idx ON public.registro_individual USING btree (aluno_codigo);
CREATE INDEX registro_individual_componente_curricular_idx ON public.registro_individual USING btree (componente_curricular_id);


-- public.registro_individual foreign keys

ALTER TABLE public.registro_individual ADD CONSTRAINT registro_individual_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
ALTER TABLE public.registro_individual ADD CONSTRAINT registro_individual_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES componente_curricular(id);