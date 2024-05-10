CREATE TABLE if not exists public.relatorio_correlacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo uuid NOT NULL,
	tipo_relatorio int4 NOT NULL,
	usuario_solicitante_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT relatorio_correlacao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists codigo_correlacao_idx ON public.relatorio_correlacao (codigo);

CREATE TABLE if not exists public.relatorio_correlacao_jasper (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	jsession_id varchar(200) NOT NULL,
	request_id uuid NOT NULL,
	export_id uuid NOT NULL,
	relatorio_correlacao_id bigint not NULL,
	CONSTRAINT relatorio_correlacao_jasper_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'relatorio_correlacao_jasper',
		'relatorio_correlacao_fk',
		'FOREIGN KEY (relatorio_correlacao_id) REFERENCES relatorio_correlacao (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'relatorio_correlacao',
		'relatorio_correlacao_usuario_fk',
		'FOREIGN KEY (usuario_solicitante_id) REFERENCES usuario (id)'
	);