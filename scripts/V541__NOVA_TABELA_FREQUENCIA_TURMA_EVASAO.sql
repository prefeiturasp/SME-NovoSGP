DROP TABLE IF EXISTS public.frequencia_turma_evasao;

CREATE TABLE public.frequencia_turma_evasao (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 NOT NULL,
	mes int4,
	quantidade_alunos_abaixo_50_porcento int4 not null,
	quantidade_alunos_0_porcento int4 not null,
	CONSTRAINT frequencia_turma_evasao_pk PRIMARY KEY (id)
);

CREATE INDEX frequencia_turma_evasao_turma_id_idx ON public.frequencia_turma_evasao USING btree (turma_id);
CREATE INDEX frequencia_turma_evasao_mes_idx ON public.frequencia_turma_evasao USING btree (mes);

ALTER TABLE public.frequencia_turma_evasao ADD CONSTRAINT frequencia_turma_evasao_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);