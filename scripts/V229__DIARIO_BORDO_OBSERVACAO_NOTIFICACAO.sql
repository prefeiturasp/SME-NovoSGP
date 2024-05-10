
--V229__DIARIO_BORDO_OBSERVACAO_NOTIFICACAO

DROP TABLE IF EXISTS public.diario_bordo_observacao_notificacao;

CREATE TABLE public.diario_bordo_observacao_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	observacao_id int8 NOT NULL,
	notificacao_id int8 not null	
);

select
	f_cria_fk_se_nao_existir(
		'diario_bordo_observacao_notificacao',
		'diario_bordo_observacao_notificacao_notificacao_fk',
		'FOREIGN KEY (notificacao_id) REFERENCES notificacao (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'diario_bordo_observacao_notificacao',
		'diario_bordo_observacao_notificacao_diario_bordo_observacao_fk',
		'FOREIGN KEY (observacao_id) REFERENCES diario_bordo_observacao (id)'
	);

CREATE INDEX diario_bordo_observacao_notificacao_notificacao_idx ON public.diario_bordo_observacao_notificacao USING btree (notificacao_id);
CREATE INDEX diario_bordo_observacao_notificacao_diario_bordo_observacao_idx ON public.diario_bordo_observacao_notificacao USING btree (observacao_id);