CREATE TABLE IF NOT EXISTS public.prioridade_perfil (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ordem int8 NOT NULL,
	nome_perfil varchar(100) NOT NULL,
	codigo_perfil varchar(10) NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT prioridade_perfil_ordem_un UNIQUE (ordem),
	CONSTRAINT prioridade_perfil_un UNIQUE (codigo_perfil)
);
