alter table pendencia_professor add periodo_escolar_id int8 null;
ALTER TABLE public.pendencia_professor ADD CONSTRAINT pendencia_professor_periodo_escolar_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id);
CREATE INDEX pendencia_professor_periodo_escolar_idx ON public.pendencia_professor USING btree (periodo_escolar_id);