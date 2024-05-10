DROP INDEX processo_executando_disciplina_idx ;

ALTER TABLE public.processo_executando ALTER COLUMN disciplina_id TYPE varchar(20);

CREATE INDEX processo_executando_disciplina_idx ON public.processo_executando USING btree (disciplina_id);

ALTER TABLE public.frequencia_aluno ALTER COLUMN disciplina_id TYPE varchar(20);