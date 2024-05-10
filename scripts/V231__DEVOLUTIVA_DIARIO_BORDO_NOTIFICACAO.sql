
DROP TABLE IF EXISTS public.devolutiva_diario_bordo_notificacao;

CREATE TABLE public.devolutiva_diario_bordo_notificacao (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	devolutiva_id int8 NOT NULL,
	notificacao_id int8 NOT null
);

select
	f_cria_fk_se_nao_existir(
		'devolutiva_diario_bordo_notificacao',
		'devolutiva_diario_bordo_notificacao_notificacao_fk',
		'FOREIGN KEY (notificacao_id) REFERENCES notificacao (id)'
	);

select
	f_cria_fk_se_nao_existir(
		'devolutiva_diario_bordo_notificacao',
		'devolutiva_diario_bordo_notificacao_diario_bordo_devolutiva_fk',
		'FOREIGN KEY (devolutiva_id) REFERENCES devolutiva (id)'
	);

CREATE INDEX devolutiva_diario_bordo_notificacao_notificacao_idx ON public.devolutiva_diario_bordo_notificacao USING btree (notificacao_id);
CREATE INDEX devolutiva_diario_bordo_notificacao_devolutiva_idx ON public.devolutiva_diario_bordo_notificacao USING btree (devolutiva_id);
