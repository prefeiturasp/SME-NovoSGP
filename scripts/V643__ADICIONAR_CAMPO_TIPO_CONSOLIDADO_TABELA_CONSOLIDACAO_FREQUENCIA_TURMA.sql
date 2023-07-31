ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists tipo_consolidacao int4 NOT NULL default 3;
ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists periodo_inicio timestamp NULL;
ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists periodo_fim timestamp NULL;
ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists total_aulas int4 NULL;
ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists total_frequencias int4 NULL;
ALTER TABLE public.consolidacao_frequencia_turma ADD column IF NOT exists total_alunos int4 NULL;