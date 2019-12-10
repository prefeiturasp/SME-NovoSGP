DROP TABLE if exists public.evento_matricula;

CREATE TABLE public.evento_matricula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno varchar(100) NOT NULL,
	tipo int4 NOT NULL,
	data_evento timestamp NOT NULL,
	nome_escola varchar(200) NULL,
	nome_turma varchar(200) NULL,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,

	CONSTRAINT evento_matricula_un UNIQUE (codigo_aluno, tipo, data_evento)
);

CREATE INDEX evento_matricula_aluno_idx ON public.evento_matricula USING btree (codigo_aluno);
