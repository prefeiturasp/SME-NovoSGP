-- LIMPAR REGISTROS EXISTENTES
delete from nota_conceito_bimestre;
delete from fechamento_turma_disciplina;
delete from pendencia;
delete from fechamento;

-- ALTERA REFERENCIA DE PERIODO FECHAMENTO para PERIODO ESCOLAR
ALTER TABLE public.fechamento_turma_disciplina DROP CONSTRAINT if exists fechamento_turma_disciplina_bimestre_fk;
ALTER TABLE public.fechamento_turma_disciplina DROP COLUMN if exists periodo_fechamento_bimestre_id;
ALTER TABLE public.fechamento_turma_disciplina ADD COLUMN if not exists periodo_escolar_id int8 null;
ALTER TABLE public.fechamento_turma_disciplina DROP CONSTRAINT if exists fechamento_turma_disciplina_periodo_fk;
ALTER TABLE public.fechamento_turma_disciplina ADD CONSTRAINT fechamento_turma_disciplina_periodo_fk FOREIGN KEY (periodo_escolar_id) REFERENCES periodo_escolar(id);
ALTER TABLE public.fechamento_turma_disciplina DROP COLUMN if exists situacao;
ALTER TABLE public.fechamento_turma_disciplina ADD COLUMN situacao int4 not null;

CREATE INDEX if not exists fechamento_turma_disciplina_periodo_id_idx ON public.fechamento_turma_disciplina (periodo_escolar_id);
CREATE INDEX if not exists fechamento_turma_disciplina_periodo_id_idx ON public.fechamento_turma_disciplina (situacao);

-- PENDENCIA x FECHAMENTO
alter table pendencia drop column if exists fechamento_id;

DROP TABLE if exists public.pendencia_fechamento;
CREATE TABLE public.pendencia_fechamento (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	fechamento_turma_disciplina_id int8 not null,
	pendencia_id int8 not null,

	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	CONSTRAINT pendencia_fechamento_pk PRIMARY KEY (id)
);
ALTER TABLE public.pendencia_fechamento DROP CONSTRAINT if exists pendencia_fechamento_fechamento_fk;
ALTER TABLE public.pendencia_fechamento DROP CONSTRAINT if exists pendencia_fechamento_pendencia_fk;
ALTER TABLE public.pendencia_fechamento ADD CONSTRAINT pendencia_fechamento_fechamento_fk FOREIGN KEY (fechamento_turma_disciplina_id) REFERENCES fechamento_turma_disciplina(id);
ALTER TABLE public.pendencia_fechamento ADD CONSTRAINT pendencia_fechamento_pendencia_fk FOREIGN KEY (pendencia_id) REFERENCES pendencia(id);

CREATE INDEX if not exists pendencia_fechamento_fechamento_turma_id_idx ON public.pendencia_fechamento (fechamento_turma_disciplina_id);
CREATE INDEX if not exists pendencia_fechamento_pendencia_id_idx ON public.pendencia_fechamento (pendencia_id);

DROP TABLE if exists fechamento;