CREATE TABLE public.relatorio_correlacao
(
	id bigint not null
	generated always as identity,
	codigo varchar(200) NOT NULL,
	codigo_tipo_relatorio bigint NOT NULL,
	usuario_solicitante_id bigint NOT NULL,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	
	CONSTRAINT relatorio_correlacao_pk PRIMARY KEY (id)
);

CREATE TABLE public.relatorio_correlacao_jasper
(
	id bigint not null
	generated always as identity,
	jsession_id varchar(200) NOT NULL,
	request_id varchar(200) NOT NULL,
	export_id varchar(200) NOT NULL,
	relatorio_correlacao_id bigint NOT NULL,
	
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	
	CONSTRAINT relatorio_correlacao_jasper_pk PRIMARY KEY (id)
);