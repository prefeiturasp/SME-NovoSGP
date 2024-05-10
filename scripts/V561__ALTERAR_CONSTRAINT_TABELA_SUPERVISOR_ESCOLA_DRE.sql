ALTER TABLE public.supervisor_escola_dre DROP CONSTRAINT if exists supervisor_escola_dre_ck;

ALTER TABLE public.supervisor_escola_dre ADD CONSTRAINT supervisor_escola_dre_ck UNIQUE (supervisor_id, escola_id, dre_id, excluido);