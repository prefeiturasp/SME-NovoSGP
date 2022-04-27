DROP TABLE IF EXISTS public.consolidacao_frequencia_aluno_mensal;

CREATE TABLE public.consolidacao_frequencia_aluno_mensal (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	aluno_codigo varchar(15) not null,
	mes int4 not null,
	percentual numeric(5,2) not null,
	quantidade_aulas int4 not null,
	quantidade_ausencias int4 not null,
	quantidade_compensacoes int4 not null,
	CONSTRAINT consolidacao_frequencia_aluno_mensal_pk PRIMARY KEY (id)
);

CREATE INDEX consolidacao_frequencia_aluno_mensal_turma_id_idx ON public.consolidacao_frequencia_aluno_mensal USING btree (turma_id);
CREATE INDEX consolidacao_frequencia_aluno_mensal_aluno_codigo_idx ON public.consolidacao_frequencia_aluno_mensal USING btree (aluno_codigo);

ALTER TABLE public.consolidacao_frequencia_aluno_mensal ADD CONSTRAINT consolidacao_frequencia_aluno_mensal_turma_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);