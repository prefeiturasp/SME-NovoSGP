DROP TABLE if exists public.carta_intencoes_observacao_notificacao;

CREATE TABLE public.carta_intencoes_observacao_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	notificacao_id int8 not null,
	observacao_id int8 not null, 
    CONSTRAINT carta_intencoes_observacao_notificacao_pk PRIMARY KEY (id)
);

select
	f_cria_fk_se_nao_existir(
		'carta_intencoes_observacao_notificacao',
		'carta_intencoes_observacao_notificacao_notificacao_fk',
		'FOREIGN KEY (notificacao_id) REFERENCES notificacao (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'carta_intencoes_observacao_notificacao',
		'carta_intencoes_observacao_notificacao_carta_intencoes_observacao_fk',
		'FOREIGN KEY (observacao_id) REFERENCES carta_intencoes_observacao (id)'
	);

CREATE INDEX carta_intencoes_observacao_notificacao_notificacao_idx ON public.carta_intencoes_observacao_notificacao USING btree (notificacao_id);
CREATE INDEX carta_intencoes_observacao_notificacao_carta_intencoes_observacao_idx ON public.carta_intencoes_observacao_notificacao USING btree (observacao_id);
