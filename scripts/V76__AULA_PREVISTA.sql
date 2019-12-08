DROP TABLE if exists public.aula_prevista;
CREATE TABLE public.aula_prevista (
	id int8 NOT NULL GENERATED ALWAYS AS identity,
	aulas_previstas int4 NOT NULL,
	bimestre int4 NOT NULL,
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

