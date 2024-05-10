CREATE INDEX periodo_escolar_tipo_calendario_id_idx ON public.periodo_escolar (tipo_calendario_id);

CREATE INDEX aula_prevista_tipo_calendario_id_idx ON public.aula_prevista (tipo_calendario_id);
CREATE INDEX aula_prevista_disciplina_id_idx ON public.aula_prevista (disciplina_id);
CREATE INDEX aula_prevista_turma_id_idx ON public.aula_prevista (turma_id);

CREATE INDEX aula_prevista_bimestre_aula_prevista_id_idx ON public.aula_prevista_bimestre (aula_prevista_id);
