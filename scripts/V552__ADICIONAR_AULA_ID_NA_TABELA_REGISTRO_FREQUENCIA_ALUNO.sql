ALTER TABLE public.registro_frequencia_aluno add column if not exists aula_id int8;

ALTER TABLE public.registro_frequencia_aluno ADD CONSTRAINT registro_frequencia_aluno_aula_fk
FOREIGN KEY (aula_id) REFERENCES public.aula(id);

CREATE INDEX if not EXISTS registro_frequencia_aluno_aula_id_idx ON public.registro_frequencia_aluno USING btree (aula_id);