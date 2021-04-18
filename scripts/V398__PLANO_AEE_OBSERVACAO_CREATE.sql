drop table if exists public.notificacao_plano_aee_observacao;
drop table if exists public.plano_aee_observacao;

CREATE TABLE public.plano_aee_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_id int8 not null,
	observacao varchar not null,
	excluido bool not null default false,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT plano_aee_observacao_pk PRIMARY KEY (id)
);
ALTER TABLE public.plano_aee_observacao ADD CONSTRAINT plano_aee_observacao_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id);
CREATE INDEX plano_aee_observacao_plano_idx ON public.plano_aee_observacao USING btree (plano_aee_id);


create table public.notificacao_plano_aee_observacao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	plano_aee_observacao_id int8 not null,
	notificacao_id int8 not null,
	excluido bool not null default false,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT notificacao_plano_aee_observacao_pk PRIMARY KEY (id)
);
ALTER TABLE public.notificacao_plano_aee_observacao ADD CONSTRAINT notificacao_plano_aee_observacao_observacao_fk FOREIGN KEY (plano_aee_observacao_id) REFERENCES plano_aee_observacao(id);
ALTER TABLE public.notificacao_plano_aee_observacao ADD CONSTRAINT notificacao_plano_aee_observacao_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);

CREATE INDEX notificacao_plano_aee_observacao_observacao_idx ON public.notificacao_plano_aee_observacao USING btree (plano_aee_observacao_id);
CREATE INDEX notificacao_plano_aee_observacao_notificacao_idx ON public.notificacao_plano_aee_observacao USING btree (notificacao_id);
