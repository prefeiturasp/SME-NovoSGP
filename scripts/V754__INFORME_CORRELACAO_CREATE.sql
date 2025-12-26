CREATE TABLE if not exists public.informativo_correlacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo uuid NOT NULL,
	informe_id int8 NOT NULL, 
	usuario_solicitante_id int8 NOT NULL, 
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT informativo_correlacao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists codigo_correlacao_idx ON public.informativo_correlacao (codigo);

select
	f_cria_fk_se_nao_existir(
		'informativo_correlacao',
		'informativo_correlacao_informe_fk', 
		'FOREIGN KEY (informe_id) REFERENCES informativo (id)' 
	);

select
	f_cria_fk_se_nao_existir(
		'informativo_correlacao',
		'informativo_correlacao_usuario_fk',
		'FOREIGN KEY (usuario_solicitante_id) REFERENCES usuario (id)'
	);