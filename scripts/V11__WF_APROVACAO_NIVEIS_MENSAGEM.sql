CREATE TABLE IF NOT EXISTS public.wf_aprovacao_nivel_notificacao
(
	id bigint NOT null generated always as identity,
	wf_aprovacao_nivel_id bigint NOT NULL,
    notificacao_id bigint NOT NULL,
    CONSTRAINT wf_aprovacao_nivel_mensagem_pk PRIMARY KEY (wf_aprovacao_nivel_id, notificacao_id)

);

ALTER TABLE public.wf_aprovacao_nivel_notificacao ADD CONSTRAINT wf_aprovacao_nivel_notificacao_wf_fk FOREIGN KEY (wf_aprovacao_nivel_id) REFERENCES wf_aprovacao_nivel(id);
ALTER TABLE public.wf_aprovacao_nivel_notificacao ADD CONSTRAINT wf_aprovacao_nivel_mensagem_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);


