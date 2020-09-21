ALTER TABLE public.atividade_avaliativa_disciplina alter column disciplina_id type bigint using disciplina_id::bigint;

ALTER TABLE public.notas_conceito alter column disciplina_id type bigint using disciplina_id::bigint;
ALTER TABLE public.notas_conceito ADD CONSTRAINT notas_conceito_atividade_avaliativa_fk FOREIGN KEY (atividade_avaliativa) REFERENCES atividade_avaliativa(id);
