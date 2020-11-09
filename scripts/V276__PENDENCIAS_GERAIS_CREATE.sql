drop table if exists public.pendencia_calendario_ue;
CREATE TABLE public.pendencia_calendario_ue (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	ue_id int8 NOT NULL,
	tipo_calendario_id int8 NOT NULL,
	
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT null,
	CONSTRAINT pendencia_calendario_ue_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'pendencia_calendario_ue',
		'pendencia_calendario_ue_ue_fk',
		'FOREIGN KEY (ue_id) REFERENCES ue (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'pendencia_calendario_ue',
		'pendencia_calendario_ue_calendario_fk',
		'FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario (id)'
	);

CREATE INDEX pendencia_calendario_ue_ue_idx ON public.pendencia_calendario_ue USING btree (ue_id);
CREATE INDEX pendencia_calendario_ue_calendario_idx ON public.pendencia_calendario_ue USING btree (tipo_calendario_id);


drop table if exists public.pendencia_parametro_evento;
CREATE TABLE public.pendencia_parametro_evento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	parametro_sistema_id int8 NOT NULL,
	
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT null,
	CONSTRAINT pendencia_parametro_evento_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'pendencia_parametro_evento',
		'pendencia_parametro_evento_parametro_fk',
		'FOREIGN KEY (parametro_sistema_id) REFERENCES parametros_sistema (id)'
	);

CREATE INDEX pendencia_parametro_evento_parametro_idx ON public.pendencia_parametro_evento USING btree (parametro_sistema_id);

drop table if exists public.pendencia_usuario;
CREATE TABLE public.pendencia_usuario (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT null,
	CONSTRAINT pendencia_usuario_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'pendencia_usuario',
		'pendencia_usuario_usuario_fk',
		'FOREIGN KEY (usuario_id) REFERENCES usuario (id)'
	);

CREATE INDEX pendencia_usuario_usuario_idx ON public.pendencia_usuario USING btree (usuario_id);
