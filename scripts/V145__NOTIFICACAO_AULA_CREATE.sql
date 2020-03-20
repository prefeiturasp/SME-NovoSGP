DROP TABLE if exists public.notificacao_aula;

CREATE TABLE public.notificacao_aula (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	notificacao_id int8 NOT NULL,
	aula_id int8 NOT NULL,
	CONSTRAINT notificacao_aula_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_aula_notificacao_idx ON public.notificacao_aula USING btree (notificacao_id);
CREATE INDEX notificacao_aula_aula_idx ON public.notificacao_aula USING btree (aula_id);

ALTER TABLE public.notificacao_aula ADD CONSTRAINT notificacao_aula_aula_fk FOREIGN KEY (aula_id) REFERENCES aula(id);
ALTER TABLE public.notificacao_aula ADD CONSTRAINT notificacao_aula_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);
