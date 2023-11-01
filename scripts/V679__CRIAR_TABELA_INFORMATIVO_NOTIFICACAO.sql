CREATE table IF NOT EXISTS public.informativo_notificacao(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	informativo_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	CONSTRAINT informativo_notificacao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists informativo_notificacao_informativo_id_idx ON public.informativo_notificacao USING btree (informativo_id);
ALTER TABLE public.informativo_notificacao DROP CONSTRAINT if exists informativo_notificacao_informativo_id_fk;
ALTER TABLE public.informativo_notificacao ADD CONSTRAINT informativo_notificacao_informativo_id_fk FOREIGN KEY (informativo_id) REFERENCES informativo(id);

CREATE INDEX if not exists informativo_notificacao_notificacao_id_idx ON public.informativo_notificacao USING btree (notificacao_id);
ALTER TABLE public.informativo_notificacao DROP CONSTRAINT if exists informativo_notificacao_notificacao_id_fk;
ALTER TABLE public.informativo_notificacao ADD CONSTRAINT informativo_notificacao_notificacao_id_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);

