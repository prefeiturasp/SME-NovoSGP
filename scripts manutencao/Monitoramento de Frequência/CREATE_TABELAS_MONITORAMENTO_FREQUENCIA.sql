DROP TABLE IF EXISTS public.frequencia_aluno_disciplina_inconsistencia;
CREATE UNLOGGED TABLE public.frequencia_aluno_disciplina_inconsistencia (	
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null
);

DROP TABLE IF EXISTS public.frequencia_aluno_geral_inconsistencia;
CREATE UNLOGGED TABLE public.frequencia_aluno_geral_inconsistencia (	
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null
);

DROP TABLE IF EXISTS public.consolidacao_frequencia_aluno_mensal_inconsistencia;
CREATE UNLOGGED TABLE public.consolidacao_frequencia_aluno_mensal_inconsistencia (	
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null
);
