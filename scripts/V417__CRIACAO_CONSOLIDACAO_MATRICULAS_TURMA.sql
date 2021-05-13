DROP TABLE if exists public.consolidacao_matricula_turma;

CREATE TABLE public.consolidacao_matricula_turma (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	turma_id int8 NOT NULL,
	quantidade int8 NOT null default 0,
	
    CONSTRAINT consolidacao_matricula_turma_pk PRIMARY KEY (id)
);
