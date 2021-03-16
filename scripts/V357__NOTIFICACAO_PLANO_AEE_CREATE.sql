DROP TABLE if exists public.notificacao_plano_aee;
CREATE TABLE public.notificacao_plano_aee (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	tipo int4 NOT NULL,
	notificacao_id int8 NOT NULL,
    plano_aee_id int8 NOT NULL,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT notificacao_plano_aee_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_plano_aee_idx ON public.notificacao_plano_aee USING btree (plano_aee_id);
CREATE INDEX notificacao_idx ON public.notificacao_plano_aee USING btree (notificacao_id);

ALTER TABLE public.notificacao_plano_aee ADD CONSTRAINT notificacao_plano_aee_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);
ALTER TABLE public.notificacao_plano_aee ADD CONSTRAINT notificacao_plano_aee_plano_fk FOREIGN KEY (plano_aee_id) REFERENCES plano_aee(id);