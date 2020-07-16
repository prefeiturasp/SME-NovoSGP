CREATE TABLE if not exists public.comunicado_aluno (
	id bigint NOT NULL GENERATED ALWAYS AS IDENTITY,
	aluno_codigo varchar(30) NOT NULL,
	comunicado_id bigint NOT NULL,
	criado_em timestamp NOT NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);

ALTER TABLE public.comunicado ADD if not exists ano_letivo int4 NOT NULL DEFAULT 2020;

ALTER TABLE public.comunicado ADD if not exists modalidade int4 NULL;

ALTER TABLE public.comunicado ADD if not exists codigo_dre varchar(50) NULL;

ALTER TABLE public.comunicado ADD if not exists codigo_ue varchar(50) NULL;

ALTER TABLE public.comunicado ADD if not exists turma varchar(50) NULL;

ALTER TABLE public.comunicado ADD if not exists alunos_especificados bool NOT NULL DEFAULT false;
