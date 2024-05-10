CREATE table if not exists public.diario_bordo_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	observacao varchar NOT NULL,
	diario_bordo_id int8 NULL,
	usuario_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(10) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(10) null,
	CONSTRAINT diario_bordo_observacao_pk PRIMARY KEY (id)
);


select
	f_cria_fk_se_nao_existir(
		'diario_bordo_observacao',
		'diario_bordo_observacao_diario_bordo_fk',
		'FOREIGN KEY (diario_bordo_id) REFERENCES diario_bordo (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'diario_bordo_observacao',
		'diario_bordo_observacao_usuario_fk',
		'FOREIGN KEY (usuario_id) REFERENCES usuario (id)'
	);

CREATE INDEX IF NOT EXISTS diario_bordo_observacoes_idx ON public.diario_bordo_observacao USING btree (diario_bordo_id);