DROP TABLE if exists public.consolidacao_frequencia_turma;

CREATE TABLE public.consolidacao_frequencia_turma (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	turma_id int8 NOT NULL,
	quantidade_acima_minimo_frequencia int8 NOT null default 0,
	quantidade_abaixo_minimo_frequencia int8 NOT null default 0,
	
    CONSTRAINT consolidacao_frequencia_turma_pk PRIMARY KEY (id)
);

ALTER TABLE public.consolidacao_frequencia_turma ADD CONSTRAINT consolidacao_frequencia_turma_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
CREATE INDEX consolidacao_frequencia_turma_turma_idx ON public.consolidacao_frequencia_turma USING btree (turma_id);
