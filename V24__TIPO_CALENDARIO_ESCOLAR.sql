CREATE TABLE IF NOT EXISTS public.tipo_calendario_escolar (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,	
	ano_letivo int4 not  null,
	nome varchar(50) not null,
	periodo int not null,
	modalidade int not null,
	situacao int not null default 1,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL
);