CREATE table if not exists public.consolidacao_alfabetizacao_nivel_escrita (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id varchar(15) not null,
	dre_id varchar(15) not null,
	periodo int4 not null,
	ano_letivo int4 not null,
	nivel_escrita char(3) not null,
	quantidade int NOT null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	CONSTRAINT consolidacao_alfabetizacao_nivel_escrita_pk PRIMARY KEY (id)
);