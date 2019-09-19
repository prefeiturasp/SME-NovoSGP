CREATE TABLE IF NOT EXISTS public.wf_aprovacao_nivel_usuario
(
	id bigint NOT null generated always as identity,
	wf_aprovacao_nivel_id bigint NOT NULL,
    usuario_id bigint NOT NULL,
    CONSTRAINT wf_aprovacao_nivel_usuario_pk PRIMARY KEY (wf_aprovacao_nivel_id, usuario_id)

);

ALTER TABLE public.wf_aprovacao_nivel_usuario ADD CONSTRAINT wf_aprovacao_nivel_usuario_wf_fk FOREIGN KEY (wf_aprovacao_nivel_id) REFERENCES wf_aprovacao_nivel(id);
ALTER TABLE public.wf_aprovacao_nivel_usuario ADD CONSTRAINT wf_aprovacao_nivel_mensagem_usuario_fk FOREIGN KEY (usuario_id) REFERENCES usuario(id);


