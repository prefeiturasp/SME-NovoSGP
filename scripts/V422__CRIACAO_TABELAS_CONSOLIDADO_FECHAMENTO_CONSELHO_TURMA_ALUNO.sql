-- public.consolidado_fechamento_componente_turma definition

-- Drop table

-- DROP TABLE public.consolidado_fechamento_componente_turma;

CREATE TABLE IF NOT EXISTS public.consolidado_fechamento_componente_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dt_atualizacao timestamp NOT NULL,
	status int4 NOT NULL,
	componente_curricular_id int8 NOT NULL,
	professor_nome varchar(100) null,
	professor_rf varchar(12) NOT null,
	turma_id int8 NOT NULL,
	bimestre int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT consolidado_fechamento_componente_turma_pk PRIMARY KEY (id)
);

CREATE INDEX if not exists consolidado_fechamento_componente_turma_turma_id_ix ON public.consolidado_fechamento_componente_turma USING btree (turma_id);
CREATE INDEX if not exists consolidado_fechamento_componente_turma_componente_curricular_id_ix ON public.consolidado_fechamento_componente_turma USING btree (componente_curricular_id);

-- public.consolidado_fechamento_componente_turma foreign keys

alter table if exists public.consolidado_fechamento_componente_turma drop constraint if exists consolidado_fechamento_componente_turma_turma_fk;
alter table if exists public.consolidado_fechamento_componente_turma drop constraint if exists consolidado_fechamento_componente_turma_componente_curricular_fk;

ALTER TABLE public.consolidado_fechamento_componente_turma ADD CONSTRAINT consolidado_fechamento_componente_turma_turma_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);
ALTER TABLE public.consolidado_fechamento_componente_turma ADD CONSTRAINT consolidado_fechamento_componente_turma_componente_curricular_fk FOREIGN KEY (componente_curricular_id) REFERENCES public.componente_curricular(id);

-- public.consolidado_conselho_classe_aluno_turma definition

-- Drop table

-- DROP TABLE public.consolidado_conselho_classe_aluno_turma;

CREATE TABLE IF NOT EXISTS public.consolidado_conselho_classe_aluno_turma (
	id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	dt_atualizacao timestamp NOT NULL,
	status int4 NOT NULL,
	aluno_codigo varchar(15) NOT NULL,
	parecer_conclusivo_id int8 null,
	turma_id int8 NOT NULL,
	bimestre int4 NOT NULL,
	criado_em timestamp NOT NULL,
	criado_por varchar(200) NOT NULL,
	alterado_em timestamp NULL,
	alterado_por varchar(200) NULL,
	criado_rf varchar(200) NOT NULL,
	alterado_rf varchar(200) NULL,
	excluido bool NOT NULL DEFAULT false,
	CONSTRAINT consolidado_conselho_classe_aluno_turma_pk PRIMARY KEY (id)
);
CREATE INDEX if not exists consolidado_conselho_classe_aluno_turma_turma_id_ix ON public.consolidado_conselho_classe_aluno_turma USING btree (turma_id);

alter table if exists public.consolidado_conselho_classe_aluno_turma drop constraint if exists consolidado_conselho_classe_aluno_turma_turma_fk;
alter table if exists public.consolidado_conselho_classe_aluno_turma drop constraint if exists consolidado_conselho_classe_aluno_turma_turma_componente_curricular_fk;


ALTER TABLE public.consolidado_conselho_classe_aluno_turma ADD CONSTRAINT consolidado_conselho_classe_aluno_turma_turma_fk FOREIGN KEY (turma_id) REFERENCES public.turma(id);
ALTER TABLE public.consolidado_conselho_classe_aluno_turma ADD CONSTRAINT consolidado_conselho_classe_aluno_turma_turma_componente_curricular_fk FOREIGN KEY (parecer_conclusivo_id) REFERENCES public.conselho_classe_parecer(id);