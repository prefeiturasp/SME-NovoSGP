ALTER TABLE public.ocorrencia add column if not exists ue_id int8 null;

ALTER TABLE public.ocorrencia ADD CONSTRAINT ocorrencia_ue_fk
FOREIGN KEY (ue_id) REFERENCES public.ue(id);

CREATE INDEX if not EXISTS ocorrencia_ue_id_idx ON public.ocorrencia USING btree (ue_id);






