ALTER TABLE public.pendencia ADD ue_id int8;
CREATE INDEX pendencia_ue_idx ON public.pendencia USING btree (ue_id);
ALTER TABLE public.pendencia ADD CONSTRAINT pendencia_ue_fk FOREIGN KEY (ue_id) REFERENCES ue(id);
