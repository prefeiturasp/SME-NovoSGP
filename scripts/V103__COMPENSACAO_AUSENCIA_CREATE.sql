-- COMPENSACAO AUSENCIA
DROP TABLE if exists public.compensacao_ausencia;
CREATE TABLE public.compensacao_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	bimestre int4 NOT NULL,
    disciplina_id varchar(15) NOT NULL,
    turma_id int8 NOT NULL,
    nome varchar not null,
    descricao varchar not null,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    migrado boolean not null default false,
    
    CONSTRAINT compensacao_ausencia_pk PRIMARY KEY (id)
);
CREATE INDEX compensacao_ausencia_disciplina_idx ON public.compensacao_ausencia USING btree (disciplina_id);
CREATE INDEX compensacao_ausencia_turma_idx ON public.compensacao_ausencia USING btree (turma_id);
ALTER TABLE public.compensacao_ausencia ADD CONSTRAINT compensacao_ausencia_turma_fk FOREIGN KEY (turma_id) REFERENCES turma(id);

-- COMPENSACAO AUSENCIA x ALUNO
DROP TABLE if exists public.compensacao_ausencia_aluno;
CREATE TABLE public.compensacao_ausencia_aluno (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	compensacao_ausencia_id int8 not null, 
	codigo_aluno varchar(100) not null,
	qtd_faltas_compensadas int4 not null,
	notificado bool not null default false,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT compensacao_ausencia_aluno_pk PRIMARY KEY (id)
);
CREATE INDEX compensacao_ausencia_aluno_compensacao_ausencia_idx ON public.compensacao_ausencia_aluno USING btree (compensacao_ausencia_id);
CREATE INDEX compensacao_ausencia_aluno_codigo_aluno_idx ON public.compensacao_ausencia_aluno USING btree (codigo_aluno);
ALTER TABLE public.compensacao_ausencia_aluno ADD CONSTRAINT compensacao_ausencia_aluno_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id);

-- COMPENSACAO_AUSENCIA x DISCIPLINAS REGENCIA 
DROP TABLE if exists public.compensacao_ausencia_disciplina_regencia;
CREATE TABLE public.compensacao_ausencia_disciplina_regencia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	compensacao_ausencia_id int8 not null, 
	disciplina_id varchar(100) not null,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT compensacao_ausencia_disciplina_regencia_pk PRIMARY KEY (id)
);
CREATE INDEX compensacao_ausencia_disciplina_regencia_compensacao_ausencia_idx ON public.compensacao_ausencia_disciplina_regencia USING btree (compensacao_ausencia_id);
ALTER TABLE public.compensacao_ausencia_disciplina_regencia ADD CONSTRAINT compensacao_ausencia_disciplina_regencia_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id);


-- NOTIFICACAO x COMPENSACAO_AUSENCIA
DROP TABLE if exists public.notificacao_compensacao_ausencia;
CREATE TABLE public.notificacao_compensacao_ausencia (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	notificacao_id int8 not null,
	compensacao_ausencia_id int8 not null, 
	
    CONSTRAINT notificacao_compensacao_ausencia_pk PRIMARY KEY (id)
);
CREATE INDEX notificacao_compensacao_ausencia_notificacao_idx ON public.notificacao_compensacao_ausencia USING btree (notificacao_id);
CREATE INDEX notificacao_compensacao_ausencia_compensacao_ausencia_idx ON public.notificacao_compensacao_ausencia USING btree (compensacao_ausencia_id);
ALTER TABLE public.notificacao_compensacao_ausencia ADD CONSTRAINT notificacao_compensacao_ausencia_notificacao_fk FOREIGN KEY (notificacao_id) REFERENCES notificacao(id);
ALTER TABLE public.notificacao_compensacao_ausencia ADD CONSTRAINT notificacao_compensacao_ausencia_compensacao_fk FOREIGN KEY (compensacao_ausencia_id) REFERENCES compensacao_ausencia(id);
