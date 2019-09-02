CREATE TABLE IF NOT EXISTS public.wf_aprova_niveis_notificacao
(
	wf_aprova_niveis_id bigint NOT NULL,
    notificacao_id bigint NOT NULL,
    CONSTRAINT wf_aprova_niveis_mensagem_pk PRIMARY KEY (wf_aprova_niveis_id, notificacao_id)

);

ALTER TABLE public.wf_aprova_niveis_notificacao ADD CONSTRAINT wf_aprova_niveis_notificacao_wf_fk FOREIGN KEY (wf_aprova_niveis_id) REFERENCES wf_aprova_niveis(id);
ALTER TABLE public.wf_aprova_niveis_notificacao ADD CONSTRAINT wf_aprova_niveis_mensagem_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);


