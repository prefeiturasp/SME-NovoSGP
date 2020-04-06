drop table if exists fechamento_final;

delete from pendencia_fechamento;
delete from nota_conceito_bimestre;
delete from anotacao_aluno_fechamento;
delete from fechamento_turma_disciplina;

-- FECHAMENTO_TURMA
CREATE TABLE public.fechamento_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	turma_id int8 not null,
	periodo_escolar_id int8 null,

	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_turma_pk PRIMARY KEY (id)
);
ALTER TABLE public.fechamento_turma ADD CONSTRAINT fechamento_turma_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);
ALTER TABLE public.fechamento_turma ADD CONSTRAINT fechamento_turma_periodo_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id);

CREATE INDEX fechamento_turma_turma_idx ON public.fechamento_turma USING btree (turma_id);
CREATE INDEX fechamento_turma_periodo_idx ON public.fechamento_turma USING btree (periodo_escolar_id);

-- FECHAMENTO_TURMA_DISCIPLINA
ALTER TABLE fechamento_turma_disciplina drop column turma_id;
ALTER TABLE fechamento_turma_disciplina drop column periodo_escolar_id;

ALTER TABLE fechamento_turma_disciplina add column fechamento_turma_id int8 not null;
ALTER TABLE public.fechamento_turma_disciplina ADD CONSTRAINT fechamento_turma_disciplina_fechamento_fk FOREIGN KEY (fechamento_turma_id) REFERENCES fechamento_turma(id);
CREATE INDEX fechamento_turma_disciplina_fechamento_idx ON public.fechamento_turma_disciplina USING btree (fechamento_turma_id);

-- FECHAMENTO_ALUNO
CREATE TABLE public.fechamento_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 not null,
	aluno_codigo varchar(15) not null,
	anotacao varchar null,

	migrado bool NOT NULL DEFAULT false,
	excluido bool NOT NULL DEFAULT false,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT fechamento_aluno_pk PRIMARY KEY (id)
);
ALTER TABLE public.fechamento_aluno ADD CONSTRAINT fechamento_aluno_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id);
CREATE INDEX fechamento_aluno_fechamento_idx ON public.fechamento_aluno USING btree (fechamento_turma_disciplina_id);
CREATE INDEX fechamento_aluno_aluno_idx ON public.fechamento_aluno USING btree (aluno_codigo);

-- FECHAMENTO_NOTA
alter table nota_conceito_bimestre rename to fechamento_nota;
alter table fechamento_nota drop column fechamento_turma_disciplina_id;
alter table fechamento_nota drop column codigo_aluno;

ALTER TABLE fechamento_nota add column fechamento_aluno_id int8 not null;
ALTER TABLE public.fechamento_nota ADD CONSTRAINT fechamento_nota_aluno_fk FOREIGN KEY (fechamento_aluno_id) REFERENCES fechamento_aluno(id);
CREATE INDEX fechamento_nota_aluno_idx ON public.fechamento_nota USING btree (fechamento_aluno_id);
