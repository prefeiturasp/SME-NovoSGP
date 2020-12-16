ALTER TABLE etl_sgp_fechamento_cc ALTER COLUMN nota TYPE NUMERIC(11,2);
ALTER TABLE etl_sgp_fechamento_cc ALTER COLUMN notacc TYPE NUMERIC(11,2);

UPDATE etl_sgp_fechamento_cc SET nota = nota/10 
WHERE nota is not null;

UPDATE etl_sgp_fechamento_cc SET notacc = notacc/10 
WHERE notacc is not null;

ALTER TABLE conselho_classe_nota ALTER COLUMN nota TYPE NUMERIC(11,2);
ALTER TABLE fechamento_nota ALTER COLUMN nota TYPE NUMERIC(11,2);


CREATE INDEX etl_sgp_fechamento_cc_disciplina_id_idx ON public.etl_sgp_fechamento_cc (disciplina_id);
CREATE INDEX etl_sgp_fechamento_cc_nota_idx ON public.etl_sgp_fechamento_cc (nota,sintese_id,conceito_id);
CREATE INDEX etl_sgp_fechamento_cc_periodo_escolar_id_idx ON public.etl_sgp_fechamento_cc (periodo_escolar_id);
CREATE INDEX etl_sgp_fechamento_cc_aluno_codigo_idx ON public.etl_sgp_fechamento_cc (aluno_codigo);
CREATE INDEX etl_sgp_fechamento_cc_turma_id_idx ON public.etl_sgp_fechamento_cc (turma_id);
CREATE INDEX etl_sgp_fechamento_cc_criado_alterado_idx ON public.etl_sgp_fechamento_cc (criado_em,alterado_em);
CREATE INDEX etl_sgp_fechamento_acerto_fechamento_turma_id_idx ON public.etl_sgp_fechamento_acerto (fechamento_turma_id);
CREATE INDEX fechamento_turma_periodo_escolar_id_idx ON public.fechamento_turma (periodo_escolar_id);
CREATE INDEX etl_sgp_fechamento_acerto_col_idx ON public.etl_sgp_fechamento_acerto (col);
CREATE INDEX fechamento_turma_periodo_escolar_is_null_idx ON public.fechamento_turma ((periodo_escolar_id IS NULL));
CREATE INDEX etl_sgp_fechamento_cc_periodo_escolar_is_null_idx ON public.etl_sgp_fechamento_cc ((periodo_escolar_id IS NULL));
CREATE INDEX etl_sgp_fechamento_cc_nota_is_not_null_idx ON public.etl_sgp_fechamento_cc ((nota IS NOT NULL));
CREATE INDEX etl_sgp_fechamento_cc_sintese_id_is_not_null_idx ON public.etl_sgp_fechamento_cc ((sintese_id IS NOT NULL));
CREATE INDEX etl_sgp_fechamento_cc_conceito_id_is_not_null_idx ON public.etl_sgp_fechamento_cc ((conceito_id IS NOT NULL));