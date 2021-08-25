drop index if exists aula_wf_aprovacao_id_idx;
CREATE INDEX aula_wf_aprovacao_id_idx ON public.aula USING btree (wf_aprovacao_id);
