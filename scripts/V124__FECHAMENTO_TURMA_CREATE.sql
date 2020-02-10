DROP TABLE if exists public.nota_conceito_bimestre;
DROP TABLE if exists public.fechamento_turma_disciplina;

CREATE TABLE public.fechamento_turma_disciplina (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	periodo_fechamento_bimestre_id int8 not null,
	turma_id int8 not null,
	disciplina_id int8 not null,

	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_turma_disciplina_pk PRIMARY KEY (id)
);
CREATE INDEX fechamento_turma_disciplina_turma_idx ON public.fechamento_turma_disciplina USING btree (turma_id);
CREATE INDEX fechamento_turma_disciplina_disciplina_idx ON public.fechamento_turma_disciplina USING btree (disciplina_id);

ALTER TABLE public.fechamento_turma_disciplina ADD CONSTRAINT fechamento_turma_disciplina_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
ALTER TABLE public.fechamento_turma_disciplina ADD CONSTRAINT fechamento_turma_disciplina_bimestre_fk FOREIGN KEY (periodo_fechamento_bimestre_id) REFERENCES periodo_fechamento_bimestre(id);


CREATE TABLE public.nota_conceito_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 not null,
	disciplina_id int8 not null,
	codigo_aluno varchar(15) not null,
	nota numeric(5,2) null,
	conceito_id int8 null,

	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT nota_conceito_bimestre_pk PRIMARY KEY (id)
);
CREATE INDEX nota_conceito_bimestre_disciplina_idx ON public.nota_conceito_bimestre USING btree (disciplina_id);
CREATE INDEX nota_conceito_bimestre_aluno_idx ON public.nota_conceito_bimestre USING btree (codigo_aluno);

ALTER TABLE public.nota_conceito_bimestre ADD CONSTRAINT nota_conceito_bimestre_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id);
ALTER TABLE public.nota_conceito_bimestre ADD CONSTRAINT nota_conceito_bimestre_conceito_fk FOREIGN KEY (conceito_id) REFERENCES conceito_valores(id);