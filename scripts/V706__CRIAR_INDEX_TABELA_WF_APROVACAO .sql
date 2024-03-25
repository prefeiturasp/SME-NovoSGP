CREATE INDEX IF NOT EXISTS idx_wf_aprovacao_nivel_notificacao_w3
    ON public.wf_aprovacao_nivel_notificacao USING btree
    (wf_aprovacao_nivel_id ASC NULLS LAST, notificacao_id ASC NULLS LAST)
    TABLESPACE pg_default;
 
CREATE INDEX IF NOT EXISTS idx_wf_aprovacao_nivel_w3
    ON public.wf_aprovacao_nivel USING btree
    (wf_aprovacao_id ASC NULLS LAST)
    TABLESPACE pg_default;