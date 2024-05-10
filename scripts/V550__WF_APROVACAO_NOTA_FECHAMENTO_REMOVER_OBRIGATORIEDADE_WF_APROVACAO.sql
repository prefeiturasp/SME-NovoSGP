--> Retirando not null do campo wf_aprovacao_id
ALTER TABLE public.wf_aprovacao_nota_fechamento ALTER COLUMN wf_aprovacao_id DROP NOT NULL;

--> Inserindo campos de auditoria
ALTER TABLE public.wf_aprovacao_nota_fechamento ADD criado_em timestamp NOT NULL default CURRENT_TIMESTAMP;

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD alterado_em timestamp NULL;

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD alterado_por varchar(200) NULL;

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD criado_por varchar(200) NOT NULL default 'Sistema';

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD criado_rf varchar(200) NOT NULL default 'Sistema';

ALTER TABLE public.wf_aprovacao_nota_fechamento ADD alterado_rf varchar(200) NULL;
