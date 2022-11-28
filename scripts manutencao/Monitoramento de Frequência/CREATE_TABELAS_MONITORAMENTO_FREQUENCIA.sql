DROP TABLE IF EXISTS public.frequencia_aluno_disciplina_inconsistencia;
CREATE TABLE public.frequencia_aluno_disciplina_inconsistencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null,
	CONSTRAINT frequencia_aluno_disciplina_inconsistencia_pk PRIMARY KEY (id)
);

DROP TABLE IF EXISTS public.frequencia_aluno_geral_inconsistencia;
CREATE TABLE public.frequencia_aluno_geral_inconsistencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null,
	CONSTRAINT frequencia_aluno_geral_inconsistencia_pk PRIMARY KEY (id)
);

DROP TABLE IF EXISTS public.consolidacao_frequencia_aluno_mensal_inconsistencia;
CREATE TABLE public.consolidacao_frequencia_aluno_mensal_inconsistencia (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	codigo_aluno VARCHAR(15) NOT NULL,
	turma_id VARCHAR(15) NOT NULL,
	bimestre int4 NOT null,
	CONSTRAINT consolidacao_frequencia_aluno_mensal_inconsistencia_pk PRIMARY KEY (id)
);