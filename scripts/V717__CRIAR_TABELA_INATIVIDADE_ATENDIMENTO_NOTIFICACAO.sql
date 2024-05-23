CREATE table IF NOT EXISTS public.inatividade_atendimento_naapa_notificacao(
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	encaminhamento_naapa_id int8 NOT NULL,
	notificacao_id int8 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool not null default false,
	CONSTRAINT inatividade_atendimento_naapa_notificacao_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists inatividade_atendimento_naapa_notificacao_encaminhamento_naapa_id_idx ON public.inatividade_atendimento_naapa_notificacao USING btree (encaminhamento_naapa_id);
ALTER TABLE public.inatividade_atendimento_naapa_notificacao DROP CONSTRAINT if exists inatividade_atendimento_naapa_notificacao_encaminhamento_naapa_id_fk;
ALTER TABLE public.inatividade_atendimento_naapa_notificacao ADD CONSTRAINT inatividade_atendimento_naapa_notificacao_encaminhamento_naapa_id_fk FOREIGN KEY (encaminhamento_naapa_id) REFERENCES encaminhamento_naapa(id);

CREATE INDEX if not exists inatividade_atendimento_naapa_notificacao_notificacao_id_idx ON public.inatividade_atendimento_naapa_notificacao USING btree (notificacao_id);
ALTER TABLE public.inatividade_atendimento_naapa_notificacao DROP CONSTRAINT if exists inatividade_atendimento_naapa_notificacao_notificacao_id_fk;
ALTER TABLE public.inatividade_atendimento_naapa_notificacao ADD CONSTRAINT inatividade_atendimento_naapa_notificacao_notificacao_id_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);

