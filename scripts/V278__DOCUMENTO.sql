CREATE TABLE IF NOT EXISTS public.tipo_documento(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(60) NULL,
	CONSTRAINT tipo_documento_pk PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.classificacao_documento(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	descricao varchar(10) NULL,
	tipo_documento_id int8 NOT NULL,
	CONSTRAINT classificacao_documento_pk PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.documento(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	usuario_id int8 NOT NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	alterado_em timestamp NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	criado_em timestamp NOT null,
	arquivo_id int8 NULL,
	ue_id int8 NOT NULL,
	ano_letivo int8 NULL,
	classificacao_documento_id int8 NOT NULL,
	CONSTRAINT documento_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'classificacao_documento',
		'classificacao_documento_tipo_documento_fk',
		'FOREIGN KEY (tipo_documento_id) REFERENCES tipo_documento (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'documento',
		'documento_classificacao_documento_fk',
		'FOREIGN KEY (classificacao_documento_id) REFERENCES classificacao_documento (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'documento',
		'documento_arquivo_fk',
		'FOREIGN KEY (arquivo_id) REFERENCES arquivo (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'documento',
		'documento_usuario_fk',
		'FOREIGN KEY (usuario_id) REFERENCES usuario (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'documento',
		'documento_ue_fk',
		'FOREIGN KEY (ue_id) REFERENCES ue (id)'
	);

CREATE INDEX classificacao_documento_tipo_documento_idx ON public.classificacao_documento USING btree (tipo_documento_id);
CREATE INDEX documento_classificacao_documento_idx ON public.documento USING btree (classificacao_documento_id);
CREATE INDEX documento_arquivo_idx ON public.documento USING btree (arquivo_id);
CREATE INDEX documento_usuario_idx ON public.documento USING btree (usuario_id);
CREATE INDEX documento_ue_idx ON public.documento USING btree (ue_id);

insert into public.tipo_documento (descricao) values ('Plano de Trabalho');
insert into public.tipo_documento (descricao) values ('Documentos');

insert into public.classificacao_documento (descricao, tipo_documento_id) values ('PAEE', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('PAP', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('POA', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('POED', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('POEI', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('POSL', 1);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('PEA', 2);
insert into public.classificacao_documento (descricao, tipo_documento_id) values ('PPP', 2);