DROP TABLE IF EXISTS public.frequencia_turma_evasao_aluno;

CREATE TABLE public.frequencia_turma_evasao_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	frequencia_turma_evasao_id int8 NOT NULL,
	codigo_aluno varchar(15), 
	nome_aluno varchar(100), 
	percentual_frequencia numeric(5,2),
	CONSTRAINT frequencia_turma_evasao_aluno_pk PRIMARY KEY (id)
);

CREATE INDEX frequencia_turma_evasao_id_idx ON public.frequencia_turma_evasao_aluno USING btree (frequencia_turma_evasao_id);
CREATE INDEX frequencia_turma_evasao_aluno_idx ON public.frequencia_turma_evasao_aluno USING btree (codigo_aluno);

ALTER TABLE public.frequencia_turma_evasao_aluno ADD CONSTRAINT frequencia_turma_evasao_aluno_fk FOREIGN KEY (frequencia_turma_evasao_id) REFERENCES public.frequencia_turma_evasao(id);