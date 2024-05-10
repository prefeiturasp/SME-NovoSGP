ALTER TABLE public.aula_prevista DROP COLUMN IF EXISTS aulas_previstas;
ALTER TABLE public.aula_prevista DROP COLUMN IF EXISTS bimestre;

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