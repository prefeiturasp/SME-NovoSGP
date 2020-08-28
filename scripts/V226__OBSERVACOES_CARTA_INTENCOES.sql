CREATE table if not exists public.carta_intencoes_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	observacao varchar NOT NULL,
	carta_intencoes_id int8 NULL,
	usuario_id int8 NULL,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(10) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(10) null,
	CONSTRAINT carta_intencoes_observacao_pk PRIMARY KEY (id)
);


select
	f_cria_fk_se_nao_existir(
		'carta_intencoes_observacao',
		'carta_intencoes_observacao_carta_intencoes_fk',
		'FOREIGN KEY (carta_intencoes_id) REFERENCES carta_intencoes (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'carta_intencoes_observacao',
		'carta_intencoes_observacao_usuario_fk',
		'FOREIGN KEY (usuario_id) REFERENCES usuario (id)'
	);

CREATE INDEX IF NOT EXISTS carta_intencoes_observacoes_idx ON public.carta_intencoes_observacao USING btree (carta_intencoes_id);