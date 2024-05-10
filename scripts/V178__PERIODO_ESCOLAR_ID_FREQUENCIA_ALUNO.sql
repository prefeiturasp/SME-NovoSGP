ALTER TABLE public.frequencia_aluno add if not exists periodo_escolar_id int8 NULL;

ALTER TABLE public.frequencia_aluno ADD CONSTRAINT frequencia_aluno_periodo_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id);

CREATE INDEX frequencia_aluno_periodo_idx ON public.frequencia_aluno USING btree (periodo_escolar_id);