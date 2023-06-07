ALTER TABLE public.wf_aprovacao_nota_conselho ADD IF NOT exists nota_anterior numeric(5, 2) NULL;
ALTER TABLE public.wf_aprovacao_nota_conselho ADD IF NOT exists conceito_id_anterior int8 NULL;
ALTER TABLE public.wf_aprovacao_nota_conselho ADD CONSTRAINT wf_aprovacao_nota_conselho_conceito_anterior_fk FOREIGN KEY (conceito_id_anterior) REFERENCES conceito_valores(id);
ALTER TABLE if exists public.wf_aprovacao_nota_conselho ADD column if not exists excluido bool NOT NULL DEFAULT false;                

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD IF NOT exists nota_anterior numeric(5, 2) NULL;
ALTER TABLE public.wf_aprovacao_nota_fechamento ADD IF NOT exists conceito_id_anterior int8 NULL;
ALTER TABLE public.wf_aprovacao_nota_fechamento ADD CONSTRAINT wf_aprovacao_nota_fechamento_conceito_anterior_fk FOREIGN KEY (conceito_id_anterior) REFERENCES conceito_valores(id);