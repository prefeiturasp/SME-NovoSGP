CREATE TABLE IF NOT EXISTS public.wf_aprova_nivel_notificacao
(
	wf_aprova_nivel_id bigint NOT NULL,
    notificacao_id bigint NOT NULL,
    CONSTRAINT wf_aprova_nivel_mensagem_pk PRIMARY KEY (wf_aprova_nivel_id, notificacao_id)

);

ALTER TABLE public.wf_aprova_nivel_notificacao ADD CONSTRAINT wf_aprova_nivel_notificacao_wf_fk FOREIGN KEY (wf_aprova_nivel_id) REFERENCES wf_aprova_nivel(id);
ALTER TABLE public.wf_aprova_nivel_notificacao ADD CONSTRAINT wf_aprova_nivel_mensagem_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);


