DROP TABLE if exists public.arquivo;

create table public.arquivo (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar not null,
	codigo uuid NOT NULL,
	tipo int not null,
	tipo_conteudo varchar not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,

	CONSTRAINT arquivo_pk PRIMARY KEY (id)
);
