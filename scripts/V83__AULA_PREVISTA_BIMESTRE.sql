DROP TABLE if exists public.aula_prevista;
CREATE TABLE public.aula_prevista (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	tipo_calendario_id int8 NOT NULL,
	disciplina_id varchar(15) NOT NULL,
	turma_id varchar(15) NOT NULL,
	
	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT aula_prevista_pk PRIMARY KEY (id)
);

ALTER TABLE public.aula_prevista ADD CONSTRAINT aula_prevista_tipo_calendario_fk FOREIGN KEY (tipo_calendario_id) REFERENCES tipo_calendario(id);

DROP TABLE if exists public.aula_prevista_bimestre;
CREATE TABLE public.aula_prevista_bimestre (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	aula_prevista_id int8 NOT NULL,
	aulas_previstas int4 NOT NULL,
	bimestre int4 NOT NULL,

	criado_em timestamp  NOT NULL,
    criado_por varchar(200) NOT NULL,
    alterado_em timestamp ,
    alterado_por varchar(200),
    criado_rf varchar(200)  NOT NULL,
    alterado_rf varchar(200),
    excluido boolean not null default false,
    
    CONSTRAINT aula_prevista_bimestre_pk PRIMARY KEY (id)
);

ALTER TABLE public.aula_prevista_bimestre ADD CONSTRAINT aula_prevista_bimestre_aula_prevista_fk FOREIGN KEY (aula_prevista_id) REFERENCES aula_prevista(id);