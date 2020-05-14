DROP TABLE if exists public.processo_executando;

CREATE TABLE public.processo_executando (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	tipo_processo int4 NOT NULL,
	turma_id varchar(15) NULL,
	disciplina_id varchar(15) NULL,
	CONSTRAINT processo_executando_pk PRIMARY KEY (id)
);
CREATE INDEX processo_executando_turma_idx ON public.processo_executando USING btree (turma_id);
CREATE INDEX processo_executando_disciplina_idx ON public.processo_executando USING btree (disciplina_id);
