alter table historico_nota_fechamento drop if exists wf_aprovacao_id;
alter table historico_nota_fechamento add wf_aprovacao_id int8 null;

ALTER TABLE public.historico_nota_fechamento ADD CONSTRAINT historico_nota_fechamento_wf_aprovacao_fk FOREIGN KEY (wf_aprovacao_id) REFERENCES wf_aprovacao(id);
CREATE INDEX if not exists historico_nota_fechamento_wf_aprovacao_idx ON public.historico_nota_fechamento USING btree (wf_aprovacao_id);