alter table frequencia_aluno 
	ALTER COLUMN codigo_aluno TYPE int8 using codigo_aluno::bigint;
	
CREATE INDEX IF NOT EXISTS frequencia_aluno_aluno_idx ON public.frequencia_aluno USING btree (codigo_aluno);
CREATE INDEX IF NOT EXISTS frequencia_aluno_disciplina_idx ON public.frequencia_aluno USING btree (disciplina_id);
CREATE INDEX IF NOT EXISTS frequencia_aluno_turma_idx ON public.frequencia_aluno USING btree (turma_id);

CREATE INDEX IF NOT EXISTS periodo_escolar_inicio_idx ON public.periodo_escolar USING btree (periodo_inicio);
CREATE INDEX IF NOT EXISTS periodo_escolar_fim_idx ON public.periodo_escolar USING btree (periodo_fim);
|
CREATE INDEX IF NOT EXISTS periodo_fechamento_bimestre_inicio_idx ON public.periodo_fechamento_bimestre USING btree (inicio_fechamento);
CREATE INDEX IF NOT EXISTS periodo_fechamento_bimestre_fim_idx ON public.periodo_fechamento_bimestre USING btree (final_fechamento);
