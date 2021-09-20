DROP TABLE if exists public.consolidacao_acompanhamento_aprendizagem_aluno;

CREATE TABLE public.consolidacao_acompanhamento_aprendizagem_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	turma_id int8 NOT NULL,
	quantidade_com_acompanhamento int8 NOT null default 0,
	quantidade_sem_acompanhamento int8 NOT null default 0,
	semestre int NOT null default 0,
	
    CONSTRAINT consolidacao_acompanhamento_aprendizagem_aluno_pk PRIMARY KEY (id)
);

ALTER TABLE public.consolidacao_acompanhamento_aprendizagem_aluno ADD CONSTRAINT consolidacao_acompanhamento_aprendizagem_aluno_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX consolidacao_acompanhamento_aprendizagem_aluno_idx ON public.consolidacao_acompanhamento_aprendizagem_aluno USING btree (turma_id);