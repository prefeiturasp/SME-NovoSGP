ALTER TABLE public.pendencia add column if not exists turma_id int8 null;

ALTER TABLE public.pendencia ADD CONSTRAINT pendencia_turma_fk
FOREIGN KEY (turma_id) REFERENCES public.turma(id);

CREATE INDEX if not EXISTS pendencia_turma_id_idx ON public.pendencia USING btree (turma_id);