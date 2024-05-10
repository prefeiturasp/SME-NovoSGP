drop TABLE if exists evento;

ALTER TABLE if exists public.feriado_calendario drop constraint if exists feriado_calendario_un;
ALTER TABLE if exists public.tipo_calendario drop constraint if exists tipo_calendario_un;

ALTER TABLE if exists public.feriado_calendario ADD CONSTRAINT feriado_calendario_un UNIQUE (id);
ALTER TABLE if exists public.tipo_calendario ADD CONSTRAINT tipo_calendario_un UNIQUE (id);

CREATE TABLE if not exists evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	nome varchar(50) not null,
	descricao varchar(500) NOT NULL,
	data_inicio date not null,
	data_fim date null,
	letivo int4 NOT NULL,
	feriado_id bigint null,
	tipo_calendario_id bigint not null,
	tipo_evento_id bigint not null,
	dre_id varchar(15) null,
	ue_id varchar(15) null,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT evento_pk PRIMARY KEY (id),
	CONSTRAINT feriado_fk foreign key (feriado_id) references public.feriado_calendario (id),
	CONSTRAINT tipo_calendario_fk foreign key (tipo_calendario_id) references public.tipo_calendario (id)
);